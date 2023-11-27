﻿namespace Xvg;

public class GroupNode : SceneNode, 
  IAliasableNode<GroupNode>, IFilterableNode<GroupNode>
{
  public override SceneNodeType Type => SceneNodeType.Group;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Transform Transform { get; set; } = Transform.Identity;
  public string FilterId { get; set; } = null;

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

  public GroupNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public GroupNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}