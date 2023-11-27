namespace Xvg;

public enum VgPathCurveType
{
  Linear, StepBefore, StepAfter, Basis, Cardinal, CatmullRom
}

public interface IVgPathCurvePather
{
  IVgPathCurvePather UsePath(VgPath path);
  VgPath AppendFigure(IList<Vector2> points);
  VgPath ExtendFigure(IList<Vector2> points);
}

public abstract class VgBaseCurvePather : IVgPathCurvePather
{
  protected VgPath _path;
  protected int _point;

  public IVgPathCurvePather UsePath(VgPath path)
  {
    _path = path;
    return this;
  }

  public VgPath AppendFigure(IList<Vector2> points)
  {
    return GenerateSteps(points, started: false);
  }

  public VgPath ExtendFigure(IList<Vector2> points)
  {
    return GenerateSteps(points, started: true);
  }

  private VgPath GenerateSteps(IList<Vector2> points, bool started)
  {
    bool defined = false;
    for (int i = 0; i <= points.Count; ++i)
    {
      if (!(i < points.Count) == defined)
      {
        if (defined = !defined)
          LineStart(started);
        else
          LineEnd();
      }
      if (defined)
        Point(points[i]);
    }
    return _path;
  }

  protected virtual void Reset()
  {
    _point = 0;
  }

  protected virtual void LineStart(bool started)
  {
    _point = started ? 1 : 0;
  }

  protected virtual void LineEnd()
  {
    Reset();
  }

  protected abstract void Point(Vector2 point);
}

public class VgLinearCurvePather : VgBaseCurvePather
{
  protected override void Point(Vector2 point)
  {
    switch (_point)
    {
      case 0:
        _point = 1;
        _path.MoveTo(point);
        break;
      case 1:
        _point = 2;
        _path.LineTo(point);
        break;
      default:
        _path.LineTo(point);
        break;
    }
  }
}

public class VgStepCurvePather : VgBaseCurvePather
{
  private readonly float _t;
  private float _x, _y;

  public VgStepCurvePather(float t = 0.5f)
  {
    _t = t;
  }

  protected override void Reset()
  {
    _x = _y = float.NaN;
    _point = 0;
  }

  protected override void LineEnd()
  {
    if (0 < _t && _t < 1 && _point == 2)
      _path.LineTo(_x, _y);
    Reset();
  }

  protected override void Point(Vector2 point)
  {
    switch (_point)
    {
      case 0:
        _point = 1;
        _path.MoveTo(point);
        break;
      case 1:
        _point = 2;
        if (_t <= 0)
        {
          _path.LineTo(_x, point.Y);
          _path.LineTo(point);
        }
        else
        {
          float x1 = _x * (1 - _t) + point.X * _t;
          _path.LineTo(x1, _y);
          _path.LineTo(x1, point.Y);
        }
        break;
      default:
        if (_t <= 0)
        {
          _path.LineTo(_x, point.Y);
          _path.LineTo(point);
        }
        else
        {
          float x1 = _x * (1 - _t) + point.X * _t;
          _path.LineTo(x1, _y);
          _path.LineTo(x1, point.Y);
        }
        break;
    }
    _x = point.X;
    _y = point.Y;
  }
}

public class VgStepBeforeCurvePather : VgStepCurvePather
{
  public VgStepBeforeCurvePather()
       : base(t: 0.0f)
  { }
}

public class VgStepAfterCurvePather : VgStepCurvePather
{
  public VgStepAfterCurvePather()
       : base(t: 1.0f)
  { }
}

public class VgBasisCurvePather : VgBaseCurvePather
{
  private float _x0, _x1;
  private float _y0, _y1;

  protected override void Reset()
  {
    _x0 = _x1 =
    _y0 = _y1 = float.NaN;
    _point = 0;
  }

  protected override void LineEnd()
  {
    switch (_point)
    {
      case 2:
        _path.LineTo(new Vector2(_x1, _y1));
        break;
      case 3:
        _Point(new Vector2(_x1, _y1));
        _path.LineTo(new Vector2(_x1, _y1));
        break;
    }
    Reset();
  }

