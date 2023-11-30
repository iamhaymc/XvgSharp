namespace Xvg;

public static partial class BoxFitTypeSvgExtensions
{
  public const string SvgXMidYMidMeet = "xMidYMid meet";
  public const string SvgXMidYMidSlice = "xMidYMid slice";
  public const string SvgNone = "none";

  public static string ToSvgStyle(this BoxFitType self)
  {
    switch (self)
    {
      case BoxFitType.XMidYMidMeet: return SvgXMidYMidMeet;
      case BoxFitType.XMidYMidSlice: return SvgXMidYMidSlice;
      case BoxFitType.None: return SvgNone;
      default: return null;
    }
  }
}
