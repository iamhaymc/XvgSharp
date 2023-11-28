namespace Xvg;

public class ArcToStep : StepBase
{
  public override PathStepType Type => PathStepType.ArcTo;

  public Vector2 Point { get; set; }
  public Vector2 Radius { get; set; }
  public float Rotation { get; set; }
  public bool Large { get; set; }
  public bool Sweep { get; set; }

  public ArcToStep(Vector2 point, Vector2 radius, float rotation, bool large, bool sweep, bool relative = false)
  {
    Point = point;
    Radius = radius;
    Rotation = rotation;
    Large = large;
    Sweep = sweep;
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
    Radius *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point;
}
