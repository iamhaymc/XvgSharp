namespace Xvg;

public class FilterNode : SceneNode
{
  public override SceneNodeType Type => SceneNodeType.Filter;

  #region [Properties]

  public bool ShadowEnable { get; set; } = FilterStyle.DefaultShadowEnable;
  public float ShadowSigma { get; set; } = FilterStyle.DefaultShadowSigma;
  public float ShadowOpacity { get; set; } = FilterStyle.DefaultShadowOpacity;
  public Vector2 ShadowOffset { get; set; } = FilterStyle.DefaultShadowOffset;

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
