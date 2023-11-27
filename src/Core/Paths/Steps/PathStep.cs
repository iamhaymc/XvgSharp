namespace Xvg;

public enum VgPathStepType
{
  MoveTo, LineTo, Bezier2To, Bezier3To, ArcTo, Close
}

public interface IVgPathStep
{
  VgPathStepType Type { get; }
  bool IsRelative { get; }
  IVgPathStep Translate(Vector2 translation);
  IVgPathStep Scale(Vector2 scale);
  Vector2? ToPoint();
}

public abstract class VgBaseStep : IVgPathStep
{
  public abstract VgPathStepType Type { get; }
  public bool IsRelative { get; set; }
  public abstract IVgPathStep Translate(Vector2 translation);
  public abstract IVgPathStep Scale(Vector2 scale);
  public abstract Vector2? ToPoint();
}

public static class VgPathStepExtensions
{
  public static bool IsMoveTo(this IVgPathStep self)
      => self.Type == VgPathStepType.MoveTo;
  public static bool IsLineTo(this IVgPathStep self)
      => self.Type == VgPathStepType.LineTo;
  public static bool IsBezier2To(this IVgPathStep self)
      => self.Type == VgPathStepType.Bezier2To;
  public static bool IsBezier3To(this IVgPathStep self)
      => self.Type == VgPathStepType.Bezier3To;
  public static bool IsArcTo(this IVgPathStep self)
      => self.Type == VgPathStepType.ArcTo;
  public static bool IsClose(this IVgPathStep self)
      => self.Type == VgPathStepType.Close;

  public static VgMoveToStep AsMoveTo(this IVgPathStep self)
      => self.IsMoveTo() ? (VgMoveToStep)self : null;
  public static VgLineToStep AsLineTo(this IVgPathStep self)
      => self.IsLineTo() ? (VgLineToStep)self : null;
  public static VgBezier2ToStep AsBezier2To(this IVgPathStep self)
      => self.IsBezier2To() ? (VgBezier2ToStep)self : null;
  public static VgBezier3ToStep AsBezier3To(this IVgPathStep self)
      => self.IsBezier3To() ? (VgBezier3ToStep)self : null;
  public static VgArcToStep AsArcTo(this IVgPathStep self)
      => self.IsArcTo() ? (VgArcToStep)self : null;
  public static VgCloseStep AsClose(this IVgPathStep self)
      => self.IsClose() ? (VgCloseStep)self : null;
}
