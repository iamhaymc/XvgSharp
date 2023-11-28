namespace Xvg;

public static class PathEllipseExtensions
{
  public static Path AddEllipse(this Path self, Vector2 point, Vector2 radius)
  {
    return self
        .MoveTo(point.X, point.Y)
        .MoveTo(-radius.X, 0, relative: true)
        .ArcTo(new Vector2(radius.X * 2, 0), radius, 0, true, false, relative: true)
        .ArcTo(new Vector2(radius.X * -2, 0), radius, 0, true, false, relative: true);
  }

  public static Path AddEllipse(this Path self, float x, float y, float rx, float ry)
  {
    return self.AddEllipse(new Vector2(x, y), new Vector2(rx, ry));
  }

  public static Path AddCircle(this Path self, Vector2 point, float r)
  {
    return self.AddEllipse(point, new Vector2(r, r));
  }

  public static Path AddCircle(this Path self, float x, float y, float r)
  {
    return self.AddEllipse(new Vector2(x, y), new Vector2(r, r));
  }
}
