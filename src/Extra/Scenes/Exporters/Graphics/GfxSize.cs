namespace Trane.Submittals.Pipeline
{
  // Based on definitions in windef.h and wingdi.h

  public struct GfxSizeL
  {
    public long W { get; set; }
    public long H { get; set; }
    public GfxSizeUnit? Uom { get; set; }

    public GfxSizeL(long w, long h, GfxSizeUnit? uom = null)
    {
      W = w;
      H = h;
      Uom = uom;
    }

    public GfxSizeL Clone() => new GfxSizeL(W, H, Uom);
    public GfxSizeL WithUom(GfxSizeUnit? uom = null) => new GfxSizeL(W, H, uom);

    #region [Well Known]

    public static readonly GfxSizeL Zero = new GfxSizeL(0, 0, null);

    public static readonly GfxSizeL Sq512 = new GfxSizeL(512, 512, null);

    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeL Vga = new GfxSizeL(640, 480, GfxSizeUnit.Pixels);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeL IsoA1 = new GfxSizeL(594, 841, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeL IsoA2 = new GfxSizeL(420, 594, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeL IsoA3 = new GfxSizeL(297, 420, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeL IsoA4 = new GfxSizeL(210, 297, GfxSizeUnit.Millis);

    #endregion
  }

  public struct GfxSizeF
  {
    public float W { get; set; }
    public float H { get; set; }
    public GfxSizeUnit? Uom { get; set; }

    public GfxSizeF(float w, float h, GfxSizeUnit? uom = null)
    {
      W = w;
      H = h;
      Uom = uom;
    }

    public GfxSizeF Clone() => new GfxSizeF(W, H, Uom);
    public GfxSizeF WithUom(GfxSizeUnit? uom = null) => new GfxSizeF(W, H, uom);

    #region [Well Known]

    public static readonly GfxSizeF Zero = new GfxSizeF(0, 0, null);

    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeF Vga = new GfxSizeF(640, 480, GfxSizeUnit.Pixels);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeF IsoA1 = new GfxSizeF(594, 841, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeF IsoA2 = new GfxSizeF(420, 594, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeF IsoA3 = new GfxSizeF(297, 420, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeF IsoA4 = new GfxSizeF(210, 297, GfxSizeUnit.Millis);

    #endregion
  }

  public struct GfxSizeD
  {
    public double W { get; set; }
    public double H { get; set; }
    public GfxSizeUnit? Uom { get; set; }

    public GfxSizeD(double w, double h, GfxSizeUnit? uom = null)
    {
      W = w;
      H = h;
      Uom = uom;
    }

    public GfxSizeD Clone() => new GfxSizeD(W, H, Uom);
    public GfxSizeD WithUom(GfxSizeUnit? uom = null) => new GfxSizeD(W, H, uom);

    #region [Well Known]

    public static readonly GfxSizeD Zero = new GfxSizeD(0, 0, null);

    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeD Vga = new GfxSizeD(640, 480, GfxSizeUnit.Pixels);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeD IsoA1 = new GfxSizeD(594, 841, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeD IsoA2 = new GfxSizeD(420, 594, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeD IsoA3 = new GfxSizeD(297, 420, GfxSizeUnit.Millis);
    /// <remarks>
    /// https://en.wikipedia.org/wiki/ISO_216
    /// </remarks>
    public static readonly GfxSizeD IsoA4 = new GfxSizeD(210, 297, GfxSizeUnit.Millis);

    #endregion
  }

  public enum GfxSizeUnit
  {
    Millis,
    Inches,
    Pixels
  }

  public static class GfxSizeUnitConversion
  {
    public static double InchMillisFactor = 25.4;
    public static double MillisToInches(double millis) => millis / InchMillisFactor;
    public static double InchesToMillis(double inches) => inches * InchMillisFactor;
  }
}
