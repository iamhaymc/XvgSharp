namespace Xvg;

public class CopyNode : SceneNode, ITransformableNode<CopyNode>
{
  public override SceneNodeType Type => SceneNodeType.Copy;

  #region [Properties]

  public string ReferenceId { get; set; }

  public Transform Transform { get; set; } = Transform.Identity;

  #endregion

  #region [Edit]

  public CopyNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public CopyNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public CopyNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  #endregion
}