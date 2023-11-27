namespace Xvg;

public enum TextJustifyType
{
  Start, Middle, End
}

public enum TextAlignType
{
  Bottom, Middle, Top
}

public static partial class TextStyle
{
  public static readonly TextJustifyType DefaultJustify = TextJustifyType.Start;
  public static readonly TextAlignType DefaultAlign = TextAlignType.Bottom;

  public const string JustifyStart = "start";
  public const string JustifyMiddle = "middle";
  public const string JustifyEnd = "end";

  public const string AlignBottom = "after-edge";
  public const string AlignMiddle = "middle";
  public const string AlignTop = "before-edge";

  public static string ToStyle(this TextJustifyType self)
  {
    switch (self)
    {
      case TextJustifyType.Start: return JustifyStart;
      case TextJustifyType.Middle: return JustifyMiddle;
      case TextJustifyType.End: return JustifyEnd;
      default: throw new NotSupportedException();
    }
  }
  public static string ToStyle(this TextAlignType self)
  {
    switch (self)
    {
      case TextAlignType.Bottom: return AlignBottom;
      case TextAlignType.Middle: return AlignMiddle;
      case TextAlignType.Top: return AlignTop;
      default: throw new NotSupportedException();
    }
  }
}
