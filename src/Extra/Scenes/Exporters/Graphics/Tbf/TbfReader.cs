using System;
using System.Collections.Generic;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  public class TbfReader : IDisposable
  {
    public static TbfReader FromData(Stream tbfStream, bool leaveOpen = false)
    {
      return new TbfReader(tbfStream, leaveOpen);
    }

    public static TbfReader FromEntry(string tbfZipPath, string tbfEntryPath)
    {
      var stream = new MemoryStream();
      if (FsZip.EntryIn(tbfZipPath, tbfEntryPath, stream))
      {
        stream.Position = 0;
        return new TbfReader(stream);
      }
      else
      {
        stream?.Dispose();
        throw new Exception($"Failure to get TBF entry {tbfEntryPath} from zip {tbfZipPath}");
      }
      //using (ClrZip tbfZip = ClrZip.OpenRead(tbfZipPath))
      //{
      //  ClrZipEntry tbfEntry = tbfZip.OpenEntryByName(tbfEntryPath);
      //  return new TbfReader(tbfEntry.Read());
      //}
    }

    BinaryReader _reader;

    public TbfReader(Stream tbfStream, bool leaveOpen = false)
    {
      _reader = new BinaryReader(tbfStream, TbfConstants.Encoding, leaveOpen);
    }

    public void Dispose()
    {
      _reader?.Dispose();
    }

    public TbfTagCode? PeekTag()
    {
      var nextByte = _reader.PeekChar();
      return Enum.IsDefined(typeof(TbfTagCode), nextByte) ? (TbfTagCode)nextByte : (TbfTagCode?)null;
    }

    public TbfReader SkipTag(bool validate = true)
    {
      byte[] tagMeta = _reader.ReadBytes(4);
      if (tagMeta.Length != 4) // EOF
        return null;

      int tagCode = (int)tagMeta[0];
      if (validate && !Enum.IsDefined(typeof(TbfTagCode), tagCode))
        throw new Exception($"Invalid tag code '{tagCode}'");

      //byte tagCodeX = tagMeta[1]; // the extra code is always zero (future use)

      int tagType = (int)tagMeta[2];
      if (validate && !Enum.IsDefined(typeof(TbfTagType), tagType))
        throw new Exception($"Invalid tag type '{tagType}'");

      int tagSize = (int)tagMeta[3];

      if (tagSize > 0)
        _reader.ReadBytes(tagSize);

      return this;
    }

    public ITbfTagToken ParseTag()
    {
      byte[] tagMeta = _reader.ReadBytes(4);
      if (tagMeta.Length != 4) // EOF
        return null;

      int tagCode = (int)tagMeta[0];
      if (!Enum.IsDefined(typeof(TbfTagCode), tagCode))
        throw new Exception($"Invalid tag code '{tagCode}'");

      //byte tagCodeX = tagMeta[1]; // the extra code is always zero (future use)

      int tagType = (int)tagMeta[2];
      if (!Enum.IsDefined(typeof(TbfTagType), tagType))
        throw new Exception($"Invalid tag type '{tagType}'");

      int tagSize = (int)tagMeta[3];

      switch ((TbfTagType)tagType)
      {
        case TbfTagType.TR_1CHAR:
          if (tagSize == TbfConstants.CharSize * 1)
            return new TbfCharacterTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadChar() });
          else if (tagSize == TbfConstants.ByteSize * 1)
            return new TbfBinaryTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadByte() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_2CHAR:
          if (tagSize == TbfConstants.CharSize * 2)
            return new TbfCharacterTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadChar(), _reader.ReadChar() });
          else if (tagSize == TbfConstants.ByteSize * 2)
            return new TbfBinaryTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadByte(), _reader.ReadByte() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_1INT:
          if (tagSize == TbfConstants.LongSize * 1)
            return new TbfLongTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt64() });
          else if (tagSize == TbfConstants.IntSize * 1)
            return new TbfIntegerTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt32() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_2INT:
          if (tagSize == TbfConstants.LongSize * 2)
            return new TbfLongTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt64(), _reader.ReadInt64() });
          else if (tagSize == TbfConstants.IntSize * 2)
            return new TbfIntegerTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt32(), _reader.ReadInt32() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_3INT:
          if (tagSize == TbfConstants.LongSize * 3)
            return new TbfLongTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt64(), _reader.ReadInt64(), _reader.ReadInt64() });
          else if (tagSize == TbfConstants.IntSize * 3)
            return new TbfIntegerTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadInt32(), _reader.ReadInt32(), _reader.ReadInt32() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_1REAL:
          if (tagSize == TbfConstants.DoubleSize * 1)
            return new TbfDoubleTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadDouble() });
          else if (tagSize == TbfConstants.FloatSize * 1)
            return new TbfFloatTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadSingle() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_2REAL:
          if (tagSize == TbfConstants.DoubleSize * 2)
            return new TbfDoubleTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadDouble(), _reader.ReadDouble() });
          else if (tagSize == TbfConstants.FloatSize * 2)
            return new TbfFloatTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadSingle(), _reader.ReadSingle() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_3REAL:
          if (tagSize == TbfConstants.DoubleSize * 3)
            return new TbfDoubleTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble() });
          else if (tagSize == TbfConstants.FloatSize * 3)
            return new TbfFloatTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_4REAL:
          if (tagSize == TbfConstants.DoubleSize * 4)
            return new TbfDoubleTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble() });
          else if (tagSize == TbfConstants.FloatSize * 4)
            return new TbfFloatTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_6REAL:
          if (tagSize == TbfConstants.DoubleSize * 6)
            return new TbfDoubleTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble(), _reader.ReadDouble() });
          else if (tagSize == TbfConstants.FloatSize * 6)
            return new TbfFloatTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, new[] { _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle() });
          throw new Exception($"Invalid tag size");
        case TbfTagType.TR_STRING:
          if (tagSize <= 0)
            throw new Exception($"Invalid tag size");
          string str = TbfConstants.Encoding.GetString(_reader.ReadBytes(tagSize));
          if (str.EndsWith("\0"))
            str = str.Substring(0, str.Length - 1);
          return new TbfStringTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, str);
        case TbfTagType.TR_NODATA:
        default:
          return new TbfBinaryTagToken((TbfTagCode)tagCode, (TbfTagType)tagType, null);
      }
    }

    public IEnumerable<ITbfTagToken> YieldTags()
    {
      for (ITbfTagToken tag = ParseTag(); tag != null;)
      {
        yield return tag;
        tag = ParseTag();
      }
    }
  }

  public interface ITbfTagToken
  {
    TbfTagCode Code { get; }
    TbfTagType Type { get; }
  }

  public abstract class TbfTagToken<TData> : ITbfTagToken
  {
    public TbfTagCode Code { get; private set; }
    public TbfTagType Type { get; private set; }
    public TData Data { get; private set; }

    public TbfTagToken(TbfTagCode code, TbfTagType type, TData data)
    {
      Code = code;
      Type = type;
      Data = data;
    }
  }

  public class TbfBinaryTagToken : TbfTagToken<byte[]>
  {
    public TbfBinaryTagToken(TbfTagCode code, TbfTagType type, byte[] data) : base(code, type, data) { }
  }

  public class TbfCharacterTagToken : TbfTagToken<char[]>
  {
    public TbfCharacterTagToken(TbfTagCode code, TbfTagType type, char[] data) : base(code, type, data) { }
  }

  public class TbfIntegerTagToken : TbfTagToken<int[]>
  {
    public TbfIntegerTagToken(TbfTagCode code, TbfTagType type, int[] data) : base(code, type, data) { }
  }

  public class TbfLongTagToken : TbfTagToken<long[]>
  {
    public TbfLongTagToken(TbfTagCode code, TbfTagType type, long[] data) : base(code, type, data) { }
  }

  public class TbfFloatTagToken : TbfTagToken<float[]>
  {
    public TbfFloatTagToken(TbfTagCode code, TbfTagType type, float[] data) : base(code, type, data) { }
  }

  public class TbfDoubleTagToken : TbfTagToken<double[]>
  {
    public TbfDoubleTagToken(TbfTagCode code, TbfTagType type, double[] data) : base(code, type, data) { }
  }

  public class TbfStringTagToken : TbfTagToken<string>
  {
    public TbfStringTagToken(TbfTagCode code, TbfTagType type, string data) : base(code, type, data) { }
  }
}
