namespace Xvg;

public class CopyNode : SceneNode,
  ITransformableNode<CopyNode>, IFilterableNode<CopyNode>, IClippable<CopyNode>
{
  public override SceneNodeType Type => SceneNodeType.Copy;

  #region [Properties]

  public string ReferenceId { get; set; }

  public Transform Transform { get; set; } = Transform.Identity;

  public string FilterId { get; set; }

  public string ClipPathId { get; set; } = null;

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

  public CopyNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public CopyNode UseClipPath(string id)
  {
    ClipPathId = id;
    return this;
  }

  #endregion
}
