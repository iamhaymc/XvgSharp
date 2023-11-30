namespace Xvg;

public class TextStyle : ISceneStyle
{
  public string Value { get; set; }
  public Vector2? Position { get; set; }
  public TextJustifyType? Justify { get; set; }
  public TextAlignType? Align { get; set; }
}

public enum TextJustifyType
{
  Start, Middle, End
}

public enum TextAlignType
{
  Bottom, Middle, Top
}

public static partial class TextTypeSvgExtensions
{
  public const string SvgJustifyStart = "start";
  public const string SvgJustifyMiddle = "middle";
  public const string SvgJustifyEnd = "end";

  public const string SvgAlignBottom = "after-edge";
  public const string SvgAlignMiddle = "middle";
  public const string SvgAlignTop = "before-edge";

  public static string ToSvgStyle(this TextJustifyType self)
  {
    switch (self)
    {
      case TextJustifyType.Start: return SvgJustifyStart;
      case TextJustifyType.Middle: return SvgJustifyMiddle;
      case TextJustifyType.End: return SvgJustifyEnd;
      default: throw new NotSupportedException();
    }
  }
  public static string ToSvgStyle(this TextAlignType self)
  {
    switch (self)
    {
      case TextAlignType.Bottom: return SvgAlignBottom;
      case TextAlignType.Middle: return SvgAlignMiddle;
      case TextAlignType.Top: return SvgAlignTop;
      default: throw new NotSupportedException();
    }
  }
}
