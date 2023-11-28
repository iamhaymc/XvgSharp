namespace Xvg;

public class PathSvgExporter : TextWriter
{
  public Path Path { get; set; }

  public string WriteSvgString(Path path)
  {
    Reset();
    Path = path;
    foreach (IPathStep step in Path.Steps)
      AppendStep(step);
    return Dump();
  }

  private void AppendStep(IPathStep step)
  {
    switch (step.Type)
    {
      case PathStepType.MoveTo:
        AppendMoveTo((MoveToStep)step);
        break;
      case PathStepType.LineTo:
        AppendLineTo((LineToStep)step);
        break;
      case PathStepType.Bezier2To:
        AppendBezier2To((Bezier2ToStep)step);
        break;
      case PathStepType.Bezier3To:
        AppendBezier3To((Bezier3ToStep)step);
        break;
      case PathStepType.ArcTo:
        AppendArcTo((ArcToStep)step);
        break;
      case PathStepType.Close:
        AppendClose((CloseStep)step);
        break;
      default: return;
    }
  }

  private void AppendMoveTo(MoveToStep step)
      => Add($"{(step.Relative ? 'm' : 'M')} {step.Point.X} {step.Point.Y}");

  private void AppendLineTo(LineToStep step)
      => Add($"{(step.Relative ? 'l' : 'L')} {step.Point.X} {step.Point.Y}");

  private void AppendBezier2To(Bezier2ToStep step)
      => Add($"{(step.Relative ? 'q' : 'Q')} {step.Point0.X} {step.Point0.Y}, {step.Point1.X} {step.Point1.Y}");

  private void AppendBezier3To(Bezier3ToStep step)
      => Add($"{(step.Relative ? 'c' : 'C')} {step.Point0.X} {step.Point0.Y}, {step.Point1.X} {step.Point1.Y}, {step.Point2.X} {step.Point2.Y}");

  private void AppendArcTo(ArcToStep step)
  {
    int ilarge = step.Large ? 1 : 0;
    int isweep = step.Sweep ? 1 : 0;
    Add($"{(step.Relative ? 'a' : 'A')} {step.Radius.X} {step.Radius.Y} {step.Rotation} {ilarge} {isweep} {step.Point.X} {step.Point.Y}");
  }

  private void AppendClose(CloseStep step) => Add("z");
}

public static class PathSvgWriterExtensions
{
  public static string ToSvgString(this Path self, PathSvgExporter writer = null)
  {
    writer = writer ?? new PathSvgExporter();
    return writer.WriteSvgString(self);
  }
}
