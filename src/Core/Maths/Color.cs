using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Xvg;

public interface IColor
{ }

public struct RgbColor : IColor
{
  public byte R { get; set; }
  public byte G { get; set; }
  public byte B { get; set; }
  public byte A { get; set; }

  public RgbColor(byte r, byte g, byte b, byte a = 255)
  {
    R = r;
    G = g;
    B = b;
    A = a;
  }

  public RgbColor Lerp(RgbColor v, double t)
    => Interpolate(this, v, t);

  private static Regex HexRegex = new Regex("^#([0-9a-f]{3,8})$");

  public static RgbColor Interpolate(RgbColor a, RgbColor b, double t)
  {
    Vector4 lerped = Vector4.Lerp(
      new Vector4(a.R, a.B, a.G, a.A),
      new Vector4(b.R, b.B, b.G, b.A), (float)t);
    return new RgbColor(
      (byte)lerped.X,
      (byte)lerped.Y,
      (byte)lerped.Z,
      (byte)lerped.W);
  }

  public static RgbColor? FromHex(string hex)
  {
    Match match = HexRegex.Match(hex);
    if (!match.Success)
      return null;
    int r, g, b, a;
    int l = match.Groups[1].Length;
    int m = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
    switch (l)
    {
      case 3:
        r = (m >> 8 & 0xf) | (m >> 4 & 0xf0);
        g = (m >> 4 & 0xf) | (m & 0xf0);
        b = ((m & 0xf) << 4) | (m & 0xf);
        a = 255;
        break;
      case 4:
        r = (m >> 12 & 0xf) | (m >> 8 & 0xf0);
        g = (m >> 8 & 0xf) | (m >> 4 & 0xf0);
        b = (m >> 4 & 0xf) | (m & 0xf0);
        a = (((m & 0xf) << 4) | (m & 0xf)) / 0xff;
        break;
      case 6:
        r = m >> 16 & 0xff;
        g = m >> 8 & 0xff;
        b = m & 0xff;
        a = 255;
        break;
      case 8:
        r = m >> 24 & 0xff;
        g = m >> 16 & 0xff;
        b = m >> 8 & 0xff;
        a = m & 0xff;
        break;
      default:
        return null;
    }
    return new RgbColor((byte)r, (byte)g, (byte)b, (byte)a);
  }

  public string ToHex(bool alpha = true)
  {
    return alpha
        ? $"#{R:X2}{G:X2}{B:X2}{A:X2}"
        : $"#{R:X2}{G:X2}{B:X2}";
  }

  public static RgbColor? FromHcl(HclColor hcl)
  {
    return HclColor.ToColor(hcl);
  }

  public HclColor ToHcl()
  {
    return HclColor.FromColor(this);
  }

  public static readonly RgbColor Zero = new RgbColor(0, 0, 0, 0);
  public static readonly RgbColor Black = new RgbColor(0, 0, 0, 255);
  public static readonly RgbColor White = new RgbColor(255, 255, 255, 255);
}

public struct XyzColor : IColor
{
  // https://github.com/muak/ColorMinePortable/blob/master/ColorMinePortable/ColorSpaces/Conversions/XyzConverter.cs

  public double X { get; set; }
  public double Y { get; set; }
  public double Z { get; set; }

  public XyzColor(double x, double y, double z)
  {
    X = x;
    Y = y;
    Z = z;
  }

  internal const double Epsilon = 0.008856;
  internal const double Kappa = 903.3;
  internal static readonly XyzColor WhiteReference = new XyzColor(95.047, 100.000, 108.883);

  public static XyzColor FromColor(RgbColor color)
  {
    double r = PivotRgb(color.R / 255.0);
    double g = PivotRgb(color.G / 255.0);
    double b = PivotRgb(color.B / 255.0);

    return new XyzColor(
        r * 0.4124 + g * 0.3576 + b * 0.1805,
        r * 0.2126 + g * 0.7152 + b * 0.0722,
        r * 0.0193 + g * 0.1192 + b * 0.9505
    );
  }

  public static RgbColor ToColor(XyzColor item)
  {
    double x = item.X / 100.0;
    double y = item.Y / 100.0;
    double z = item.Z / 100.0;

    double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
    double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
    double b = x * 0.0557 + y * -0.2040 + z * 1.0570;

    r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
    g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
    b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;

    return new RgbColor(
        r: Real.Quantize(r),
        g: Real.Quantize(g),
        b: Real.Quantize(b)
    );
  }

