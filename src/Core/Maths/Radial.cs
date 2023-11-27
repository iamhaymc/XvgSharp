namespace Xvg;

public static class Radial
{
  public static readonly double Tau = Math.PI * 2;

  public static readonly float PIF = (float)Math.PI;
  public static readonly float PI2F = (float)(Math.PI / 2);
  public static readonly float PI4F = (float)(Math.PI / 4);
  public static readonly float TauF = (float)(Math.PI * 2);

  public static double ToRadians(double degrees)
  {
    return Math.PI / 180.0 * degrees;
  }
  public static float ToRadians(float degrees)
  {
    return (float)ToRadians((double)degrees);
  }

  public static double ToDegrees(double radians)
  {
    return 180.0 / Math.PI * radians;
  }
  public static float ToDegrees(float radians)
  {
    return (float)ToDegrees((double)radians);
  }

  public static Vector2 ToPoint(Vector2 origin, double radius, double angle)
  {
    return new Vector2(
       origin.X + (float)(Math.Cos(angle) * radius),
       origin.Y + (float)(Math.Sin(angle) * radius));
  }
  public static Vector2 ToPoint(Vector2 origin, float radius, float angle)
  {
    return ToPoint(origin, radius, (double)angle);
  }
}
