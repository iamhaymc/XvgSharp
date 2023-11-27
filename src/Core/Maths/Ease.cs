namespace Xvg;

public static class Ease
{
  const float DefaultPolynomialExponent = 2.0f;

  public static double PolynomialIn(double t,
    double e = DefaultPolynomialExponent)
  {
    return Math.Pow(t, e);
  }

  public static float PolynomialIn(float t,
    float e = DefaultPolynomialExponent)
  {
    return (float)PolynomialIn((double)t, e);
  }

  public static double PolynomialOut(double t,
    double e = DefaultPolynomialExponent)
  {
    return 1 - Math.Pow(1 - t, e);
  }

  public static float PolynomialOut(float t,
    float e = DefaultPolynomialExponent)
  {
    return (float)PolynomialOut((double)t, e);
  }

  public static double PolynomialInOut(double t,
    double e = DefaultPolynomialExponent)
  {
    return ((t *= 2) <= 1 ? Math.Pow(t, e) : 2 - Math.Pow(2 - t, e)) / 2;
  }

  public static float PolynomialInOut(float t,
    float e = DefaultPolynomialExponent)
  {
    return (float)PolynomialInOut((double)t, e);
  }

  public static double ExponentialIn(double t)
  {
    return Tpmt(1 - t);
  }

  public static float ExponentialIn(float t)
  {
    return (float)ExponentialIn((double)t);
  }

  public static double ExponentialOut(double t)
  {
    return 1 - Tpmt(t);
  }

  public static float ExponentialOut(float t)
  {
    return (float)ExponentialOut((double)t);
  }

  public static double ExponentialInOut(double t)
  {
    return ((t *= 2) <= 1 ? Tpmt(1 - t) : 2 - Tpmt(t - 1)) / 2;
  }

  public static float ExponentialInOut(float t)
  {
    return (float)ExponentialInOut((double)t);
  }

  public static double CircleIn(double t)
  {
    return 1 - Math.Sqrt(1 - t * t);
  }

  public static float CircleIn(float t)
  {
    return (float)CircleIn((double)t);
  }

  public static double CircleOut(double t)
  {
    return Math.Sqrt(1 - --t * t);
  }

  public static float CircleOut(float t)
  {
    return (float)CircleOut((double)t);
  }

  public static double CircleInOut(double t)
  {
    return ((t *= 2) <= 1
      ? 1 - Math.Sqrt(1 - t * t)
      : Math.Sqrt(1 - (t -= 2) * t) + 1) / 2;
  }

  public static float CircleInOut(float t)
  {
    return (float)CircleInOut((double)t);
  }

  const float DefaultElasticAmplitude = 1.0f;
  const float DefaultElasticPeriod = 0.3f;

  public static double ElasticIn(double t,
    double amplitude = DefaultElasticAmplitude, double period = DefaultElasticPeriod)
  {
    double s = Math.Asin(1 / (amplitude = Math.Max(1, amplitude))) * (period /= Radial.Tau);
    return amplitude * Tpmt(-(--t)) * Math.Sin((s - t) / period);
  }

  public static float ElasticIn(float t,
    float amplitude = DefaultElasticAmplitude, float period = DefaultElasticPeriod)
  {
    return (float)ElasticIn((double)t, amplitude, period);
  }

  public static double ElasticOut(double t,
    double amplitude = DefaultElasticAmplitude, double period = DefaultElasticPeriod)
  {
    double s = Math.Asin(1 / (amplitude = Math.Max(1, amplitude))) * (period /= Radial.Tau);
    return 1 - amplitude * Tpmt(t = +t) * Math.Sin((t + s) / period);
  }

  public static float ElasticOut(float t,
    float amplitude = DefaultElasticAmplitude, float period = DefaultElasticPeriod)
  {
    return (float)ElasticOut((double)t, amplitude, period);
  }

  public static double ElasticInOut(double t,
    double amplitude = DefaultElasticAmplitude, double period = DefaultElasticPeriod)
  {
    double s = Math.Asin(1 / (amplitude = Math.Max(1, amplitude))) * (period /= Radial.Tau);
    return ((t = t * 2 - 1) < 0
      ? amplitude * Tpmt(-t) * Math.Sin((s - t) / period)
      : 2 - amplitude * Tpmt(t) * Math.Sin((s + t) / period)) / 2;
  }

  public static float ElasticInOut(float t,
    float amplitude = DefaultElasticAmplitude, float period = DefaultElasticPeriod)
  {
    return (float)ElasticInOut((double)t, amplitude, period);
  }

  const float
    Bounce1 = 4 / 11, Bounce2 = 6 / 11,
    Bounce3 = 8 / 11, Bounce4 = 3 / 4,
    Bounce5 = 9 / 11, Bounce6 = 10 / 11,
    Bounce7 = 15 / 16, Bounce8 = 21 / 22,
    Bounce9 = 63 / 64, Bounce0 = 1 / Bounce1 / Bounce1;

  public static double BounceIn(double t)
  {
    return 1 - BounceOut(1 - t);
  }

  public static float BounceIn(float t)
  {
    return (float)BounceIn((double)t);
  }

  public static double BounceOut(double t)
  {
    return (t = +t) < Bounce1
      ? Bounce0 * t * t
      : t < Bounce3
      ? Bounce0 * (t -= Bounce2) * t + Bounce4
      : t < Bounce6
      ? Bounce0 * (t -= Bounce5) * t + Bounce7
      : Bounce0 * (t -= Bounce8) * t + Bounce9;
  }

  public static float BounceOut(float t)
  {
    return (float)BounceOut((double)t);
  }

  public static double BounceInOut(double t)
  {
    return ((t *= 2) <= 1
      ? 1 - BounceOut(1 - t)
      : BounceOut(t - 1) + 1) / 2;
  }

  public static float BounceInOut(float t)
  {
    return (float)BounceInOut((double)t);
  }

  static double Tpmt(double x) => (Math.Pow(2, -10 * x) - 0.0009765625) * 1.0009775171065494;
}
