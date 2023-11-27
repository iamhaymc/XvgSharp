using System;

namespace Xvg;

public interface IRng
{
  /// <summary>
  /// Produces a random double in the range [0, 1]
  /// </summary>
  double Next();
  /// <summary>
  /// Produces a random integer in the range [0, max]
  /// </summary>
  int NextInt(int max = int.MaxValue);
  /// <summary>
  /// Produces a random integer in the range [min, max]
  /// </summary>
  int NextInt(int min, int max);
  /// <summary>
  /// Produces a random unsigned integer in the range
  /// </summary>
  uint NextUInt();
};

public abstract class RngBase : IRng
{
  /// <inheritdoc/>
  public abstract double Next();

  /// <inheritdoc/>
  public virtual int NextInt(int max = int.MaxValue)
  {
    return (int)(Next() * 2.0 - 1.0 * max);
  }

  /// <inheritdoc/>
  public virtual int NextInt(int min, int max)
  {
    return NextInt(max - min + 1) + min;
  }

  /// <inheritdoc/>
  public virtual uint NextUInt()
  {
    return ((uint)NextInt(1 << 30) << 2) | (uint)NextInt(1 << 2);
  }
}

public class UniformRng : RngBase
{
  private Random _rng;

  public UniformRng(Random? rng = null)
  {
    _rng = rng ?? new Random();
  }

  /// <inheritdoc/>
  public override double Next()
  {
    return _rng.NextDouble();
  }

  /// <inheritdoc/>
  public override int NextInt(int max = int.MaxValue)
  {
    return _rng.Next(max);
  }

  /// <inheritdoc/>
  public override int NextInt(int min, int max)
  {
    return _rng.Next(min, max);
  }
}

public class NormalRng : RngBase
{
  public double Mu { get; set; }
  public double Sigma { get; set; }

  private Random _rng;
  private double? _x, _r;

  public NormalRng(double mu = 0, double sigma = 1, Random? rng = null)
  {
    Mu = mu;
    Sigma = sigma;
    _rng = rng ?? new Random();
    _x = _r = null;
  }

  /// <inheritdoc/>
  public override double Next()
  {
    double y;
    if (_x != null) { y = _x.Value; _x = null; }
    else do
      {
        _x = _rng.NextDouble() * 2 - 1;
        y = _rng.NextDouble() * 2 - 1;
        _r = _x * _x + y * y;
      } while (_r == 0 || _r > 1);
    return Mu + Sigma * y * Math.Sqrt(-2 * Math.Log(_r!.Value) / _r.Value);
  }
}
