using System;

namespace Xvg;

// Based on definitions in windef.h and wingdi.h

public interface IGfxBrush : IDisposable
{
  IGfxColor Color { get; }
}
