namespace Xvg;

public class Bezier2ToStep : StepBase
{
  public override PathStepType Type => PathStepType.Bezier2To;

  public Vector2 Point0 { get; set; }
  public Vector2 Point1 { get; set; }

  public Bezier2ToStep(Vector2 point0, Vector2 point1, bool relative = false)
  {
    Point0 = point0;
    Point1 = point1;
    Relative = relative;
  }

  public override IPathStep Translate(Vector2 translation)
  {
    if (!Relative)
    {
      Point0 += translation;
      Point1 += translation;
    }
    return this;
  }

  public override IPathStep Scale(Vector2 scale)
  {
    Point0 *= scale;
    Point1 *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point1;
}