  private static double PivotRgb(double n)
  {
    return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
  }
}

public struct LabColor : IColor
{
  // https://github.com/muak/ColorMinePortable/blob/master/ColorMinePortable/ColorSpaces/Conversions/LabConverter.cs

  public double L { get; set; }
  public double A { get; set; }
  public double B { get; set; }

  public LabColor(double l, double a, double b)
  {
    L = l;
    A = a;
    B = b;
  }

  public static LabColor FromColor(RgbColor color)
  {
    XyzColor xyz = XyzColor.FromColor(color);

    XyzColor white = XyzColor.WhiteReference;
    double x = PivotXyz(xyz.X / white.X);
    double y = PivotXyz(xyz.Y / white.Y);
    double z = PivotXyz(xyz.Z / white.Z);

    return new LabColor(
        l: Math.Max(0, 116 * y - 16),
        a: 500 * (x - y),
        b: 200 * (y - z));
  }

  public static RgbColor ToColor(LabColor item)
  {
    double y = (item.L + 16.0) / 116.0;
    double x = item.A / 500.0 + y;
    double z = y - item.B / 200.0;

    var white = XyzColor.WhiteReference;
    double x3 = x * x * x;
    double z3 = z * z * z;

    return XyzColor.ToColor(new XyzColor
    (
        x: white.X * (x3 > XyzColor.Epsilon ? x3 : (x - 16.0 / 116.0) / 7.787),
        y: white.Y * (item.L > (XyzColor.Kappa * XyzColor.Epsilon)
           ? Math.Pow(((item.L + 16.0) / 116.0), 3)
           : item.L / XyzColor.Kappa),
        z: white.Z * (z3 > XyzColor.Epsilon ? z3 : (z - 16.0 / 116.0) / 7.787)
    ));
  }

  private static double PivotXyz(double n)
  {
    return n > XyzColor.Epsilon ? CubeRoot(n) : (XyzColor.Kappa * n + 16) / 116;
  }

  private static double CubeRoot(double n)
  {
    return Math.Pow(n, 1.0 / 3.0);
  }
}

public struct HclColor : IColor
{
  // https://github.com/muak/ColorMinePortable/blob/master/ColorMinePortable/ColorSpaces/Conversions/LchConverter.cs

  public double H { get; set; }
  public double C { get; set; }
  public double L { get; set; }

  public HclColor(double h, double c, double l)
  {
    H = h;
    C = c;
    L = l;
  }

  public HclColor Lerp(HclColor v, double t)
    => Interpolate(this, v, t);

  public static HclColor Interpolate(HclColor a, HclColor b, double t)
  {
    return new HclColor(
        h: InterpolateHue(a.H, b.H, t),
        c: InterpolateNoGamma(a.C, b.C, t),
        l: InterpolateNoGamma(a.L, b.L, t));
  }

  private static double InterpolateHue(double a, double b, double t)
  {
    double d = b - a;
    return d != 0
        ? InterpolateLinear(a, d > 180f || d < -180f ? d - 360f * (double)Math.Round(d / 360) : d, t)
        : double.IsNaN(a) ? b : a;
  }

  private static double InterpolateNoGamma(double a, double b, double t)
  {
    double d = b - a;
    return d != 0
        ? InterpolateLinear(a, d, t)
        : double.IsNaN(a) ? b : a;
  }

  private static double InterpolateLinear(double a, double d, double t)
  {
    return a + t * d;
  }

  public static HclColor FromColor(RgbColor color)
  {
    LabColor lab = LabColor.FromColor(color);

    double h = Math.Atan2(lab.B, lab.A);
    if (h > 0)
      h = (h / Math.PI) * 180.0;
    else
      h = 360 - (Math.Abs(h) / Math.PI) * 180.0;
    if (h < 0)
      h += 360.0;
    else if (h >= 360)
      h -= 360.0;

    return new HclColor(
        l: lab.L,
        c: Math.Sqrt(lab.A * lab.A + lab.B * lab.B),
        h: h);
  }

  public static RgbColor ToColor(HclColor item)
  {
    double hrads = item.H * Math.PI / 180.0;

    return LabColor.ToColor(new LabColor(
        l: item.L,
        a: Math.Cos(hrads) * item.C,
        b: Math.Sin(hrads) * item.C
    ));
  }
}
