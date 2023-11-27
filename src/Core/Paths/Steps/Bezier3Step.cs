namespace Xvg;


public class VgBezier3ToStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.Bezier3To;

  public Vector2 Point0 { get; set; }
  public Vector2 Point1 { get; set; }
  public Vector2 Point2 { get; set; }

  public VgBezier3ToStep(Vector2 point0, Vector2 point1, Vector2 point2, bool relative = false)
  {
    Point0 = point0;
    Point1 = point1;
    Point2 = point2;
    IsRelative = relative;
  }

  public override IVgPathStep Translate(Vector2 translation)
  {
    if (!IsRelative)
    {
      Point0 += translation;
      Point1 += translation;
      Point2 += translation;
    }
    return this;
  }

  public override IVgPathStep Scale(Vector2 scale)
  {
    Point0 *= scale;
    Point1 *= scale;
    Point2 *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point2;
}