  protected override void Point(Vector2 point)
  {
    switch (_point)
    {
      case 0:
        _point = 1;
        _path.MoveTo(point);
        break;
      case 1:
        _point = 2;
        break;
      case 2:
        _point = 3;
        _path.LineTo((5f * _x0 + _x1) / 6f, (5 * _y0 + _y1) / 6f);
        _Point(point);
        break;
      default:
        _Point(point);
        break;
    }
    _x0 = _x1;
    _x1 = point.X;
    _y0 = _y1;
    _y1 = point.Y;
  }

  private void _Point(Vector2 point)
  {
    _path.Bezier3To(
      (2f * _x0 + _x1) / 3f,
      (2f * _y0 + _y1) / 3f,
      (_x0 + 2f * _x1) / 3f,
      (_y0 + 2f * _y1) / 3f,
      (_x0 + 4f * _x1 + point.X) / 6f,
      (_y0 + 4f * _y1 + point.Y) / 6f
    );
  }
}

public class VgCardinalCurvePather : VgBaseCurvePather
{
  private readonly float _k;
  private float _x0, _x1, _x2;
  private float _y0, _y1, _y2;

  public VgCardinalCurvePather(float tension = 0f)
  {
    _k = (1f - tension) / 6f;
  }

  protected override void Reset()
  {
    _x0 = _x1 = _x2 =
    _y0 = _y1 = _y2 = float.NaN;
    _point = 0;
  }

  protected override void LineEnd()
  {
    switch (_point)
    {
      case 2:
        _path.LineTo(_x2, _y2);
        break;
      case 3:
        Point(new Vector2(_x1, _y1));
        break;
    }
    Reset();
  }

  protected override void Point(Vector2 point)
  {
    switch (_point)
    {
      case 0:
        _point = 1;
        _path.MoveTo(point);
        break;
      case 1:
        _point = 2;
        _x1 = point.X;
        _y1 = point.Y;
        break;
      case 2:
        _point = 3;
        _Point(point);
        break;
      default:
        _Point(point);
        break;
    }
    _x0 = _x1;
    _x1 = _x2;
    _x2 = point.X;
    _y0 = _y1;
    _y1 = _y2;
    _y2 = point.Y;
  }

  private void _Point(Vector2 point)
  {
    _path.Bezier3To(
        _x1 + _k * (_x2 - _x0),
        _y1 + _k * (_y2 - _y0),
        _x2 + _k * (_x1 - point.X),
        _y2 + _k * (_y1 - point.Y),
        _x2,
        _y2);
  }
}

public class VgCatmullRomCurvePather : VgBaseCurvePather
{
  private readonly float _alpha;
  private float _x0, _x1, _x2;
  private float _y0, _y1, _y2;
  private float _l01_a, _l12_a, _l23_a;
  private float _l01_2a, _l12_2a, _l23_2a;

  public VgCatmullRomCurvePather(float alpha = 0.5f)
  {
    _alpha = alpha;
  }

  protected override void Reset()
  {
    _x0 = _x1 = _x2 =
    _y0 = _y1 = _y2 = float.NaN;
    _l01_a = _l12_a = _l23_a =
    _l01_2a = _l12_2a = _l23_2a = 0;
    _point = 0;
  }

  protected override void LineEnd()
  {
    switch (_point)
    {
      case 2:
        _path.LineTo(_x2, _y2);
        break;
      case 3:
        Point(new Vector2(_x2, _y2));
        break;
    }
    Reset();
  }

