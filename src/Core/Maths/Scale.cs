namespace Xvg;

public interface IScale<D, R>
{
}

public static class Scale
{
  public static ContinuousScale ToIdentity(double dMin, double dMax)
    => new ContinuousScale(new[] { dMin, dMax }, new[] { dMin, dMax },
  transformer: new ScaleIdentityTransformer());

  public static ContinuousScale ToIdentity(Range d, Range r)
    => ToLinear(d.Min, d.Max, r.Min, r.Max);

  public static ContinuousScale ToLinear(double dMin, double dMax, double rMin, double rMax)
    => new ContinuousScale(new[] { dMin, dMax }, new[] { rMin, rMax },
      transformer: new ScaleIdentityTransformer());

  public static ContinuousScale ToLinear(Range d, Range r)
    => ToLinear(d.Min, d.Max, r.Min, r.Max);

  public static ContinuousScale<SR> ToLinear<SR>(double dMin, double dMax, SR rMin, SR rMax, Func<SR, SR, double, SR> lerp)
    => new ContinuousScale<SR>(new[] { dMin, dMax }, new[] { rMin, rMax },
  transformer: new ScaleIdentityTransformer(),
  interpolator: lerp);

  public static ContinuousScale ToPower(double dMin, double dMax, double rMin, double rMax, double e)
    => new ContinuousScale(new[] { dMin, dMax }, new[] { rMin, rMax },
    transformer: e == 1 ? new ScaleIdentityTransformer()
    : e == 0.5 ? new ScaleSqrtTransformer()
    : (IScaleTransformer)new ScalePowerTransformer(e));

  public static ContinuousScale ToPower(Range d, Range r, double e)
    => ToPower(d.Min, d.Max, r.Min, r.Max, e);

  public static ContinuousScale<SR> ToPower<SR>(double dMin, double dMax, SR rMin, SR rMax, double e, Func<SR, SR, double, SR> lerp)
    => new ContinuousScale<SR>(new[] { dMin, dMax }, new[] { rMin, rMax },
    transformer: e == 1 ? new ScaleIdentityTransformer()
      : e == 0.5 ? new ScaleSqrtTransformer()
      : (IScaleTransformer)new ScalePowerTransformer(e),
    interpolator: lerp);

  public static ContinuousScale ToLog(double dMin, double dMax, double rMin, double rMax)
    => new ContinuousScale(new[] { dMin, dMax }, new[] { rMin, rMax },
      transformer: new ScaleLogTransformer());

  public static ContinuousScale ToLog(Range d, Range r)
    => ToLog(d.Min, d.Max, r.Min, r.Max);

  public static ContinuousScale<SR> ToLog<SR>(double dMin, double dMax, SR rMin, SR rMax, Func<SR, SR, double, SR> lerp)
    => new ContinuousScale<SR>(new[] { dMin, dMax }, new[] { rMin, rMax },
      transformer: new ScaleLogTransformer(),
      interpolator: lerp);
}

public class ContinuousScale : IScale<double, double>
{
  protected double[] _domain;
  protected double[] _range;
  protected IScaleClamper? _clamper;
  protected IScaleTransformer? _transformer;
  protected IScaleMapper<double, double>? _mapper;

  public ContinuousScale(
    double[] domain, double[] range,
    IScaleTransformer? transformer = null)
  {
    _domain = domain; _range = range;
    _transformer = transformer;
    Update();
  }

  public ContinuousScale UseDomain(params double[] domain)
  {
    _domain = domain;
    Update();
    return this;
  }
  public ContinuousScale UseRange(params double[] range)
  {
    _range = range;
    Update();
    return this;
  }
  public ContinuousScale UseClamp(IScaleClamper? clamper = null)
  {
    _clamper = clamper ?? new ScaleRangeClamper();
    Update();
    return this;
  }

  public double Scale(double value)
  {
    return _mapper.Map(
      _transformer.Transform(
        _clamper.Clamp(value)));
  }

  public double Unscale(double value)
    => _clamper.Clamp(
      _transformer.Untransform(
        _mapper.Unmap(value)));

  protected void Update()
  {
    // Update clamper
    if (_clamper != null)
      _clamper.Update(_domain, Math.Min(_domain.Length, _range.Length));
    else _clamper = new ScaleIdentityClamper();
    // Ensure transformer
    if (_transformer == null)
      _transformer = new ScaleIdentityTransformer();
    // Rebuild mapper
    _mapper = Math.Min(_domain.Length, _range.Length) > 2
      ? new ScalePolyMapper().Update(
      _domain.Select(_transformer.Transform).ToArray(), _range)
      : new ScaleBiMapper().Update(
      _domain.Select(_transformer.Transform).ToArray(), _range);
  }
}

public class ContinuousScale<R> : IScale<double, R>
{
  protected double[] _domain;
  protected R[] _range;
  protected IScaleClamper? _clamper;
  protected IScaleTransformer? _transformer;
  protected Func<R, R, double, R>? _interpolator;
  protected IScaleMapper<double, R>? _mapper;

  public ContinuousScale(
    double[] domain, R[] range,
    IScaleTransformer? transformer,
    Func<R, R, double, R> interpolator)
  {
    _domain = domain; _range = range;
    _transformer = transformer;
    _interpolator = interpolator;
    Update();
  }

  public ContinuousScale<R> UseDomain(params double[] domain)
  {
    _domain = domain;
    Update();
    return this;
  }
  public ContinuousScale<R> UseRange(params R[] range)
  {
    _range = range;
    Update();
    return this;
  }
  public ContinuousScale<R> UseClamp(IScaleClamper? clamper = null)
  {
    _clamper = clamper ?? new ScaleRangeClamper();
    Update();
    return this;
  }

