namespace Xvg;

// Based on definitions in windef.h and wingdi.h

public interface IGfxColor
{
  IGfxColor Clone();
  GfxRgbaColor ToRgba();
}

public struct GfxRgbaColor : IGfxColor
{
  public byte R { get; set; }
  public byte G { get; set; }
  public byte B { get; set; }
  public byte A { get; set; }

  public GfxRgbaColor(byte r, byte g, byte b, byte a)
  {
    R = r;
    G = g;
    B = b;
    A = a;
  }

  public IGfxColor Clone() => new GfxRgbaColor(R, G, B, A);
  public GfxRgbaColor ToRgba() => (GfxRgbaColor)Clone();

  #region [Well Known]

  public static readonly GfxRgbaColor Zero = new GfxRgbaColor(0, 0, 0, 0);
  public static readonly GfxRgbaColor Black = new GfxRgbaColor(0, 0, 0, 255);
  public static readonly GfxRgbaColor White = new GfxRgbaColor(255, 255, 255, 255);

  #endregion
}

public enum GfxBgColorType
{
  TRANSPARENT = 1,
  OPAQUE = 2
}
