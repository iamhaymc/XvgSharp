namespace Xvg;

public class VgPathCsExporter : TextWriter
{
  public Path Path { get; set; }

  private string _preScaleXTerm, _preScaleYTerm;
  private string _translateXTerm, _translateYTerm;
  private string _postScaleXTerm, _postScaleYTerm;

  public string WriteCsString(Path path,
    string preScaleXTerm = null, string preScaleYTerm = null,
    string translateXTerm = null, string translateYTerm = null,
    string postScaleXTerm = null, string postScaleYTerm = null)
  {
    Reset();
    Path = path;
    _preScaleXTerm = preScaleXTerm; _preScaleYTerm = preScaleYTerm;
    _translateXTerm = translateXTerm; _translateYTerm = translateYTerm;
    _postScaleXTerm = postScaleXTerm; _postScaleYTerm = postScaleYTerm;
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
    => Add(FormatInvocation("MoveTo",
        FormatFloat(step.Point.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatBoolean(step.Relative)));

  private void AppendLineTo(LineToStep step)
    => Add(FormatInvocation("LineTo",
        FormatFloat(step.Point.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatBoolean(step.Relative)));

  private void AppendBezier2To(Bezier2ToStep step)
    => Add(FormatInvocation("Bezier2To",
        FormatFloat(step.Point0.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point0.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point1.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point1.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatBoolean(step.Relative)));

  private void AppendBezier3To(Bezier3ToStep step)
    => Add(FormatInvocation("Bezier3To",
        FormatFloat(step.Point0.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point0.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point1.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point1.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point2.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point2.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatBoolean(step.Relative)));

  private void AppendArcTo(ArcToStep step)
    => Add(FormatInvocation("ArcTo",
        FormatFloat(step.Point.X, _preScaleXTerm ?? _preScaleYTerm, _translateXTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Point.Y, _preScaleXTerm ?? _preScaleYTerm, _translateYTerm, _postScaleXTerm ?? _postScaleYTerm),
        FormatFloat(step.Radius.X, _preScaleXTerm, null, _postScaleXTerm),
        FormatFloat(step.Radius.Y, _preScaleYTerm, null, _postScaleYTerm),
        FormatFloat(step.Rotation),
        FormatBoolean(step.Large), FormatBoolean(step.Sweep),
        FormatBoolean(step.Relative)));

  private void AppendClose(CloseStep step)
    => Add(FormatInvocation("Close"));

  private string FormatInvocation(string name, params string[] attributes)
    => $".{name}({FormatAttributes(attributes)})";

  private string FormatAttributes(params string[] attributes)
    => string.Join(", ", attributes).Trim();

  private string FormatFloat(float value,
    string preScaleTerm = null, string offsetTerm = null, string postScaleTerm = null)
  {
    string text = value.ToString() + "f";
    if (!string.IsNullOrEmpty(preScaleTerm)) text += $" * {preScaleTerm}";
    if (!string.IsNullOrEmpty(offsetTerm)) text += $" + {offsetTerm}";
    if (!string.IsNullOrEmpty(postScaleTerm)) text += $" * {postScaleTerm}";
    return text;
  }

  private string FormatBoolean(bool value)
    => value.ToString().ToLower();
}

public static class VgPathCsWriterExtensions
{
  public static string ToCsString(this Path self,
    string preScaleXTerm = null, string preScaleYTerm = null,
    string translateXTerm = null, string translateYTerm = null,
    string postScaleXTerm = null, string postScaleYTerm = null,
    VgPathCsExporter writer = null)
  {
    return (writer ?? new VgPathCsExporter()).WriteCsString(self,
      preScaleXTerm, preScaleYTerm,
      translateXTerm, translateYTerm,
      postScaleXTerm, postScaleYTerm);
  }

  public static string ToCsString(this Path self, VgPathCsExporter writer)
    => (writer ?? new VgPathCsExporter()).WriteCsString(self);
}
