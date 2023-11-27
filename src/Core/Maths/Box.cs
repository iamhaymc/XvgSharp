namespace Xvg;
public struct Box
{
  public Vector2 Min { get; set; }
  public Vector2 Max { get; set; }

  public Box(Vector2 min, Vector2 max)
  {
    Min = min;
    Max = max;
  }

  public float X => Min.X;
  public float Y => Min.Y;
  public float Width => Max.X - Min.X;
  public float HalfWidth => Width / 2f;
  public float Height => Max.Y - Min.Y;
  public float HalfHeight => Height / 2f;

  public float Left => Min.X;
  public float Top => Min.Y;
  public float Center => Left + HalfWidth;
  public float Middle => Top + HalfHeight;
  public float Right => Left + Width;
  public float Bottom => Top + Height;

  public Vector2 Size => Max - Min;
  public Vector2 HalfSize => Size / 2f;
  public Vector2 Centroid => Min + HalfSize;

  public Box Clone() => new Box(Min, Max);

  public Box Resize(Vector2 size)
  {
    Max = Min + size;
    return this;
  }
  public Box Translate(Vector2 translation)
  {
    Min += translation;
    Max += translation;
    return this;
  }
  public Box Scale(Vector2 scale)
  {
    Min *= scale;
    Max *= scale;
    return this;
  }

  public bool Contains(Vector2 point)
  {
    return !(point.X < Min.X || point.X > Max.X
          || point.Y < Min.Y || point.Y > Max.Y);
  }
  public bool Contains(Box box)
  {
    return Min.X <= box.Min.X && box.Max.X <= Max.X
        && Min.Y <= box.Min.Y && box.Max.Y <= Max.Y;
  }
  public bool Intersects(Box box)
  {
    return !(box.Max.X < Min.X || box.Min.X > Max.X
          || box.Max.Y < Min.Y || box.Min.Y > Max.Y);
  }

  public Box Intersect(Box box)
  {
    return new Box(Vector2.Max(Min, box.Min), Vector2.Min(Max, box.Max));
  }
  public Box Union(Box box)
  {
    return new Box(Vector2.Min(Min, box.Min), Vector2.Max(Max, box.Max));
  }

  public static Box Zero
    => new Box(Vector2.Zero, Vector2.Zero);

  public static Box From(Vector2 position, Vector2 size)
    => new Box(position, position + size);
  public static Box From(float x, float y, float w, float h)
    => From(new Vector2(x, y), new Vector2(w, h));

  public static Box FromPosition(Vector2 position)
    => From(position, Vector2.Zero);
  public static Box FromPosition(float x, float y)
    => From(x, y, 0, 0);

  public static Box FromSize(Vector2 size)
    => From(Vector2.Zero, size);
  public static Box FromSize(float w, float h)
    => From(0, 0, w, h);

  /// <summary>
  /// Creates a view box that fits in a frame while considering the aspect ratio
  /// </summary>
  public static Box FromFit(Box view, Box frame, AspectType? aspect = null)
  {
    switch (aspect)
    {
      default:
      case AspectType.XMidYMidMeet:
        return FromFitMeet(view, frame);
      case AspectType.XMidYMidSlice:
        return FromFitMeet(view, frame);
      case AspectType.None:
        return From(frame.Min, frame.Size);
    }
  }

  /// <summary>
  /// Creates a view box that fits in a frame while considering the aspect ratio
  /// </summary>
  public static Box FromFitMeet(Box view, Box frame)
  {
    float scale = Math.Min(frame.Width / view.Width, frame.Height / view.Height);
    Vector2 position = (frame.Min) + (view.Min * scale)
             + (frame.HalfSize) - (view.HalfSize * scale);
    Vector2 size = view.Size * scale;
    return From(position, size);
  }

  /// <summary>
  /// Creates a view box that fits in a frame while considering the aspect ratio
  /// </summary>
  public static Box FromFitSlice(Box view, Box frame)
  {
    float scale = Math.Max(frame.Width / view.Width, frame.Height / view.Height);
    Vector2 position = (frame.Min) + (view.Min * scale)
             + (frame.HalfSize) - (view.HalfSize * scale);
    Vector2 size = view.Size * scale;
    return From(position, size);
  }
}
