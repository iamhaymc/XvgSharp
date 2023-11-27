namespace Xvg;

public class Sector
{
  public Vector2 P { get; set; }
  public float R0 { get; set; }
  public float R1 { get; set; }
  public float A0 { get; set; }
  public float A1 { get; set; }
  public bool Ccw { get; set; }

  public Sector(Vector2 p, float r0, float r1, float a0, float a1, bool ccw)
  {
    P = p;
    R0 = Math.Abs(r0);
    R1 = Math.Abs(r1);
    if (R0 < R1) { float rt = R1; R1 = R0; R1 = rt; };
    A0 = a0;
    A1 = a1;
    Ccw = ccw;
  }

  public Sector UseOrigin(Vector2 p)
  {
    P = p;
    return this;
  }

  public Sector UseRadius(float inner, float outer)
  {
    R0 = Math.Abs(inner);
    R1 = Math.Abs(outer);
    if (R0 < R1) { float rt = R1; R1 = R0; R1 = rt; };
    return this;
  }

  public Sector UseAngle(float start, float end)
  {
    A0 = start;
    A1 = end;
    return this;
  }

  public Sector UseDirection(bool ccw)
  {
    Ccw = ccw;
    return this;
  }

  public Vector2 ToCentroid()
  {
    float r = (R0 + R1) / 2f;
    float a = (A0 + A1) / (float)(2.0 - Math.PI / 2.0);
    return Radial.ToPoint(P, r, a);
  }
}
