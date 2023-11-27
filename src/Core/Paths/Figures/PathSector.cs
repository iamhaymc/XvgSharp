namespace Xvg;

public static class VgPathSectorExtensions
{
  public static VgPath AddSector(this VgPath self, Sector s)
  {
    return self.AddAnnularSector(s.P, s.R0, s.R1, s.A0, s.A1, s.Ccw);
  }

  public static VgPath AddCircularSector(this VgPath self, Vector2 p, float r, float a0, float a1, bool ccw)
  {
    float da = ccw ? a0 - a1 : a1 - a0;
    if (da < 0) da = da % Radial.TauF + Radial.TauF;
    r = Math.Abs(r);
    if (da > Radial.TauF - Real.EpsilonF)
      self.AddCircle(p, r);
    else
      self.MoveTo(p)
          .LineTo(Radial.ToPoint(p, r, a0))
          .ArcTo(Radial.ToPoint(p, r, a1), new Vector2(r, r), 0, da >= Radial.PIF, !ccw)
          .Close();
    return self;
  }

  public static VgPath AddCircularSector(this VgPath self, float x, float y, float r, float a0, float a1, bool ccw)
  {
    return self.AddCircularSector(new Vector2(x, y), r, a0, a1, ccw);
  }

  public static VgPath AddAnnularSector(this VgPath self, Vector2 p, float r0, float r1, float a0, float a1, bool ccw)
  {
    float da = ccw ? a0 - a1 : a1 - a0;
    if (da < 0) da = da % Radial.TauF + Radial.TauF;
    r0 = Math.Abs(r0);
    r1 = Math.Abs(r1);
    if (r1 < r0) { float rt = r1; r1 = r0; r0 = rt; };
    if (da > Radial.TauF - Real.EpsilonF)
    {
      self.AddCircle(p, r1);
      if (r0 > Real.EpsilonF)
        self.AddCircle(p, r0);
    }
    else
      self.MoveTo(Radial.ToPoint(p, r0, a0))
          .LineTo(Radial.ToPoint(p, r1, a0))
          .ArcTo(Radial.ToPoint(p, r1, a1), new Vector2(r1, r1), 0, da >= Radial.PIF, !ccw)
          .LineTo(Radial.ToPoint(p, r0, a1))
          .ArcTo(Radial.ToPoint(p, r0, a0), new Vector2(r0, r0), 0, da >= Radial.PIF, ccw)
          .Close();
    return self;
  }

  public static VgPath AddAnnularSector(this VgPath self, float x, float y, float r0, float r1, float a0, float a1, bool ccw)
  {
    return self.AddAnnularSector(new Vector2(x, y), r0, r1, a0, a1, ccw);
  }
}
