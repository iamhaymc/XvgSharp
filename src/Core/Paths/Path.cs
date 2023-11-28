namespace Xvg;

public class Path
{
  public List<IPathStep> Steps { get; set; }

  public Path(List<IPathStep> steps = null)
  {
    Steps = steps ?? new List<IPathStep>();
  }
}

public static class PathExtensions
{
  public static Path MoveTo(this Path self, Vector2 point, bool relative = false)
  {
    self.Steps?.Add(new MoveToStep(point, relative));
    return self;
  }

  public static Path MoveTo(this Path self, float x, float y, bool relative = false)
  {
    return self.MoveTo(new Vector2(x, y), relative);
  }

  public static Path LineTo(this Path self, Vector2 point, bool relative = false)
  {
    self.Steps?.Add(new LineToStep(point, relative));
    return self;
  }

  public static Path LineTo(this Path self, float x, float y, bool relative = false)
  {
    return self.LineTo(new Vector2(x, y), relative);
  }

  public static Path AddLine(this Path self, Vector2 p0, Vector2 p1)
  {
    return self.MoveTo(p0).LineTo(p1);
  }

  public static Path AddLine(this Path self, float x0, float y0, float x1, float y1)
  {
    return self.AddLine(new Vector2(x0, y0), new Vector2(x1, y1));
  }

  public static Path Bezier2To(this Path self, Vector2 p0, Vector2 p1, bool relative = false)
  {
    self.Steps?.Add(new Bezier2ToStep(p0, p1, relative));
    return self;
  }

  public static Path Bezier2To(this Path self, float x0, float y0, float x1, float y1, bool relative = false)
  {
    return self.Bezier2To(new Vector2(x0, y0), new Vector2(x1, y1), relative);
  }

  public static Path AddBezier2(this Path self, Vector2 p0, Vector2 p1, Vector2 p2)
  {
    return self.MoveTo(p0).Bezier2To(p1, p2);
  }

  public static Path AddBezier2(this Path self, float x0, float y0, float x1, float y1, float x2, float y2)
  {
    return self.AddBezier2(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2));
  }

  public static Path Bezier3To(this Path self, Vector2 p0, Vector2 p1, Vector2 p3, bool relative = false)
  {
    self.Steps?.Add(new Bezier3ToStep(p0, p1, p3, relative));
    return self;
  }

  public static Path Bezier3To(this Path self, float x0, float y0, float x1, float y1, float x2, float y2, bool relative = false)
  {
    return self.Bezier3To(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2), relative);
  }

  public static Path AddBezier3(this Path self, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
  {
    return self.MoveTo(p0).Bezier3To(p1, p2, p3);
  }

  public static Path AddBezier3(this Path self, float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3)
  {
    return self.AddBezier3(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
  }

  public static Path ArcTo(this Path self, Vector2 point, Vector2 radius, float rotation, bool large, bool sweep, bool relative = false)
  {
    self.Steps?.Add(new ArcToStep(point, radius, rotation, large, sweep, relative));
    return self;
  }

  public static Path ArcTo(this Path self, float x, float y, float rx, float ry, float rotation, bool large, bool sweep, bool relative = false)
  {
    return self.ArcTo(new Vector2(x, y), new Vector2(rx, ry), rotation, large, sweep, relative);
  }

  public static Path AddArc(this Path self, Vector2 p0, Vector2 p1, Vector2 radius, float rotation, bool large, bool sweep)
  {
    return self.MoveTo(p0).ArcTo(p1, radius, rotation, large, sweep);
  }

  public static Path AddArc(this Path self, float x0, float y0, float x1, float y1, float rx, float ry, float rotation, bool large, bool sweep)
  {
    return self.AddArc(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(rx, ry), rotation, large, sweep);
  }

  public static Path Close(this Path self)
  {
    self.Steps?.Add(new CloseStep());
    return self;
  }

  public static Path Translate(this Path self, Vector2 translation)
  {
    if (translation != Vector2.Zero)
      foreach (IPathStep step in self.Steps)
        step.Translate(translation);
    return self;
  }

  public static Path Translate(this Path self, float x, float y)
  {
    return self.Translate(new Vector2(x, y));
  }

  public static Path Scale(this Path self, Vector2 scale)
  {
    if (scale != Vector2.One)
      foreach (IPathStep step in self.Steps)
        step.Scale(scale);
    return self;
  }

  public static Path Scale(this Path self, float x, float y)
  {
    return self.Scale(new Vector2(x, y));
  }

  public static Path Scale(this Path self, float s)
  {
    return self.Scale(new Vector2(s, s));
  }

  public static bool IsEmpty(this Path self)
  {
    return self.Steps?.Count == 0;
  }

  public static bool IsPoint(this Path self)
  {
    return self.Steps.Count == 1 && self.Steps[0].Type == PathStepType.MoveTo;
  }

  public static bool IsSeries(this Path self)
  {
    return self.Steps?.Count > 1;
  }

  public static IPathStep ToHead(this Path self)
  {
    return self.Steps.FirstOrDefault();
  }

  public static IPathStep ToTail(this Path self)
  {
    return self.Steps.LastOrDefault();
  }

  public static Box ToBox(this Path self)
  {
    Vector2 min = Vector2.Zero, max = Vector2.Zero;
    Vector2 position0 = Vector2.Zero, positionN = Vector2.Zero;
    foreach (IPathStep step in self.Steps)
    {
      if (!step.IsClose())
      {
        Vector2 point = step.ToPoint() ?? Vector2.Zero;
        positionN = step.Relative ? positionN + point : point;
        if (step.IsMoveTo())
          position0 = positionN;
      }
      else
        positionN = position0;
      min.X = Math.Min(min.X, positionN.X);
      min.Y = Math.Min(min.Y, positionN.Y);
      max.X = Math.Max(max.X, positionN.X);
      max.Y = Math.Max(max.Y, positionN.Y);
    }
    return new Box(min, max);
  }
}