  public R Scale(double value)
  {
    return _mapper.Map(
      _transformer.Transform(
        _clamper.Clamp(value)));
  }

  protected virtual void Update()
  {
    // Update clamper
    if (_clamper != null)
      _clamper.Update(_domain, Math.Min(_domain.Length, _range.Length));
    else _clamper = new ScaleIdentityClamper();
    // Ensure transformer
    if (_transformer == null)
      _transformer = new ScaleIdentityTransformer();
    // Rebuild mapper
    _mapper = Math.Min(_domain.Length, _range.Length) > 2
      ? new ScalePolyMapper<R>().Update(
      _domain.Select(_transformer.Transform).ToArray(), _range, _interpolator)
      : new ScaleBiMapper<R>().Update(
      _domain.Select(_transformer.Transform).ToArray(), _range, _interpolator);
  }
}

#region [Clampers]

public interface IScaleClamper
{
  IScaleClamper Update(double[] domain, int count);
  double Clamp(double x);
}

public class ScaleIdentityClamper : IScaleClamper
{
  public IScaleClamper Update(double[] domain, int count) => this;
  public double Clamp(double x) => x;
}

public class ScaleRangeClamper : IScaleClamper
{
  double _min, _max;
  public IScaleClamper Update(double[] domain, int count)
  {
    _min = domain[0]; _max = domain[count - 1];
    return this;
  }
  public double Clamp(double x) => Real.Clamp(x, _min, _max);
}

#endregion

#region [Transformers]

public interface IScaleTransformer
{
  double Transform(double x);
  double Untransform(double x);
}

public class ScaleIdentityTransformer : IScaleTransformer
{
  public double Transform(double x) => x;
  public double Untransform(double x) => x;
}

public class ScaleSqrtTransformer : IScaleTransformer
{
  public double Transform(double x)
    => x < 0 ? -Math.Sqrt(-x) : Math.Sqrt(x);
  public double Untransform(double x)
    => x < 0 ? -x * x : x * x;
}

public class ScalePowerTransformer : IScaleTransformer
{
  double _e, _ie;
  public ScalePowerTransformer(double e)
  {
    _e = e; _ie = 1 / _e;
  }
  public double Transform(double x)
    => x < 0 ? -Math.Pow(-x, _e) : Math.Pow(x, _e);
  public double Untransform(double x)
    => x < 0 ? -Math.Pow(-x, _ie) : Math.Pow(x, _ie);
}

public class ScaleLogTransformer : IScaleTransformer
{
  public double Transform(double x) => Math.Log(x);
  public double Untransform(double x) => Math.Exp(x);
}

#endregion

#region [Mappers]

public interface IScaleMapper<D, R>
{
  R Map(D x);
  D Unmap(R x);
}

public class ScaleBiMapper : IScaleMapper<double, double>
{
  protected double _dMin, _dMax;
  protected double _rMin, _rMax;

  public IScaleMapper<double, double> Update(double[] domain, double[] range)
  {
    _dMin = domain[0]; _dMax = domain[1];
    _rMin = range[0]; _rMax = range[1];
    return this;
  }

  public double Map(double x)
    => _dMax < _dMin
      ? Lerp.To(_rMax, _rMin, Real.Normalize(x, _dMax, _dMin))
      : Lerp.To(_rMin, _rMax, Real.Normalize(x, _dMin, _dMax));

  public double Unmap(double x)
    => _dMax < _dMin
      ? Lerp.To(_dMax, _dMin, Real.Normalize(x, _rMax, _rMin))
      : Lerp.To(_dMin, _dMax, Real.Normalize(x, _rMin, _rMax));
}

public class ScaleBiMapper<R> : IScaleMapper<double, R>
{
  protected double _dMin, _dMax;
  protected R _rMin, _rMax;
  protected Func<R, R, double, R> _lerp;

  public IScaleMapper<double, R> Update(double[] domain, R[] range, Func<R, R, double, R> lerp)
  {
    _dMin = domain[0]; _dMax = domain[1];
    _rMin = range[0]; _rMax = range[1];
    _lerp = lerp;
    return this;
  }

  public R Map(double x)
    => _dMax < _dMin
      ? _lerp(_rMax, _rMin, Real.Normalize(x, _dMax, _dMin))
      : _lerp(_rMin, _rMax, Real.Normalize(x, _dMin, _dMax));

  public double Unmap(R x) => throw new NotSupportedException(
    "Unable to map a from a type range to a continuous domain");
}

public class ScalePolyMapper : IScaleMapper<double, double>
{
  public IScaleMapper<double, double> Update(double[] domain, double[] range)
  {
    throw new NotImplementedException();
  }

  public double Map(double x)
  {
    // TODO: see "polymap" in https://github.com/d3/d3-scale/blob/main/src/continuous.js
    throw new NotImplementedException();
  }

  public double Unmap(double x)
  {
    // TODO: see "polymap" in https://github.com/d3/d3-scale/blob/main/src/continuous.js
    throw new NotImplementedException();
  }
}

public class ScalePolyMapper<R> : IScaleMapper<double, R>
{
  public IScaleMapper<double, R> Update(double[] domain, R[] range, Func<R, R, double, R> lerp)
  {
    throw new NotImplementedException();
  }

  public R Map(double x)
  {
    // TODO: see "polymap" in https://github.com/d3/d3-scale/blob/main/src/continuous.js
    throw new NotImplementedException();
  }

  public double Unmap(R x) => throw new NotSupportedException(
    "Unable to map a from a type range to a continuous domain");
}

#endregion
