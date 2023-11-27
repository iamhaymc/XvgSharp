namespace Xvg;

public class FilterNode : SceneNode
{
  public override SceneNodeType Type => SceneNodeType.Filter;

  #region [Properties]

  public Vector2 ShadowOffset { get; set; } = ShadowStyle.DefaultOffset;
  public float ShadowSigma { get; set; } = ShadowStyle.DefaultSigma;
  public float ShadowOpacity { get; set; } = ShadowStyle.DefaultOpacity;

  #endregion

  #region [Edit]

  public SceneNode UseShadow(Vector2 offset, float sigma, float opacity)
  {
    ShadowOffset = offset;
    ShadowSigma = sigma;
    ShadowOpacity = opacity;
    return this;
  }

  #endregion
}