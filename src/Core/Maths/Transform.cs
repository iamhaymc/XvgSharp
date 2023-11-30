namespace Xvg;

public class Transform
{
  public Vector2 Translation { get; set; }
  public float Rotation { get; set; }
  public Vector2 Scale { get; set; }

  public Transform(Vector2 translation, float rotation, Vector2 scale)
  {
    Translation = translation;
    Rotation = rotation;
    Scale = scale;
  }

  public Transform Clone() => new Transform(Translation, Rotation, Scale);

  public Transform WithTranslation(Vector2 translation)
    => new Transform(translation, Rotation, Scale);
  public Transform WithRotation(float rotation)
    => new Transform(Translation, rotation, Scale);
  public Transform WithScale(Vector2 scale)
    => new Transform(Translation, Rotation, scale);

  public static Transform From(Vector2 translation, float rotation, Vector2 scale)
    => new Transform(translation, rotation, scale);
  public static Transform From(float x, float y, float r, float sx, float sy)
    => new Transform(new Vector2(x, y), r, new Vector2(sx, sy));
  public static Transform FromTranslation(Vector2 translation)
    => new Transform(translation, 0, Vector2.One);
  public static Transform FromTranslation(float x, float y)
    => FromTranslation(new Vector2(x, y));
  public static Transform FromRotation(float rotation)
    => new Transform(Vector2.Zero, rotation, Vector2.One);
  public static Transform FromScale(Vector2 scale)
    => new Transform(Vector2.Zero, 0, scale);
  public static Transform FromScale(float x, float y)
    => FromScale(new Vector2(x, y));
  public static Transform FromBox(Box box)
    => new Transform(box.Min, 0f, box.Size);

  public static Transform Zero => new Transform(Vector2.Zero, 0, Vector2.Zero);
  public static Transform Identity => new Transform(Vector2.Zero, 0, Vector2.One);
}
