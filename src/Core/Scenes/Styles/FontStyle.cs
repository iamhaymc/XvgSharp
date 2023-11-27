namespace Xvg;

public enum FontFamilyType
{
  SansSerif,
  Serif,
  Monospace,
  Display,
  Bitmap,
}

public enum FontWeightType
{
  Normal, Bold
}

public enum FontStyleType
{
  Normal, Italic
}

public static partial class FontStyle
{
  public const FontFamilyType DefaultFamily = FontFamilyType.SansSerif;
  public const FontWeightType DefaultWeight = FontWeightType.Normal;
  public const FontStyleType DefaultStyle = FontStyleType.Normal;
  public static readonly float DefaultSize = 16;

  public const string FamilyM3x6 = "m5x7";
  public const string FamilyRobotoMono = "Roboto Mono";
  public const string FamilyOpenSans = "Open Sans";
  public const string FamilyMerriweather = "Merriweather";
  public const string FamilyComfortaa = "Comfortaa";

  public const string WeightNormal = "normal";
  public const string WeightBold = "bold";

  public const string StyleNormal = "normal";
  public const string StyleItalic = "italic";

  public static string ToStyle(this FontFamilyType self)
  {
    switch (self)
    {
      case FontFamilyType.SansSerif: return FamilyOpenSans;
      case FontFamilyType.Serif: return FamilyMerriweather;
      case FontFamilyType.Monospace: return FamilyRobotoMono;
      case FontFamilyType.Display: return FamilyComfortaa;
      case FontFamilyType.Bitmap: return FamilyM3x6;
      default: throw new NotSupportedException();
    }
  }

  public static string ToStyle(this FontWeightType self)
  {
    switch (self)
    {
      case FontWeightType.Normal: return WeightNormal;
      case FontWeightType.Bold: return WeightBold;
      default: throw new NotSupportedException();
    }
  }

  public static string ToStyle(this FontStyleType self)
  {
    switch (self)
    {
      case FontStyleType.Normal: return StyleNormal;
      case FontStyleType.Italic: return StyleItalic;
      default: throw new NotSupportedException();
    }
  }
}