namespace Xvg;

public class StrokeStyle : ISceneStyle
{
  public IColor Color { get; set; }
  public StrokeJointType? Joint { get; set; }
  public StrokeCapType? Cap { get; set; }
  public float? Width { get; set; }
}

public enum StrokeCapType
{
  Butt, Round, Square
}

public enum StrokeJointType
{
  Miter, Round, Bevel
}

public static partial class StrokeTypeSvgExtensions
{
  public const string SvgCapButt = "butt";
  public const string SvgCapRound = "round";
  public const string SvgCapSquare = "square";

  public const string SvgJointMiter = "miter";
  public const string SvgJointRound = "round";
  public const string SvgJointBevel = "bevel";

  public static string ToSvgStyle(this StrokeCapType self)
  {
    switch (self)
    {
      case StrokeCapType.Butt: return SvgCapButt;
      case StrokeCapType.Round: return SvgCapRound;
      case StrokeCapType.Square: return SvgCapSquare;
      default: throw new NotSupportedException();
    }
  }

  public static string ToSvgStyle(this StrokeJointType self)
  {
    switch (self)
    {
      case StrokeJointType.Miter: return SvgJointMiter;
      case StrokeJointType.Round: return SvgJointRound;
      case StrokeJointType.Bevel: return SvgJointBevel;
      default: throw new NotSupportedException();
    }
  }
}