namespace Xvg;

public static class VgPathPolygonExtensions
{
  public static VgPath AddPolygon(this VgPath self, Polygon polygon)
  {
    return self.AddPolygon(polygon.Points);
  }

  public static VgPath AddPolygon(this VgPath self, params Vector2[] points)
  {
    if (points.Length > 2)
    {
      Vector2 p0 = points[0];
      Vector2 pl = points[points.Length - 1];
      self.MoveTo(p0);
      for (int i = 1; i < points.Length; ++i)
        self.LineTo(points[i]);
      if (p0 != pl)
        self.Close();
    }
    return self;
  }

  public static VgPath AddTriangle(this VgPath self, Vector2 p0, Vector2 p1, Vector2 p2)
  {
    return self.MoveTo(p0).LineTo(p1).LineTo(p2).Close();
  }

  public static VgPath AddRectangle(this VgPath self, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
  {
    return self.MoveTo(p0).LineTo(p1).LineTo(p2).LineTo(p3).Close();
  }

  public static VgPath AddRectangle(this VgPath self, Vector2 position, Vector2 size)
  {
    return self.AddRectangle(
        p0: position,
        p1: new Vector2(position.X + size.X, position.Y),
        p2: new Vector2(position.X + size.X, position.Y + size.Y),
        p3: new Vector2(position.X, position.Y + size.Y));
  }

  public static VgPath AddRectangle(this VgPath self, float x, float y, float w, float h)
  {
    return self.AddRectangle(new Vector2(x, y), new Vector2(w, h));
  }

  public static VgPath AddSquare(this VgPath self, Vector2 position, float size)
  {
    return self.AddRectangle(position, new Vector2(size, size));
  }

  public static VgPath AddSquare(this VgPath self, float x, float y, float s)
  {
    return self.AddRectangle(new Vector2(x, y), new Vector2(s, s));
  }
}
