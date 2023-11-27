namespace Xvg;

public struct Range
{
  public double Min { get; set; }
  public double Max { get; set; }

  public Range(double min, double max)
  {
    Min = min; Max = max;
  }

  public double Length => Math.Abs(Max - Min);

  public double[] ToSteps(int count) => Step(Min, Max, count);
  public double[] ToSteps(double step) => Step(Min, Max, step);
  public double[] ToTicks(int count, bool fit = false) => Tick(Min, Max, count, fit);

  public static Range From(double min, double max)
    => new Range(min, max);

  public static Range From(double max)
    => From(0, max);

  public static Range FromExtent(IEnumerable<double> values)
    => new Range(values.Min(), values.Max());

  public static Range FromExtent(IEnumerable<float> values)
    => new Range(values.Min(), values.Max());

  public static Range FromExtent<T>(IEnumerable<T> values, Func<T, double> selector)
    => new Range(values.Min(selector), values.Max(selector));

  public static Range FromExtent<T>(IEnumerable<T> values, Func<T, float> selector)
    => new Range(values.Min(selector), values.Max(selector));

  public static double[] Step(double start, double stop, int count)
  {
    if (start == stop)
      count = 1;
    switch (count)
    {
      case 0: return new double[0];
      case 1: return new[] { start + (stop - start) / 2 };
      default:
        List<double> parts = new List<double>();
        double step = (stop - start) / (count - 1);
        parts.Add(start);
        for (int i = 0; i < count - 2; ++i) parts.Add(start += step);
        parts.Add(stop);
        return parts.ToArray();
    }
  }
  public static double[] Step(double start, double stop, double step = 1f)
  {
    int n = (int)Math.Max(0, Math.Ceiling((stop - start) / step));
    double[] values = new double[n];
    for (int i = 0; i < n; ++i)
      values[i] = start + i * step;
    return values;
  }
  public static double[] Step(Range range, double step = 1f)
    => Step(range.Min, range.Max, step);

  public static double[] Tick(double start, double stop, int count, bool fit = false)
  {
    double[] ticks;
    bool reverse;
    double n, step;
    int i = -1;
    if (fit)
    {
      var newRange = TickFit(start, stop, count);
      start = newRange.Min;
      stop = newRange.Max;
    }
    if (start == stop && count > 0)
      return new double[] { start };
    if (reverse = stop < start)
    {
      n = start;
      start = stop;
      stop = n;
    }
    if ((step = TickIncr(start, stop, count)) == 0
      || !Real.Finite(step))
      return new double[0];
    if (step > 0)
    {
      int r0 = (int)Math.Round(start / step),
        r1 = (int)Math.Round(stop / step);
      if (r0 * step < start)
        ++r0;
      if (r1 * step > stop)
        --r1;
      ticks = new double[(int)(n = r1 - r0 + 1)];
      while (++i < n)
        ticks[i] = (r0 + i) * step;
    }
    else
    {
      step = -step;
      int r0 = (int)Math.Round(start * step),
        r1 = (int)Math.Round(stop * step);
      if (r0 / step < start)
        ++r0;
      if (r1 / step > stop)
        --r1;
      ticks = new double[(int)(n = r1 - r0 + 1)];
      while (++i < n)
        ticks[i] = (r0 + i) / step;
    }
    if (reverse) ticks.Reverse();
    return ticks;
  }
  public static double[] Tick(Range range, int count, bool fit = false)
  {
    return Tick(range.Min, range.Max, count, fit);
  }

  static Range TickFit(double start, double stop, int count)
  {
    double prestep = 0;
    while (true)
    {
      double step = TickIncr(start, stop, count);
      if (step == prestep || step == 0 || !Real.Finite(step))
        break;
      else if (step > 0)
      {
        start = Math.Floor(start / step) * step;
        stop = Math.Ceiling(stop / step) * step;
      }
      else if (step < 0)
      {
        start = Math.Ceiling(start * step) / step;
        stop = Math.Floor(stop * step) / step;
      }
      prestep = step;
    }
    return new Range(start, stop);
  }
  static double TickIncr(double start, double stop, int count)
  {
    double step = (stop - start) / Math.Max(0, count);
    double power = Math.Floor(Math.Log(step) / Real.LN10);
    double error = step / Math.Pow(10, power);
    return power >= 0
      ? (error >= Real.E10 ? 10
      : error >= Real.E5 ? 5
      : error >= Real.E2 ? 2 : 1) * Math.Pow(10, power)
      : -Math.Pow(10, -power) / (error >= Real.E10
      ? 10 : error >= Real.E5
      ? 5 : error >= Real.E2
      ? 2 : 1);
  }
  static double TickStep(double start, double stop, int count)
  {
    double step0 = Math.Abs(stop - start) / Math.Max(0, count);
    double step1 = Math.Pow(10, Math.Floor(Math.Log(step0) / Real.LN10));
    double error = step0 / step1;
    if (error >= Real.E10)
      step1 *= 10;
    else if (error >= Real.E5)
      step1 *= 5;
    else if (error >= Real.E2)
      step1 *= 2;
    return stop < start ? -step1 : step1;
  }
}
