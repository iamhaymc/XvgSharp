using Vanara.PInvoke;
using Gdi = Vanara.PInvoke.Gdi32;

namespace Xvg;

public class GdiPen : IGfxPen
{
  public IGfxColor Color { get; }
  public float Width { get; }
  public Gdi.SafeHPEN Handle { get; set; } = Gdi.SafeHPEN.Null;

  public GdiPen(IGfxColor color, int width, Gdi.SafeHPEN handle)
  {
    Color = color;
    Width = width;
    Handle = handle;
  }

  public void Dispose()
  {
    if (Handle != Gdi.SafeHPEN.Null)
    {
      Gdi.DeleteObject(Handle);
      Handle = Gdi.SafeHPEN.Null;
    }
  }
}
