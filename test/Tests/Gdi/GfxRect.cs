namespace Xvg
{
  // Based on definitions in windef.h and wingdi.h

  public struct GfxRectL
  {
    public long Left { get; set; }
    public long Top { get; set; }
    public long Right { get; set; }
    public long Bottom { get; set; }

    public GfxRectL(long w, long h) : this(0, 0, w, h) { }
    public GfxRectL(long l, long t, long r, long b)
    {
      Left = l;
      Top = t;
      Right = r;
      Bottom = b;
    }

    #region [Well Known]

    public static readonly GfxRectL Zero = new GfxRectL(0, 0, 0, 0);

    #endregion
  }

  public struct GfxRectF
  {
    public float Left { get; set; }
    public float Top { get; set; }
    public float Right { get; set; }
    public float Bottom { get; set; }

    public GfxRectF(float w, float h) : this(0, 0, w, h) { }
    public GfxRectF(float l, float t, float r, float b)
    {
      Left = l;
      Top = t;
      Right = r;
      Bottom = b;
    }

    #region [Well Known]

    public static readonly GfxRectF Zero = new GfxRectF(0, 0, 0, 0);

    #endregion
  }

  public struct GfxRectD
  {
    public double Left { get; set; }
    public double Top { get; set; }
    public double Right { get; set; }
    public double Bottom { get; set; }

    public GfxRectD(double w, double h) : this(0, 0, w, h) { }
    public GfxRectD(double l, double t, double r, double b)
    {
      Left = l;
      Top = t;
      Right = r;
      Bottom = b;
    }

    #region [Well Known]

    public static readonly GfxRectD Zero = new GfxRectD(0, 0, 0, 0);

    #endregion
  }
}
