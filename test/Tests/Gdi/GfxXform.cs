namespace Xvg
{
  // Based on definitions in windef.h and wingdi.h

  public struct GfxXformF
  {
    public float M11 { get; set; }
    public float M12 { get; set; }
    public float M21 { get; set; }
    public float M22 { get; set; }
    public float Dx { get; set; }
    public float Dy { get; set; }
  }
}
