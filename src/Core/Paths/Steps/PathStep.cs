namespace Xvg;

public enum PathStepType
{
  MoveTo, LineTo, Bezier2To, Bezier3To, ArcTo, Close
}

public interface IPathStep
{
  PathStepType Type { get; }
  bool Relative { get; }
  IPathStep Translate(Vector2 translation);
  IPathStep Scale(Vector2 scale);
  Vector2? ToPoint();
}

public abstract class StepBase : IPathStep
{
  public abstract PathStepType Type { get; }
  public bool Relative { get; set; }
  public abstract IPathStep Translate(Vector2 translation);
  public abstract IPathStep Scale(Vector2 scale);
  public abstract Vector2? ToPoint();
}

public static class PathStepExtensions
{
  public static bool IsMoveTo(this IPathStep self)
      => self.Type == PathStepType.MoveTo;
  public static bool IsLineTo(this IPathStep self)
      => self.Type == PathStepType.LineTo;
  public static bool IsBezier2To(this IPathStep self)
      => self.Type == PathStepType.Bezier2To;
  public static bool IsBezier3To(this IPathStep self)
      => self.Type == PathStepType.Bezier3To;
  public static bool IsArcTo(this IPathStep self)
      => self.Type == PathStepType.ArcTo;
  public static bool IsClose(this IPathStep self)
      => self.Type == PathStepType.Close;

  public static MoveToStep AsMoveTo(this IPathStep self)
      => self.IsMoveTo() ? (MoveToStep)self : null;
  public static LineToStep AsLineTo(this IPathStep self)
      => self.IsLineTo() ? (LineToStep)self : null;
  public static Bezier2ToStep AsBezier2To(this IPathStep self)
      => self.IsBezier2To() ? (Bezier2ToStep)self : null;
  public static Bezier3ToStep AsBezier3To(this IPathStep self)
      => self.IsBezier3To() ? (Bezier3ToStep)self : null;
  public static ArcToStep AsArcTo(this IPathStep self)
      => self.IsArcTo() ? (ArcToStep)self : null;
  public static CloseStep AsClose(this IPathStep self)
      => self.IsClose() ? (CloseStep)self : null;
}