  protected override void Point(Vector2 point)
  {
    if (_point > 0)
    {
      float x23 = _x2 - point.X;
      float y23 = _y2 - point.Y;
      _l23_2a = (float)Math.Pow(x23 * x23 + y23 * y23, _alpha);
      _l23_a = (float)Math.Sqrt(_l23_2a);
    }
    switch (_point)
    {
      case 0:
        _point = 1;
        _path.MoveTo(point);
        break;
      case 1:
        _point = 2;
        break;
      case 2:
        _point = 3;
        _Point(point);
        break;
      default:
        _Point(point);
        break;
    }
    _l01_a = _l12_a;
    _l12_a = _l23_a;
    _l01_2a = _l12_2a;
    _l12_2a = _l23_2a;
    _x0 = _x1;
    _x1 = _x2;
    _x2 = point.X;
    _y0 = _y1;
    _y1 = _y2;
    _y2 = point.Y;
  }

  private const float _epsilon = 1e-12f;
  private void _Point(Vector2 point)
  {
    float x1 = _x1, y1 = _y1, x2 = _x2, y2 = _y2;
    if (_l01_a > _epsilon)
    {
      float a = 2 * _l01_2a + 3 * _l01_a * _l12_a + _l12_2a,
           n = 3 * _l01_a * (_l01_a + _l12_a);
      x1 = (x1 * a - _x0 * _l12_2a + _x2 * _l01_2a) / n;
      y1 = (y1 * a - _y0 * _l12_2a + _y2 * _l01_2a) / n;
    }
    if (_l23_a > _epsilon)
    {
      float b = 2 * _l23_2a + 3 * _l23_a * _l12_a + _l12_2a,
           m = 3 * _l23_a * (_l23_a + _l12_a);
      x2 = (x2 * b + _x1 * _l23_2a - point.X * _l12_2a) / m;
      y2 = (y2 * b + _y1 * _l23_2a - point.Y * _l12_2a) / m;
    }
    _path.Bezier3To(x1, y1, x2, y2, _x2, _y2);
  }
}

public static class VgPathCurveExtensions
{
  public static VgPath CurveTo(this VgPath self, IVgPathCurvePather pather, params Vector2[] points)
  {
    return pather.UsePath(self).ExtendFigure(points);
  }
  public static VgPath AddCurve(this VgPath self, IVgPathCurvePather pather, params Vector2[] points)
  {
    return pather.UsePath(self).AppendFigure(points);
  }
  public static VgPath AddCurve(this VgPath self, VgPathCurveType type, params Vector2[] points)
  {
    switch (type)
    {
      default:
      case VgPathCurveType.Linear: return self.AddLinearCurve(points);
      case VgPathCurveType.StepBefore: return self.AddStepBeforeCurve(points);
      case VgPathCurveType.StepAfter: return self.AddStepAfterCurve(points);
      case VgPathCurveType.Basis: return self.AddBasisCurve(points);
      case VgPathCurveType.Cardinal: return self.AddCardinalCurve(points);
      case VgPathCurveType.CatmullRom: return self.AddCatmullRomCurve(points);
    }
  }

  public static VgPath LinearCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgLinearCurvePather(), points);
  }
  public static VgPath AddLinearCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgLinearCurvePather(), points);
  }

  public static VgPath StepCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgStepCurvePather(), points);
  }
  public static VgPath AddStepCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgStepCurvePather(), points);
  }

  public static VgPath StepBeforeCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgStepBeforeCurvePather(), points);
  }
  public static VgPath AddStepBeforeCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgStepBeforeCurvePather(), points);
  }

  public static VgPath StepAfterCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgStepAfterCurvePather(), points);
  }
  public static VgPath AddStepAfterCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgStepAfterCurvePather(), points);
  }

  public static VgPath BasisCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgBasisCurvePather(), points);
  }
  public static VgPath AddBasisCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgBasisCurvePather(), points);
  }

  public static VgPath CardinalCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgCardinalCurvePather(), points);
  }
  public static VgPath AddCardinalCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgCardinalCurvePather(), points);
  }

  public static VgPath CatmullRomCurveTo(this VgPath self, params Vector2[] points)
  {
    return self.CurveTo(new VgCatmullRomCurvePather(), points);
  }
  public static VgPath AddCatmullRomCurve(this VgPath self, params Vector2[] points)
  {
    return self.AddCurve(new VgCatmullRomCurvePather(), points);
  }
}
