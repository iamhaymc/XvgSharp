using Vanara.PInvoke;
using Gdi = Vanara.PInvoke.Gdi32;

namespace Xvg;

public class GdiBrush : IGfxBrush
{
  public IGfxColor Color { get; }
  public Gdi.SafeHBRUSH Handle { get; set; } = Gdi.SafeHBRUSH.Null;

  public GdiBrush(IGfxColor color, Gdi.SafeHBRUSH handle)
  {
    Color = color;
    Handle = handle;
  }

  public void Dispose()
  {
    if (Handle != Gdi.SafeHBRUSH.Null)
    {
      Gdi.DeleteObject(Handle);
      Handle = Gdi.SafeHBRUSH.Null;
    }
  }
}
