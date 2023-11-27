namespace Xvg;

public class Polygon
{
  public Vector2[] Points { get; set; }

  public Polygon(params Vector2[] points)
  {
    Points = points;
  }

  public float Area => MeasureArea(Points);
  public float Perimeter => MeasurePerimeter(Points);
  public Vector2 Centroid => ToCentroid(Points);

  public bool Contains(Vector2 point) => Contains(point, Points);

  public static Polygon FromBox(Box box)
  {
    return new Polygon(
      new Vector2(box.Min.X, box.Min.Y),
      new Vector2(box.Max.X, box.Min.Y),
      new Vector2(box.Max.X, box.Max.Y),
      new Vector2(box.Min.X, box.Max.Y));
  }
  public static Polygon? FromConvexHull(params Vector2[] polygon)
  {
    if (polygon.Length < 2) // Not a polygon
      return null;
    else if (polygon.Length == 3) // Is a triangle
      return new Polygon(polygon);

    List<(Vector2, int)> sortedPoints = new List<(Vector2, int)>(polygon.Length);
    List<Vector2> flippedPoints = new List<Vector2>(polygon.Length);

    for (int i = 0; i < polygon.Length; ++i)
      sortedPoints.Add((polygon[i], i));
    sortedPoints.Sort((a, b) =>
    {
      float d = a.Item1.X - b.Item1.X;
      return (d != 0) ? (int)d : (int)(a.Item1.Y - b.Item1.Y);
    });
    for (int i = 0; i < polygon.Length; ++i)
      flippedPoints.Add(new Vector2(sortedPoints[i].Item1.X, -sortedPoints[i].Item1.Y));

    Vector2[] upperPoints = sortedPoints.Select(s => s.Item1).ToArray();
    List<int> upperIndexes = new List<int> { 0, 1 };
    int upperSize = 2;
    for (int i = 2; i < upperPoints.Length; ++i)
    {
      while (upperSize > 1 && CrossProduct(
          a: upperPoints[upperIndexes[upperSize - 2]],
          b: upperPoints[upperIndexes[upperSize - 1]],
          c: upperPoints[i]) <= 0)
        --upperSize;
      if (upperIndexes.Count >= upperSize + 1)
        upperIndexes[upperSize] = i;
      else
        upperIndexes.Add(i);
      upperSize++;
    }
    upperIndexes = upperIndexes.Take(upperSize).ToList();

    Vector2[] lowerPoints = flippedPoints.ToArray();
    List<int> lowerIndexes = new List<int> { 0, 1 };
    int lowerSize = 2;
    for (int i = 2; i < lowerPoints.Length; ++i)
    {
      while (lowerSize > 1 && CrossProduct(
          a: lowerPoints[lowerIndexes[lowerSize - 2]],
          b: lowerPoints[lowerIndexes[lowerSize - 1]],
          c: lowerPoints[i]) <= 0)
        --lowerSize;
      if (lowerSize < lowerIndexes.Count)
        lowerIndexes[lowerSize] = i;
      else
        lowerIndexes.Add(i);
      lowerSize++;
    }
    lowerIndexes = lowerIndexes.Take(lowerSize).ToList();

    bool skipLeft = lowerIndexes[0] == upperIndexes[0],
       skipRight = lowerIndexes[lowerIndexes.Count - 1] == upperIndexes[upperIndexes.Count - 1];

    List<Vector2> hullPoints = new List<Vector2>();
    for (int i = upperIndexes.Count - 1; i >= 0; --i)
      hullPoints.Add(polygon[sortedPoints[upperIndexes[i]].Item2]);
    for (int i = skipLeft ? 1 : 0; i < lowerIndexes.Count - (skipRight ? 1 : 0); ++i)
      hullPoints.Add(polygon[sortedPoints[lowerIndexes[i]].Item2]);

    return new Polygon(hullPoints.ToArray());
  }

  public static bool Contains(Vector2 point, params Vector2[] polygon)
  {
    Vector2 p = polygon[polygon.Length - 1];
    float x = point.X, y = point.Y,
      x0 = p.X, y0 = p.Y, x1, y1;
    bool inside = false;
    for (var i = 0; i < polygon.Length; ++i)
    {
      p = polygon[i];
      x1 = p.X;
      y1 = p.Y;
      if (((y1 > y) != (y0 > y))
        && (x < (x0 - x1) * (y - y1) / (y0 - y1) + x1))
        inside = !inside;
      x0 = x1;
      y0 = y1;
    }
    return inside;
  }

  public static float MeasureArea(params Vector2[] polygon)
  {
    Vector2 a, b = polygon[polygon.Length - 1];
    float area = 0f;
    for (int i = -1; ++i < polygon.Length;)
    {
      a = b;
      b = polygon[i];
      area += a.Y * b.X - a.X * b.Y;
    }
    return area / 2f;
  }
  public static float MeasurePerimeter(params Vector2[] polygon)
  {
    Vector2 b = polygon[polygon.Length - 1];
    float xa, ya, xb = b.X, yb = b.Y,
      perimeter = 0f;
    for (int i = -1; ++i < polygon.Length;)
    {
      xa = xb;
      ya = yb;
      b = polygon[i];
      xb = b.X;
      yb = b.Y;
      xa -= xb;
      ya -= yb;
      perimeter += Hypotenuse(xa, ya);
    }
    return perimeter;
  }

  public static Vector2 ToCentroid(params Vector2[] polygon)
  {
    Vector2 a, b = polygon[polygon.Length - 1];
    float c, k = 0, x = 0, y = 0;
    for (int i = -1; ++i < polygon.Length;)
    {
      a = b;
      b = polygon[i];
      k += c = a.X * b.Y - b.X * a.Y;
      x += (a.X + b.X) * c;
      y += (a.Y + b.Y) * c;
    }
    k *= 3f;
    return new Vector2(x / k, y / k);
  }

  public static float Hypotenuse(float x, float y)
    => (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

  public static float CrossProduct(Vector2 a, Vector2 b, Vector2 c)
    => (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
}
