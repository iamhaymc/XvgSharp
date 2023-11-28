using System.Text;

namespace Xvg;

public partial class TextReader
{
  public string Source { get; set; }
  public int Index { get; set; }
  public int Line { get; set; }
  public int Column { get; set; }
  public StringBuilder Buffer { get; private set; } = new StringBuilder();

  public TextReader Reset(string source)
  {
    Source = source;
    Index = 0;
    Line = Column = 1;
    Buffer ??= new StringBuilder();
    Buffer?.Clear();
    return this;
  }

  #region [Parsing]

  public Vector2? ParseVector2()
  {
    double? x = ParseNumber32();
    if (!x.HasValue)
      return null;
    SkipSpace();
    if (TestCharacter(','))
      PullCharacter();
    double? y = ParseNumber32();
    if (!y.HasValue)
      return null;
    return new Vector2((float)x.Value, (float)y.Value);
  }

  public float? ParseNumber32() => (float?)ParseNumber();
  public double? ParseNumber()
  {
    SkipSpace();
    if (TestCharacter('-', '.') && IsDigitCharacter(PeekCharacter(1)))
      PushBuffer();
    SkipSpace();
    if (IsDigitCharacter(PeekCharacter()))
    {
      while (IsDigitCharacter(PeekCharacter()))
        PushBuffer();
      if (TestCharacter('.'))
      {
        PushBuffer();
        while (IsDigitCharacter(PeekCharacter()))
          PushBuffer();
      }
      return double.TryParse(DumpBuffer(), out double xf)
          ? xf : (double?)null;
    }
    return null;
  }

  public int? ParseInteger32() => (int?)ParseInteger();
  public long? ParseInteger()
  {
    SkipSpace();
    if (TestCharacter('-') && IsDigitCharacter(PeekCharacter(1)))
      PushBuffer();
    SkipSpace();
    if (IsDigitCharacter(PeekCharacter()))
    {
      while (IsDigitCharacter(PeekCharacter()))
        PushBuffer();
      return long.TryParse(DumpBuffer(), out long xf)
          ? xf : (long?)null;
    }
    return null;
  }

  public bool? ParseBoolean()
  {
    SkipSpace();
    if (TestCharacter('1', '0')
        || TestString("true", "false"))
    {
      PushBuffer();
      return DumpBuffer() == "1";
    }
    return null;
  }

  #endregion

  #region [Buffering]

  public string DumpBuffer()
  {
    string text = Buffer?.ToString();
    Buffer?.Clear();
    return text;
  }

  public string PushBuffer(int length)
  {
    string s = PullString(length);
    Buffer.Append(s);
    return s;
  }

  public char PushBuffer()
  {
    char c = PullCharacter();
    Buffer.Append(c);
    return c;
  }

  #endregion

  #region [Scanning]

  public void SkipSpace()
  {
    while (IsSpaceCharacter(PeekCharacter()))
      PullCharacter();
  }

  public string PullString(int length)
  {
    if (Index + length < Source.Length)
      return Source.Substring(Index, Math.Min(Index + length, Source.Length));
    return null;
  }

  public bool TestString(string expected)
  {
    return Source.IndexOf(expected, Index) == 0;
  }

  public bool TestString(params string[] expected)
  {
    return expected.Any(s => Source.IndexOf(s, Index) == 0);
  }

  public string PeekString(int length)
  {
    string s = null;
    if (Index + length < Source.Length)
      s = Source.Substring(Index, length);
    return s;
  }

  public char PullCharacter()
  {
    char c = (char)0;
    if (Index < Source.Length)
    {
      c = Source[Index++];
      if (!IsLineCharacter(c))
        Column++;
      else
      {
        Line++;
        Column = 1;
      }
      return c;
    }
    return c;
  }

  public bool TestCharacter(params char[] expected)
  {
    return expected.Any(c => PeekCharacter() == c);
  }

  public bool TestCharacter(char expected)
  {
    return PeekCharacter() == expected;
  }

  public char PeekCharacter(int k = 0)
  {
    char c = (char)0;
    if (Index + k < Source.Length)
      c = Source[Index + k];
    return c;
  }

  public bool IsLetterCharacter(char c)
    => !IsNullCharacter(c) && char.IsLetter(c);

  public bool IsDigitCharacter(char c)
    => !IsNullCharacter(c) && char.IsDigit(c);

  public bool IsSpaceCharacter(char c)
    => !IsNullCharacter(c) && char.IsWhiteSpace(c);

  public bool IsLineCharacter(char c) => c == '\n';

  public bool IsNullCharacter(char c) => c == (char)0;

  #endregion
}
