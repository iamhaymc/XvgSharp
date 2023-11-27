namespace Xvg;

public class VgPathSvgWriter : VgTextWriter
{
  public VgPath Path { get; set; }

  public string WriteSvgString(VgPath path)
  {
    Reset();
    Path = path;
    foreach (IVgPathStep step in Path.Steps)
      AppendStep(step);
    return Dump();
  }

  private void AppendStep(IVgPathStep step)
  {
    switch (step.Type)
    {
      case VgPathStepType.MoveTo:
        AppendMoveTo((VgMoveToStep)step);
        break;
      case VgPathStepType.LineTo:
        AppendLineTo((VgLineToStep)step);
        break;
      case VgPathStepType.Bezier2To:
        AppendBezier2To((VgBezier2ToStep)step);
        break;
      case VgPathStepType.Bezier3To:
        AppendBezier3To((VgBezier3ToStep)step);
        break;
      case VgPathStepType.ArcTo:
        AppendArcTo((VgArcToStep)step);
        break;
      case VgPathStepType.Close:
        AppendClose((VgCloseStep)step);
        break;
      default: return;
    }
  }

  private void AppendMoveTo(VgMoveToStep step)
      => Add($"{(step.IsRelative ? 'm' : 'M')} {step.Point.X} {step.Point.Y}");

  private void AppendLineTo(VgLineToStep step)
      => Add($"{(step.IsRelative ? 'l' : 'L')} {step.Point.X} {step.Point.Y}");

  private void AppendBezier2To(VgBezier2ToStep step)
      => Add($"{(step.IsRelative ? 'q' : 'Q')} {step.Point0.X} {step.Point0.Y}, {step.Point1.X} {step.Point1.Y}");

  private void AppendBezier3To(VgBezier3ToStep step)
      => Add($"{(step.IsRelative ? 'c' : 'C')} {step.Point0.X} {step.Point0.Y}, {step.Point1.X} {step.Point1.Y}, {step.Point2.X} {step.Point2.Y}");

  private void AppendArcTo(VgArcToStep step)
  {
    int ilarge = step.Large ? 1 : 0;
    int isweep = step.Sweep ? 1 : 0;
    Add($"{(step.IsRelative ? 'a' : 'A')} {step.Radius.X} {step.Radius.Y} {step.Rotation} {ilarge} {isweep} {step.Point.X} {step.Point.Y}");
  }

  private void AppendClose(VgCloseStep step) => Add("z");
}

public static class VgPathSvgWriterExtensions
{
  public static string ToSvgString(this VgPath self, VgPathSvgWriter writer = null)
  {
    writer = writer ?? new VgPathSvgWriter();
    return writer.WriteSvgString(self);
  }
}
