namespace Xvg;

public class GroupNode : SceneNode, ILayoutNode<GroupNode>, IShapeNode<GroupNode>
{
  public override SceneNodeType Type => SceneNodeType.Group;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public Transform Transform { get; set; } = Transform.Identity;
  public Vector2 ShadowOffset { get; set; } = ShadowStyle.DefaultOffset;
  public float ShadowSigma { get; set; } = ShadowStyle.DefaultSigma;
  public float ShadowOpacity { get; set; } = ShadowStyle.DefaultOpacity;
  public bool AntiAlias { get; set; } = true;

  #endregion

  #region [Edit]

  public GroupNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public GroupNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public GroupNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public GroupNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public GroupNode UseShadow(Vector2 offset, float sigma, float opacity)
  {
    throw new NotImplementedException();
  }

  public GroupNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}