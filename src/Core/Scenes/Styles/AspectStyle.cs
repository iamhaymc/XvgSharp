namespace Xvg;

public static partial class AspectStyle
{
  public const AspectType Default = AspectType.XMidYMidMeet;

  public const string XMidYMidMeet = "xMidYMid meet";
  public const string XMidYMidSlice = "xMidYMid slice";
  public const string None = "none";

  public static string ToStyle(this AspectType self)
  {
    switch (self)
    {
      case AspectType.XMidYMidMeet: return XMidYMidMeet;
      case AspectType.XMidYMidSlice: return XMidYMidSlice;
      case AspectType.None: return None;
      default: return null;
    }
  }
}
