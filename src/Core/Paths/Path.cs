namespace Xvg;

public class VgPath
{
  public List<IVgPathStep> Steps { get; set; }

  public VgPath()
       : this(new List<IVgPathStep>())
  { }

  public VgPath(List<IVgPathStep> steps)
  {
    Steps = steps;
  }
}

public static class VgPathExtensions
{
  public static VgPath MoveTo(this VgPath self, Vector2 point, bool relative = false)
  {
    self.Steps?.Add(new VgMoveToStep(point, relative));
    return self;
  }

  public static VgPath MoveTo(this VgPath self, float x, float y, bool relative = false)
  {
    return self.MoveTo(new Vector2(x, y), relative);
  }

  public static VgPath LineTo(this VgPath self, Vector2 point, bool relative = false)
  {
    self.Steps?.Add(new VgLineToStep(point, relative));
    return self;
  }

  public static VgPath LineTo(this VgPath self, float x, float y, bool relative = false)
  {
    return self.LineTo(new Vector2(x, y), relative);
  }

  public static VgPath AddLine(this VgPath self, Vector2 p0, Vector2 p1)
  {
    return self.MoveTo(p0).LineTo(p1);
  }

  public static VgPath AddLine(this VgPath self, float x0, float y0, float x1, float y1)
  {
    return self.AddLine(new Vector2(x0, y0), new Vector2(x1, y1));
  }

  public static VgPath Bezier2To(this VgPath self, Vector2 p0, Vector2 p1, bool relative = false)
  {
    self.Steps?.Add(new VgBezier2ToStep(p0, p1, relative));
    return self;
  }

  public static VgPath Bezier2To(this VgPath self, float x0, float y0, float x1, float y1, bool relative = false)
  {
    return self.Bezier2To(new Vector2(x0, y0), new Vector2(x1, y1), relative);
  }

  public static VgPath AddBezier2(this VgPath self, Vector2 p0, Vector2 p1, Vector2 p2)
  {
    return self.MoveTo(p0).Bezier2To(p1, p2);
  }

  public static VgPath AddBezier2(this VgPath self, float x0, float y0, float x1, float y1, float x2, float y2)
  {
    return self.AddBezier2(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2));
  }

  public static VgPath Bezier3To(this VgPath self, Vector2 p0, Vector2 p1, Vector2 p3, bool relative = false)
  {
    self.Steps?.Add(new VgBezier3ToStep(p0, p1, p3, relative));
    return self;
  }

  public static VgPath Bezier3To(this VgPath self, float x0, float y0, float x1, float y1, float x2, float y2, bool relative = false)
  {
    return self.Bezier3To(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2), relative);
  }

  public static VgPath AddBezier3(this VgPath self, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
  {
    return self.MoveTo(p0).Bezier3To(p1, p2, p3);
  }

  public static VgPath AddBezier3(this VgPath self, float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3)
  {
    return self.AddBezier3(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
  }

  public static VgPath ArcTo(this VgPath self, Vector2 point, Vector2 radius, float rotation, bool large, bool sweep, bool relative = false)
  {
    self.Steps?.Add(new VgArcToStep(point, radius, rotation, large, sweep, relative));
    return self;
  }

  public static VgPath ArcTo(this VgPath self, float x, float y, float rx, float ry, float rotation, bool large, bool sweep, bool relative = false)
  {
    return self.ArcTo(new Vector2(x, y), new Vector2(rx, ry), rotation, large, sweep, relative);
  }

  public static VgPath AddArc(this VgPath self, Vector2 p0, Vector2 p1, Vector2 radius, float rotation, bool large, bool sweep)
  {
    return self.MoveTo(p0).ArcTo(p1, radius, rotation, large, sweep);
  }

  public static VgPath AddArc(this VgPath self, float x0, float y0, float x1, float y1, float rx, float ry, float rotation, bool large, bool sweep)
  {
    return self.AddArc(new Vector2(x0, y0), new Vector2(x1, y1), new Vector2(rx, ry), rotation, large, sweep);
  }

  public static VgPath Close(this VgPath self)
  {
    self.Steps?.Add(new VgCloseStep());
    return self;
  }

  public static VgPath Translate(this VgPath self, Vector2 translation)
  {
    if (translation != Vector2.Zero)
      foreach (IVgPathStep step in self.Steps)
        step.Translate(translation);
    return self;
  }

  public static VgPath Translate(this VgPath self, float x, float y)
  {
    return self.Translate(new Vector2(x, y));
  }

  public static VgPath Scale(this VgPath self, Vector2 scale)
  {
    if (scale != Vector2.One)
      foreach (IVgPathStep step in self.Steps)
        step.Scale(scale);
    return self;
  }

  public static VgPath Scale(this VgPath self, float x, float y)
  {
    return self.Scale(new Vector2(x, y));
  }

  public static VgPath Scale(this VgPath self, float s)
  {
    return self.Scale(new Vector2(s, s));
  }

  public static bool IsEmpty(this VgPath self)
  {
    return self.Steps?.Count == 0;
  }

  public static bool IsPoint(this VgPath self)
  {
    return self.Steps.Count == 1 && self.Steps[0].Type == VgPathStepType.MoveTo;
  }

  public static bool IsSeries(this VgPath self)
  {
    return self.Steps?.Count > 1;
  }

  public static IVgPathStep ToHead(this VgPath self)
  {
    return self.Steps.FirstOrDefault();
  }

  public static IVgPathStep ToTail(this VgPath self)
  {
    return self.Steps.LastOrDefault();
  }

  public static Box ToBox(this VgPath self)
  {
    Vector2 min = Vector2.Zero, max = Vector2.Zero;
    Vector2 position0 = Vector2.Zero, positionN = Vector2.Zero;
    foreach (IVgPathStep step in self.Steps)
    {
      if (!step.IsClose())
      {
        Vector2 point = step.ToPoint() ?? Vector2.Zero;
        positionN = step.IsRelative ? positionN + point : point;
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
