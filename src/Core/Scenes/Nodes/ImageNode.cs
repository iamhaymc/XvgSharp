namespace Xvg;

public class ImageNode : SceneNode, IShapeNode<ImageNode>, IFrameableNode<ImageNode>, IFilterableNode<ImageNode>
{
  public override SceneNodeType Type => SceneNodeType.Image;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public string Url { get; set; }
  public Box Frame { get; set; } = Box.Zero;
  public AspectType Aspect { get; set; } = AspectStyle.Default;
  public Transform Transform { get; set; } = Transform.Identity;
  public Vector2 ShadowOffset { get; set; } = ShadowStyle.DefaultOffset;
  public float ShadowSigma { get; set; } = ShadowStyle.DefaultSigma;
  public float ShadowOpacity { get; set; } = ShadowStyle.DefaultOpacity;
  public string FilterId { get; set; } = null;
  public bool AntiAlias { get; set; } = true;

  #endregion

  #region [Edit]

  public ImageNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ImageNode UseUrl(string url)
  {
    Url = url;
    return this;
  }

  public ImageNode UseFrame(Box frame)
  {
    Frame = frame;
    return this;
  }
  public ImageNode UseFrame(Vector2 position, Vector2 size)
    => UseFrame(Box.From(position, size));
  public ImageNode UseFrame(Vector2 size)
    => UseFrame(Box.FromSize(size));

  public ImageNode UseAspect(AspectType aspect)
  {
    Aspect = aspect;
    return this;
  }

  public ImageNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public ImageNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public ImageNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public ImageNode UseShadow(Vector2 offset, float sigma, float opacity)
  {
    throw new NotImplementedException();
  }

  public ImageNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public ImageNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}
