namespace Xvg;

public class FillStyle
{
  public IColor? Color { get; set; }
  public FillRuleType? Rule { get; set; }
}

public enum FillRuleType
{
  NonZero,
  EvenOdd
}

public static partial class FillRuleTypeSvgExtensions
{
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