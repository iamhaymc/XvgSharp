namespace Xvg;

public struct Transform
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

  public static Transform Identity => new Transform(Vector2.Zero, 0, Vector2.One);

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

  /// <summary>
  /// Creates a transform that can project from view space to frame space
  /// </summary>
  public static Transform ToProjector(Transform frame, Box view, AspectType? aspect = null)
    => NestProjector(Identity, frame, FromBox(view), aspect);
  public static Transform ToProjector(Transform frame, Transform view, AspectType? aspect = null)
    => NestProjector(Identity, frame, view, aspect);

  /// <summary>
  /// Updates a transform to project from nested view space to frame space
  /// </summary>
  public static Transform NestProjector(Transform projector, Transform frame, Box view, AspectType? aspect = null)
    => NestProjector(projector, frame, FromBox(view), aspect);
  public static Transform NestProjector(Transform projector, Transform frame, Transform view, AspectType? aspect = null)
  {
    Transform result = projector.Clone();
    switch (aspect)
    {
      default:
      case AspectType.XMidYMidMeet:
        result.Scale *= Math.Min(frame.Scale.X / view.Scale.X, frame.Scale.Y / view.Scale.Y);
        result.Translation +=
          (frame.Translation * projector.Scale) + (view.Translation * result.Scale)
          + (frame.Scale * projector.Scale / 2f) - (view.Scale * result.Scale / 2f);
        break;
      case AspectType.XMidYMidSlice:
        result.Scale *= Math.Max(frame.Scale.X / view.Scale.X, frame.Scale.Y / view.Scale.Y);
        result.Translation +=
          (frame.Translation * projector.Scale) + (view.Translation * result.Scale)
          + (frame.Scale * projector.Scale / 2f) - (view.Scale * result.Scale / 2f);
        break;
      case AspectType.None:
        result.Scale *= frame.Scale / view.Scale;
        result.Translation += (frame.Translation * projector.Scale) + (view.Translation * result.Scale);
        break;
    }
    result.Rotation += frame.Rotation + view.Rotation;
    return result;
  }
}

public static class TransformNumericsExtensions
{
  public static System.Numerics.Matrix3x2 ToTranslation(this Transform self)
    => System.Numerics.Matrix3x2.CreateTranslation(self.Translation.X, self.Translation.Y);
  public static System.Numerics.Matrix3x2 ToRotation(this Transform self, bool degs2rads = false)
    => System.Numerics.Matrix3x2.CreateRotation(degs2rads ? Radial.ToRadians(self.Rotation) : self.Rotation);
  public static System.Numerics.Matrix3x2 ToScale(this Transform self)
    => System.Numerics.Matrix3x2.CreateScale(self.Scale.X, self.Scale.Y);
}
