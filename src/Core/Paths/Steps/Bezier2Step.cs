namespace Xvg;

public class VgBezier2ToStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.Bezier2To;

  public Vector2 Point0 { get; set; }
  public Vector2 Point1 { get; set; }

  public VgBezier2ToStep(Vector2 point0, Vector2 point1, bool relative = false)
  {
    Point0 = point0;
    Point1 = point1;
    IsRelative = relative;
  }

  public override IVgPathStep Translate(Vector2 translation)
  {
    if (!IsRelative)
    {
      Point0 += translation;
      Point1 += translation;
    }
    return this;
  }

  public override IVgPathStep Scale(Vector2 scale)
  {
    Point0 *= scale;
    Point1 *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point1;
}
