namespace Xvg;

public class VgArcToStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.ArcTo;

  public Vector2 Point { get; set; }
  public Vector2 Radius { get; set; }
  public float Rotation { get; set; }
  public bool Large { get; set; }
  public bool Sweep { get; set; }

  public VgArcToStep(Vector2 point, Vector2 radius, float rotation, bool large, bool sweep, bool relative = false)
  {
    Point = point;
    Radius = radius;
    Rotation = rotation;
    Large = large;
    Sweep = sweep;
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
    Radius *= scale;
    return this;
  }

  public override Vector2? ToPoint() => Point;
}
