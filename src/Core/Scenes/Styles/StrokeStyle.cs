namespace Xvg;

public enum StrokeCapType
{
  Butt, Round, Square
}

public enum StrokeJointType
{
  Miter, Round, Bevel
}

public static partial class StrokeStyle
{
  public static readonly ColorType DefaultColor = ColorType.Black;
  public static readonly StrokeCapType DefaultCap = StrokeCapType.Butt;
  public static readonly StrokeJointType DefaultJoint = StrokeJointType.Miter;
  public static readonly float DefaultWidth = 1;

  public const string CapButt = "butt";
  public const string CapRound = "round";
  public const string CapSquare = "square";

  public const string JointMiter = "miter";
  public const string JointRound = "round";
  public const string JointBevel = "bevel";

  public static string ToStyle(this StrokeCapType self)
  {
    switch (self)
    {
      case StrokeCapType.Butt: return CapButt;
      case StrokeCapType.Round: return CapRound;
      case StrokeCapType.Square: return CapSquare;
      default: throw new NotSupportedException();
    }
  }

  public static string ToStyle(this StrokeJointType self)
  {
    switch (self)
    {
      case StrokeJointType.Miter: return JointMiter;
      case StrokeJointType.Round: return JointRound;
      case StrokeJointType.Bevel: return JointBevel;
      default: throw new NotSupportedException();
    }
  }
}
