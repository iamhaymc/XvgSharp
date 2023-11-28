namespace Xvg;

public class CloseStep : StepBase
{
  public override PathStepType Type => PathStepType.Close;

  public override IPathStep Translate(Vector2 translation)
  {
    return this;
  }

  public override IPathStep Scale(Vector2 scale)
  {
    return this;
  }

  public override Vector2? ToPoint() => null;
}
