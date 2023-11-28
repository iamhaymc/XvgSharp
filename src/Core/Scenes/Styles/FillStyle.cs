namespace Xvg;

public enum FillRuleType
{
  NonZero, EvenOdd
}

public static partial class FillStyle
{
  public const ColorKind DefaultColor = ColorKind.White;
  public const FillRuleType DefaultRule = FillRuleType.NonZero;

  public const string SvgNonZero = "nonzero";
  public const string SvgEvenOdd = "evenodd";

  public static string ToSvgStyle(this FillRuleType self)
  {
    switch (self)
    {
      case FillRuleType.NonZero: return SvgNonZero;
      case FillRuleType.EvenOdd: return SvgEvenOdd;
      default: throw new NotSupportedException();
    }
  }
}
