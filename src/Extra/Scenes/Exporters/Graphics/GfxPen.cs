using System;

namespace Trane.Submittals.Pipeline
{
  // Based on definitions in windef.h and wingdi.h

  public interface IGfxPen : IDisposable
  {
    IGfxColor Color { get; }
    float Width { get; }
  }
}
