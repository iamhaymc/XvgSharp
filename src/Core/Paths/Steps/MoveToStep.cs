namespace Xvg;

public class VgMoveToStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.MoveTo;

  public Vector2 Point { get; set; }

  public VgMoveToStep(Vector2 point, bool relative = false)
  {
    Point = point;
    IsRelative = relative;
  }

  public override IVgPathStep Translate(Vector2 translation)
  {
    if (!IsRelative)
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
