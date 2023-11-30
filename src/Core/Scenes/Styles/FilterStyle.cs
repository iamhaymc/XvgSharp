namespace Xvg;

public class FilterStyle : ISceneStyle
{
  public const bool DefaultShadowEnable = false;
  public const float DefaultShadowSigma = 0.01f;
  public const float DefaultShadowOpacity = 0.2f;
  public static readonly Vector2 DefaultShadowOffset = Vector2.Zero;

  public bool ShadowEnable { get; set; } = DefaultShadowEnable;
  public float ShadowSigma { get; set; } = DefaultShadowSigma;
  public float ShadowOpacity { get; set; } = DefaultShadowOpacity;
  public Vector2 ShadowOffset { get; set; } = DefaultShadowOffset;
}