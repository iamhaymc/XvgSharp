namespace Xvg;

public static partial class AspectStyle
{
  public const BoxFitType Default = BoxFitType.XMidYMidMeet;

  public const string XMidYMidMeet = "xMidYMid meet";
  public const string XMidYMidSlice = "xMidYMid slice";
  public const string None = "none";

  public static string ToStyle(this BoxFitType self)
  {
    switch (self)
    {
      case BoxFitType.XMidYMidMeet: return XMidYMidMeet;
      case BoxFitType.XMidYMidSlice: return XMidYMidSlice;
      case BoxFitType.None: return None;
      default: return null;
    }
  }
}
