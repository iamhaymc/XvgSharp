namespace Xvg;

public enum FillRuleType
{
  NonZero, EvenOdd
}

public static partial class FillStyle
{
  public const ColorType DefaultColor = ColorType.White;
  public const FillRuleType DefaultRule = FillRuleType.NonZero;

  public const string NonZero = "nonzero";
  public const string EvenOdd = "evenodd";

  public static string ToStyle(this FillRuleType self)
  {
    switch (self)
    {
      case FillRuleType.NonZero: return NonZero;
      case FillRuleType.EvenOdd: return EvenOdd;
      default: throw new NotSupportedException();
    }
  }
}