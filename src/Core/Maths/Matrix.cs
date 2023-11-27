namespace Xvg;

public struct Matrix3
{
  public float E11 { get; set; } // [0]
  public float E12 { get; set; } // [1]
  public float E13 { get; set; } // [2]
  public float E21 { get; set; } // [3]
  public float E22 { get; set; } // [4]
  public float E23 { get; set; } // [5]
  public float E31 { get; set; } // [6]
  public float E32 { get; set; } // [7]
  public float E33 { get; set; } // [8]

  public Matrix3(
    float e11, float e12, float e13,
    float e21, float e22, float e23,
    float e31, float e32, float e33)
  {
    E11 = e11; E12 = e12; E13 = e13;
    E21 = e21; E22 = e22; E23 = e23;
    E31 = e31; E32 = e32; E33 = e33;
  }

  public override bool Equals(object o) => Eq((Matrix3)o);
  public override int GetHashCode() => base.GetHashCode();

  public bool Eq(Matrix3 b)
    => E11 == b.E11 && E12 == b.E12 && E13 == b.E13
    && E21 == b.E21 && E22 == b.E22 && E23 == b.E23
    && E31 == b.E31 && E32 == b.E32 && E33 == b.E33;

  public Matrix3 Mul(Matrix3 b)
  {
    Matrix3 c = new Matrix3();
    c.E11 = E11 * b.E11 + E12 * b.E21 + E13 * b.E31;
    c.E21 = E11 * b.E12 + E12 * b.E22 + E13 * b.E32;
    c.E31 = E11 * b.E13 + E12 * b.E23 + E13 * b.E33;
    c.E12 = E21 * b.E11 + E22 * b.E21 + E23 * b.E31;
    c.E22 = E21 * b.E12 + E22 * b.E22 + E23 * b.E32;
    c.E32 = E21 * b.E13 + E22 * b.E23 + E23 * b.E33;
    c.E13 = E31 * b.E11 + E32 * b.E21 + E33 * b.E31;
    c.E23 = E31 * b.E12 + E32 * b.E22 + E33 * b.E32;
    c.E33 = E31 * b.E13 + E32 * b.E23 + E33 * b.E33;
    return c;
  }

  public Vector2 Mul(Vector2 b)
  {
    return new Vector2(
      E11 * b.X + E21 * b.Y + E31,
      E12 * b.X + E22 * b.Y + E32);
  }

  public Matrix3 Clone() => new Matrix3(
    E11, E12, E13,
    E21, E22, E23,
    E31, E32, E33);

  public override string ToString()
    => $"{E11} {E12} {E13} {E21} {E22} {E23} {E31} {E32} {E33}";

  public static Matrix3 operator *(Matrix3 a, Matrix3 b) => a.Mul(b);
  public static Vector2 operator *(Matrix3 a, Vector2 b) => a.Mul(b);

  public static Matrix3 OfTranslation(float x, float y)
  {
    return new Matrix3(
      1, 0, x,
      0, 1, y,
      0, 0, 1);
  }

  public static Matrix3 OfRotation(float radians)
  {
    float c = MathF.Cos(radians);
    float s = MathF.Sin(radians);
    return new Matrix3(
      c, -s, 0,
      s, c, 0,
      0, 0, 1);
  }

  public static Matrix3 OfScale(float x, float y)
  {
    return new Matrix3(
      x, 0, 0,
      0, y, 0,
      0, 0, 1);
  }

  public static readonly Matrix3 Zero = new Matrix3(
    0, 0, 0,
    0, 0, 0,
    0, 0, 0);

  public static readonly Matrix3 Identity = new Matrix3(
    1, 0, 0,
    0, 1, 0,
    0, 0, 1);
}
