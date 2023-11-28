namespace Xvg;

public class LineToStep : StepBase
{
  public override PathStepType Type => PathStepType.LineTo;

  public Vector2 Point { get; set; }

  public LineToStep(Vector2 point, bool relative = false)
  {
    Point = point;
    Relative = relative;
  }

  public override IPathStep Translate(Vector2 translation)
  {
    if (!Relative)
      Point += translation;
    return this;
  }

  public override IPathStep Scale(Vector2 scale)
  {
    Point *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point;
}
