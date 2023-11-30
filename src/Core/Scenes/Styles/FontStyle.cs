namespace Xvg;

public class FontStyle : ISceneStyle
{
  public FontFamilyType? Family { get; set; }
  public FontWeightType? Weight { get; set; }
  public FontStyleType? Style { get; set; }
  public float? Size { get; set; }
}

public enum FontFamilyType
{
  SansSerif,
  Serif,
  Monospace,
}

public enum FontWeightType
{
  Normal, Bold
}

public enum FontStyleType
{
  Normal, Italic
}

public static partial class FontTypeSvgExtensions
{
  public const string SvgFamilyOpenSans = "Open Sans";
  public const string SvgFamilyLora = "Lora";
  public const string SvgFamilyFiraCode = "Fira Code";

  public const string SvgWeightNormal = "normal";
  public const string SvgWeightBold = "bold";

  public const string SvgStyleNormal = "normal";
  public const string SvgStyleItalic = "italic";

  public static string ToSvgStyle(this FontFamilyType self)
  {
    switch (self)
    {
      case FontFamilyType.SansSerif: return SvgFamilyOpenSans;
      case FontFamilyType.Serif: return SvgFamilyLora;
      case FontFamilyType.Monospace: return SvgFamilyFiraCode;
      default: throw new NotSupportedException();
    }
  }

  public static string ToSvgStyle(this FontWeightType self)
  {
    switch (self)
    {
      case FontWeightType.Normal: return SvgWeightNormal;
      case FontWeightType.Bold: return SvgWeightBold;
      default: throw new NotSupportedException();
    }
  }

  public static string ToSvgStyle(this FontStyleType self)
  {
    switch (self)
    {
      case FontStyleType.Normal: return SvgStyleNormal;
      case FontStyleType.Italic: return SvgStyleItalic;
      default: throw new NotSupportedException();
    }
  }
}
