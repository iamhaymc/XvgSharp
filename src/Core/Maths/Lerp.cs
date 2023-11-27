namespace Xvg;

public static class Lerp
{
  public static byte To(byte a, byte b, double t)
    => (byte)Math.Round(a * (1 - t) + b * t);
  public static float To(float a, float b, float t)
    => a * (1 - t) + b * t;
  public static double To(double a, double b, double t)
    => a * (1 - t) + b * t;
  public static Vector2 To(Vector2 a, Vector2 b, double t)
    => a.Lerp(b, t);
  public static RgbColor To(RgbColor a, RgbColor b, double t)
    => a.Lerp(b, t);
  public static HclColor To(HclColor a, HclColor b, double t)
    => a.Lerp(b, t);
  public static DateTime To(DateTime a, DateTime b, double t)
    => new DateTime((long)To(a.Ticks, b.Ticks, t));
}
