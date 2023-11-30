using System.IO;
using System.Text;

namespace Trane.Submittals.Pipeline
{
  public class TbfRecord
  {
    //private float[,] m_TransformationMatrix;

    public TbfRecord()
    {
      //m_TransformationMatrix = new float[4,4];
    }

    // this version writes out the tag, but no data is output (TR_NODATA)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode)
    {
      return WriteTbfTag(tbfBinaryWriter, tagCode, 0, 0);
    }

    // this version writes out the tag along with one string (TR_STRING)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, string szOutput)
    {
      bool bRetVal = true;
      //int		stringLength = szOutput.Length + 1;

      szOutput += "\0";
      //int		stringLength = szOutput.Length;

      if (szOutput.Length <= 255)
      {
        //bRetVal = WriteTbfTag(tagCode, (byte)TagTypes.TR_STRING, (byte)stringLength);
        bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_STRING, (byte)szOutput.Length);
        if (bRetVal)
        {
          try
          {
            //m_TbfBinaryWriter.Write(szOutput);
            tbfBinaryWriter.Write(Encoding.ASCII.GetBytes(szOutput));
            //outputByteArray.AddRange(Encoding.ASCII.GetBytes(szOutput));
            //m_TbfStreamWriter.Write('\0');
          }
          catch
          {
            bRetVal = false;
          }
        }
      }
      else
      {
        bRetVal = false;
      }
      return bRetVal;
    }

    // this version writes out the tag along with one integer (TR_1INT)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, int nVal)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_1INT, (byte)TbfConstants.IntSize);

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(nVal);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    // this version writes out the tag along with one float (TR_1REAL)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, float fVal)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_1REAL, (byte)TbfConstants.FloatSize);

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(fVal);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    // this version writes out the tag along with two floats (TR_2REAL)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, float fVal1, float fVal2)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_2REAL, (byte)(2 * (int)TbfConstants.FloatSize));

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(fVal1);
          tbfBinaryWriter.Write(fVal2);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    // this version writes out the tag along with three floats, 1 3D Points (TR_3REAL)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, TbfPoint3 point1)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_3REAL, (byte)(3 * (int)TbfConstants.FloatSize));

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(point1.X);
          tbfBinaryWriter.Write(point1.Y);
          tbfBinaryWriter.Write(point1.Z);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    // this version writes out the tag along with four floats (TR_4REAL)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, float fVal1, float fVal2, float fVal3, float fVal4)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_4REAL, (byte)(4 * (int)TbfConstants.FloatSize));

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(fVal1);
          tbfBinaryWriter.Write(fVal2);
          tbfBinaryWriter.Write(fVal3);
          tbfBinaryWriter.Write(fVal4);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    // this version writes out the tag along with six floats, 2 3D Points (TR_6REAL)
    public bool WriteTbfTagAndData(BinaryWriter tbfBinaryWriter, byte tagCode, TbfPoint3 point1, TbfPoint3 point2)
    {
      bool bRetVal = true;

      bRetVal = WriteTbfTag(tbfBinaryWriter, tagCode, (byte)TbfTagType.TR_6REAL, (byte)(6 * (int)TbfConstants.FloatSize));

      if (bRetVal)
      {
        try
        {
          tbfBinaryWriter.Write(point1.X);
          tbfBinaryWriter.Write(point1.Y);
          tbfBinaryWriter.Write(point1.Z);
          tbfBinaryWriter.Write(point2.X);
          tbfBinaryWriter.Write(point2.Y);
          tbfBinaryWriter.Write(point2.Z);
        }
        catch
        {
          bRetVal = false;
        }
      }
      return bRetVal;
    }

    private bool WriteTbfTag(BinaryWriter tbfBinaryWriter, byte tagCode, byte tagType, byte tagLength)
    {
      bool bRetVal = true;
      byte[] tagArray = new byte[4];

      try
      {
        tagArray[0] = tagCode;
        tagArray[1] = 0;     // the extra code is always zero (future use)
        tagArray[2] = tagType;
        tagArray[3] = tagLength;
        tbfBinaryWriter.Write(tagArray);
      }
      catch
      {
        bRetVal = false;
      }
      return bRetVal;
    }
  }
}
