namespace Xvg;

public static partial class ShadowStyle
{
  public static readonly Vector2 DefaultOffset = Vector2.Zero;
  public const float DefaultSigma = 0.01f;
  public const float DefaultOpacity = 0.2f;

  public const float L1OffsetFactor = 0.004f;
  public const float L1SigmaFactor = 0.004f;
  public const float L2OffsetFactor = 0.01f;
  public const float L2SigmaFactor = 0.01f;

  public static Vector2 ToL1Offset(Vector2 size)
    => new Vector2(0, Math.Min(size.X, size.Y) * L1OffsetFactor);
  public static float ToL1Sigma(Vector2 size)
    => Math.Min(size.X, size.Y) * L1SigmaFactor;
  public static Vector2 ToL2Offset(Vector2 size)
     => new Vector2(0, Math.Min(size.X, size.Y) * L2OffsetFactor);
  public static float ToL2Sigma(Vector2 size)
    => Math.Min(size.X, size.Y) * L2SigmaFactor;
}