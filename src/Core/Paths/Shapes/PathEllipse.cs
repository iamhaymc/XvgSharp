namespace Xvg;

public static class VgPathEllipseExtensions
{
  public static VgPath AddEllipse(this VgPath self, Vector2 point, Vector2 radius)
  {
    return self
        .MoveTo(point.X, point.Y)
        .MoveTo(-radius.X, 0, relative: true)
        .ArcTo(new Vector2(radius.X * 2, 0), radius, 0, true, false, relative: true)
        .ArcTo(new Vector2(radius.X * -2, 0), radius, 0, true, false, relative: true);
  }

  public static VgPath AddEllipse(this VgPath self, float x, float y, float rx, float ry)
  {
    return self.AddEllipse(new Vector2(x, y), new Vector2(rx, ry));
  }

  public static VgPath AddCircle(this VgPath self, Vector2 point, float r)
  {
    return self.AddEllipse(point, new Vector2(r, r));
  }

  public static VgPath AddCircle(this VgPath self, float x, float y, float r)
  {
    return self.AddEllipse(new Vector2(x, y), new Vector2(r, r));
  }
}
