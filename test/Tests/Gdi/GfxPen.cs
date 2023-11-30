using System;

namespace Xvg
{
  // Based on definitions in windef.h and wingdi.h

  public interface IGfxPen : IDisposable
  {
    IGfxColor Color { get; }
    float Width { get; }
  }
}
