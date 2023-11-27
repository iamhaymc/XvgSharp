namespace Xvg;

public static class Real
{
  public static readonly double LN10 = Math.Log(10);
  public static readonly double E10 = Math.Sqrt(50);
  public static readonly double E5 = Math.Sqrt(10);
  public static readonly double E2 = Math.Sqrt(2);

  public static readonly float EpsilonF = 1e-6f;

  public static bool Finite(double value)
  {
    return !double.IsInfinity(value) && !double.IsNaN(value);
  }
  public static bool Finite(float value)
  {
    return !float.IsInfinity(value) && !float.IsNaN(value);
  }

  public static double Clamp(double v, double min, double max)
  {
    return Math.Max(Math.Min(v, max), min);
  }
  public static float Clamp(float v, float min, float max)
  {
    return Math.Max(Math.Min(v, max), min);
  }

  public static double Normalize(double v, double min, double max)
  {
    return (max -= min) != 0
      ? (v - min) / max : (double.IsNaN(max) ? double.NaN : 0.5);
  }
  public static float Normalize(float v, float min, float max)
  {
    return (float)Normalize((double)v, min, max);
  }

  public static byte Quantize(double v)
  {
    return (byte)(v >= 1.0 ? 255 : (v <= 0.0 ? 0 : Math.Floor(v * 256.0)));
  }
  public static byte Quantize(float v)
  {
    return Quantize((double)v);
  }
  public static double Quantize(double v, long n)
  {
    return v >= 1.0 ? n : (v <= 0.0 ? 0 : Math.Floor(v * (n + 1)));
  }
  public static float Quantize(float v, long n)
  {
    return (float)Quantize((double)v, n);
  }

  public static float Dequantize(byte v)
  {
    return v / 255.0f;
  }
  public static float Dequantize(float v, long n)
  {
    return v / n;
  }
}
