namespace Xvg;

public class VgLineToStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.LineTo;

  public Vector2 Point { get; set; }

  public VgLineToStep(Vector2 point, bool relative = false)
  {
    Point = point;
    Relative = relative;
  }

  public override IVgPathStep Translate(Vector2 translation)
  {
    if (!Relative)
      Point += translation;
    return this;
  }

  public override IVgPathStep Scale(Vector2 scale)
  {
    Point *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point;
}
