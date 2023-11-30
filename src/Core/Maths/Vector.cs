namespace Xvg;

public struct Vector2 : IComparable<Vector2>
{
  public float X { get; set; }
  public float Y { get; set; }

  public Vector2(float x, float y)
  {
    X = x; Y = y;
  }

  public int CompareTo(Vector2 other)
  {
    // Compares lexigraphic order
    float d = X - other.X;
    return (d != 0) ? (int)d : (int)(Y - other.Y);
  }
  
  public override bool Equals(object o) => Eq((Vector2)o);

  public override int GetHashCode() => base.GetHashCode();

  public bool Eq(Vector2 v)
    => X == v.X && Y == v.Y;

  public bool Lt(Vector2 v)
    => CompareTo(v) < 0;

  public bool LtEq(Vector2 v)
    => CompareTo(v) < 1;

  public bool Gt(Vector2 v)
    => CompareTo(v) > 0;

  public bool GtEq(Vector2 v)
    => CompareTo(v) > -1;

  public Vector2 Add(float s)
    => new Vector2(X + s, Y + s);

  public Vector2 Add(Vector2 v)
    => new Vector2(X + v.X, Y + v.Y);

  public Vector2 Sub(float s)
    => new Vector2(X - s, Y - s);

  public Vector2 Sub(Vector2 v)
    => new Vector2(X - v.X, Y - v.Y);

  public Vector2 Mul(float s)
    => new Vector2(X * s, Y * s);

  public Vector2 Mul(Vector2 v)
    => new Vector2(X * v.X, Y * v.Y);

  public Vector2 Div(float s)
    => new Vector2(X / s, Y / s);

  public Vector2 Div(Vector2 v)
    => new Vector2(X / v.X, Y / v.Y);

  public Vector2 Pow(float e)
    => new Vector2((float)Math.Pow(X, e), (float)Math.Pow(Y, e));

  public Vector2 Min(Vector2 v)
    => new Vector2(Math.Min(X, v.X), Math.Min(Y, v.Y));

  public Vector2 Max(Vector2 v)
    => new Vector2(Math.Max(X, v.X), Math.Max(Y, v.Y));

  public Vector2 Neg()
    => new Vector2(-X, -Y);

  public Vector2 Exp()
    => new Vector2((float)Math.Exp(X), (float)Math.Exp(Y));

  public Vector2 Log()
    => new Vector2((float)Math.Log(X), (float)Math.Log(Y));

  public Vector2 Floor()
    => new Vector2((float)Math.Floor(X), (float)Math.Floor(Y));

  public Vector2 Ceil()
    => new Vector2((float)Math.Ceiling(X), (float)Math.Ceiling(Y));

  public Vector2 Round(int decimals = 0)
    => new Vector2((float)Math.Round(X, decimals), (float)Math.Round(Y, decimals));

  public Vector2 Clamp(float min, float max)
    => new Vector2(
      Math.Max(min, Math.Min(max, X)),
      Math.Max(min, Math.Min(max, Y)));

  public Vector2 Clamp(Vector2 min, Vector2 max)
    => new Vector2(
      Math.Max(min.X, Math.Min(max.X, X)),
      Math.Max(min.Y, Math.Min(max.Y, Y)));

  public float ToLength()
    => (float)Math.Sqrt(X * X + Y * Y);

  public Vector2 Normalize()
  {
    float length = ToLength();
    return Div(length != 0 ? length : 1f);
  }

  public Vector2 Resize(float length)
    => Normalize().Mul(length);

  public Vector2 Resize(float minLength, float maxLength)
  {
    float length = ToLength();
    return Div(length != 0 ? length : 1f)
      .Mul(Math.Max(minLength, Math.Min(maxLength, length)));
  }

  public float ToDistance(Vector2 v)
  {
    float dx = X - v.X, dy = Y - v.Y;
    return (float)Math.Sqrt(dx * dx + dy * dy);
  }

  public float ToManhattan(Vector2 v)
    => Math.Abs(X - v.X) - Math.Abs(Y - v.Y);

  public float Dot(Vector2 v)
    => X * v.X + Y * v.Y;

  public float Cross(Vector2 v)
    => X * v.Y - Y * v.X;

  public float ToAngle() // w.r.t x-axis
    => (float)Math.Atan2(-Y, -X) + (float)Math.PI;

  public Vector2 Rotate(Vector2 center, float angle)
  {
    float c = (float)Math.Cos(angle);
    float s = (float)Math.Sin(angle);
    float x = X - center.X;
    float y = Y - center.Y;
    return new Vector2(
      x * c - y * s + center.X,
      x * s + y * c + center.Y);
  }

  public Vector2 ToOrthogonal()
     => new Vector2(-Y, X);

  public Vector2 Lerp(Vector2 v, double t)
    => Mul(1 - (float)t).Add(v.Mul((float)t));

  public float[] ToArray()
    => new[] { X, Y };

  public override string ToString()
    => $"{X} {Y}";

  public static bool operator ==(Vector2 a, Vector2 b) => a.Eq(b);
  public static bool operator !=(Vector2 a, Vector2 b) => !a.Eq(b);
  public static bool operator <(Vector2 a, Vector2 b) => a.Lt(b);
  public static bool operator <=(Vector2 a, Vector2 b) => a.LtEq(b);
  public static bool operator >(Vector2 a, Vector2 b) => a.Gt(b);
  public static bool operator >=(Vector2 a, Vector2 b) => a.GtEq(b);
  public static Vector2 operator -(Vector2 a) => a.Neg();
  public static Vector2 operator +(Vector2 a, Vector2 b) => a.Add(b);
  public static Vector2 operator -(Vector2 a, Vector2 b) => a.Sub(b);
  public static Vector2 operator *(Vector2 a, float b) => a.Mul(b);
  public static Vector2 operator *(Vector2 a, Vector2 b) => a.Mul(b);
  public static Vector2 operator /(Vector2 a, float b) => a.Div(b);
  public static Vector2 operator /(Vector2 a, Vector2 b) => a.Div(b);

  public static Vector2 Min(Vector2 a, Vector2 b) => a.Min(b);
  public static Vector2 Max(Vector2 a, Vector2 b) => a.Max(b);

  public static Vector2 Zero => new Vector2(0, 0);
  public static Vector2 One => new Vector2(1, 1);
}