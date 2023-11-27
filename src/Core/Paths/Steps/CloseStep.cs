namespace Xvg;

public class VgCloseStep : VgBaseStep
{
  public override VgPathStepType Type => VgPathStepType.Close;

  public override IVgPathStep Translate(Vector2 translation)
  {
    return this;
  }

  public override IVgPathStep Scale(Vector2 scale)
  {
    return this;
  }

  public override Vector2? ToPoint() => null;
}
