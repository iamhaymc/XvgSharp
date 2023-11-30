namespace Trane.Submittals.Pipeline
{
  // Based on definitions in windef.h and wingdi.h

  /// <remarks>
  /// Uses the "wide character" structure variant
  /// </remarks>
  public class GfxFont
  {
    public const int FaceSize = 32;
    public const int FullFaceSize = 64;
    public long Height { get; set; }
    public long Width { get; set; }
    public long Escapement { get; set; }
    public long Orientation { get; set; }
    public long Weight { get; set; }
    public byte Italic { get; set; }
    public byte Underline { get; set; }
    public byte StrikeOut { get; set; }
    public byte CharSet { get; set; }
    public byte OutPrecision { get; set; }
    public byte ClipPrecision { get; set; }
    public byte Quality { get; set; }
    public byte PitchAndFamily { get; set; }
    public char[] FaceName { get; set; } = new char[FaceSize];
  }

  /// <remarks>
  /// Uses the "wide character" structure variant
  /// </remarks>
  public class GfxTextMetrics
  {
    public long Height { get; set; }
    public long Ascent { get; set; }
    public long Descent { get; set; }
    public long InternalLeading { get; set; }
    public long ExternalLeading { get; set; }
    public long AveCharWidth { get; set; }
    public long MaxCharWidth { get; set; }
    public long Weight { get; set; }
    public long Overhang { get; set; }
    public long DigitizedAspectX { get; set; }
    public long DigitizedAspectY { get; set; }
    public char FirstChar { get; set; }
    public char LastChar { get; set; }
    public char DefaultChar { get; set; }
    public char BreakChar { get; set; }
    public byte Italic { get; set; }
    public byte Underlined { get; set; }
    public byte StruckOut { get; set; }
    public byte PitchAndFamily { get; set; }
    public byte CharSet { get; set; }
  }
}
