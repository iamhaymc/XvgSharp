
namespace Xvg;

public interface IGfxCanvas : IDisposable
{
  IGfxBrush CreateBrush(byte r, byte g, byte b);
  IGfxBrush CreateBrush(IGfxColor color);
  IGfxCanvas UseBrush(IGfxBrush brush);

  IGfxPen CreatePen(byte r, byte g, byte b, int w);
  IGfxPen CreatePen(IGfxColor color, int width);
  IGfxCanvas UsePen(IGfxPen pen);

  IGfxCanvas MoveTo(int x, int y);
  IGfxCanvas StartPath();
  IGfxCanvas LineTo(int x, int y);
  IGfxCanvas BezierTo(int c0x, int c0y, int c1x, int c1y, int x, int y);
  IGfxCanvas EndPath();

  IGfxCanvas Close();
}

public abstract class GfxCanvas : IGfxCanvas
{
  public GfxSizeD DisplaySize { get; protected set; }
  public GfxSizeD ResolutionSize { get; protected set; }

  protected virtual GfxSizeD EnsureDisplaySizeMillis()
  {
    return DisplaySize = GfxSizeD.IsoA1;
  }

  protected virtual GfxSizeD EnsureDisplayResolutionPixels()
  {
    GfxSizeD size = EnsureDisplaySizeMillis();
    double ppmm = GfxSizeUnitConversion.InchesToMillis(96);
    return ResolutionSize = new GfxSizeD(size.W * ppmm, size.H * ppmm, GfxSizeUnit.Pixels);
  }

  protected GfxRectL CreateFrame(double width, double height, GfxSizeUnit sizeUnit)
  {
    /// Note: Metafile dimensions are specified
    /// using 1/100 of a millimeter as the unit of measure.

    var displaySizeMillis = EnsureDisplaySizeMillis();
    var displayResolutionPixels = EnsureDisplayResolutionPixels();

    return sizeUnit switch
    {
      GfxSizeUnit.Millis => new GfxRectL(
        (int)(width * 100),
        (int)(height * 100)
        ),
      GfxSizeUnit.Inches => new GfxRectL(
        (int)(GfxSizeUnitConversion.InchesToMillis(width) * 100),
        (int)(GfxSizeUnitConversion.InchesToMillis(height) * 100)
        ),
      GfxSizeUnit.Pixels => new GfxRectL(
        (int)(width * displaySizeMillis.W / displayResolutionPixels.W * 100), // FIXME: are we always off by 1 pixel?
        (int)(height * displaySizeMillis.H / displayResolutionPixels.H * 100) // FIXME: are we always off by 1 pixel?
        ),
      _ => throw new NotSupportedException("The size unit '{uom}' is not supported"),
    };
  }

  public abstract IGfxBrush CreateBrush(byte r, byte g, byte b);
  public IGfxBrush CreateBrush(IGfxColor color)
  {
    var rgb = color.ToRgba();
    return CreateBrush(rgb.R, rgb.G, rgb.B);
  }
  public abstract IGfxCanvas UseBrush(IGfxBrush brush);

  public abstract IGfxPen CreatePen(byte r, byte g, byte b, int w);
  public IGfxPen CreatePen(IGfxColor color, int width)
  {
    var rgb = color.ToRgba();
    return CreatePen(rgb.R, rgb.G, rgb.B, width);
  }
  public abstract IGfxCanvas UsePen(IGfxPen pen);

  public abstract IGfxCanvas MoveTo(int x, int y);
  public abstract IGfxCanvas StartPath();
  public abstract IGfxCanvas LineTo(int x, int y);
  public abstract IGfxCanvas BezierTo(int c0x, int c0y, int c1x, int c1y, int x, int y);
  public abstract IGfxCanvas EndPath();

  public abstract IGfxCanvas Close();

  public abstract void Dispose();
}
