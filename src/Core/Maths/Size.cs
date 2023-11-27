namespace Xvg;

public struct Size
{
  public float W { get; set; }
  public float H { get; set; }
  public SizeUnitType Uom { get; set; }
  public Size(float w, float h, SizeUnitType uom = SizeUnitType.None)
  {
    W = w;
    H = h;
    Uom = uom;
  }

  public Size Clone() => new Size(W, H, Uom);
  public Size WithUom(SizeUnitType uom) => new Size(W, H, uom);

  #region [Well Known]

  public static readonly Size Zero = new Size(0, 0);
  public static readonly Size Sq100 = new Size(100, 100);
  public static readonly Size Sq1000 = new Size(512, 512);

  #endregion
}

public struct SizeI
{
  public int W { get; set; }
  public int H { get; set; }
  public SizeUnitType Uom { get; set; }
  public SizeI(int w, int h, SizeUnitType uom = SizeUnitType.None)
  {
    W = w;
    H = h;
    Uom = uom;
  }

  public SizeI Clone() => new SizeI(W, H, Uom);
  public SizeI WithUom(SizeUnitType uom) => new SizeI(W, H, uom);

  #region [Well Known]

  public static readonly SizeI Zero = new SizeI(0, 0);
  public static readonly SizeI Sq100 = new SizeI(100, 100);
  public static readonly SizeI Sq1000 = new SizeI(512, 512);

  /// <remarks>
  /// https://en.wikipedia.org/wiki/ISO_216
  /// </remarks>
  public static readonly SizeI Vga = new SizeI(640, 480, SizeUnitType.Pixels);
  /// <remarks>
  /// https://en.wikipedia.org/wiki/ISO_216
  /// </remarks>
  public static readonly SizeI IsoA1 = new SizeI(594, 841, SizeUnitType.Millis);
  /// <remarks>
  /// https://en.wikipedia.org/wiki/ISO_216
  /// </remarks>
  public static readonly SizeI IsoA2 = new SizeI(420, 594, SizeUnitType.Millis);
  /// <remarks>
  /// https://en.wikipedia.org/wiki/ISO_216
  /// </remarks>
  public static readonly SizeI IsoA3 = new SizeI(297, 420, SizeUnitType.Millis);
  /// <remarks>
  /// https://en.wikipedia.org/wiki/ISO_216
  /// </remarks>
  public static readonly SizeI IsoA4 = new SizeI(210, 297, SizeUnitType.Millis);

  #endregion
}

public enum SizeUnitType
{
  None,
  Millis,
  Inches,
  Pixels
}

public static class SizeConversion
{
  public static double InchMillisFactor = 25.4;
  public static float MillisToInches(float millis) => millis / (float)InchMillisFactor;
  public static float InchesToMillis(float inches) => inches * (float)InchMillisFactor;
}
