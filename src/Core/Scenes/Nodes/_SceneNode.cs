namespace Xvg;

public enum SceneNodeType
{
  Filter, View, Group, Path, Text, Image, Copy,
}

public interface ISceneNode
{
  SceneNodeType Type { get; }
  string Id { get; }
  IEnumerable<ISceneNode> YieldNodes();
}

public interface IAbstractableNode<TNode>
{
  public bool Abstract { get; set; }
  TNode UseAbstraction(bool truth);
}

public interface IAliasableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  bool AntiAlias { get; set; }
  TNode UseAntiAliasing(bool truth);
}

public interface ITransformableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  Transform Transform { get; set; }
  TNode UseTranslation(Vector2 translation);
  TNode UseRotation(float degrees);
  TNode UseScale(Vector2 scale);
}

public interface IFrameableNode<TNode> : ISceneNode
{
  Box Frame { get; set; }
  TNode UseFrame(Box frame);
  TNode UseFrame(Vector2 position, Vector2 size);
  TNode UseFrame(Vector2 size);
  TNode UseFit(BoxFitType fit);
}

public interface IFillableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  ColorKind FillColor { get; set; }
  FillRuleType FillRule { get; set; }
  TNode UseFill(ColorKind color, FillRuleType rule);
}

public interface IStrokableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  ColorKind StrokeColor { get; set; }
  StrokeJointType StrokeJoint { get; set; }
  StrokeCapType StrokeCap { get; set; }
  float StrokeWidth { get; set; }
  TNode UseStroke(ColorKind color, StrokeJointType join, StrokeCapType cap, float width);
}

public interface IFilterableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  string FilterId { get; set; }
  TNode UseFilter(string id);
}

public interface IClippable<TNode> : ISceneNode
  where TNode : ISceneNode
{
  string ClipPathId { get; set; }
  TNode UseClipPath(string id);
}

public abstract class SceneNode : ISceneNode
{
  public abstract SceneNodeType Type { get; }
  public string Id { get; set; }

  public SceneNode EnsureId()
  {
    if (string.IsNullOrEmpty(Id))
      Id = Identifier.NextId;
    return this;
  }

  public abstract IEnumerable<ISceneNode> YieldNodes();
}

public abstract class ContainerNode : SceneNode,
    IAbstractableNode<ContainerNode>, IAliasableNode<ContainerNode>, ITransformableNode<ContainerNode>, IFrameableNode<ContainerNode>, IFillableNode<ContainerNode>, IFilterableNode<ContainerNode>, IClippable<ContainerNode>
{
  #region [Properties]

  public List<ISceneNode> Nodes { get; set; } = new List<ISceneNode>();

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Transform Transform { get; set; } = Transform.Identity;
  public string FilterId { get; set; } = null;
  public string ClipPathId { get; set; } = null;

  #endregion


  public override IEnumerable<ISceneNode> YieldNodes()
  {
    yield return this;
    foreach (ISceneNode child in Nodes)
      foreach (ISceneNode node in child.YieldNodes())
        yield return node;
  }

  #region [Edit]

  public ContainerNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ContainerNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  public ContainerNode UseValue(Action<Path> edit)
  {
    edit?.Invoke(Value);
    return this;
  }

  public ContainerNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public ContainerNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public ContainerNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public ContainerNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public ContainerNode UseClipPath(string id)
  {
    ClipPathId = id;
    return this;
  }

  #endregion
}

public abstract class ShapeNode : SceneNode,
    IAbstractableNode<ShapeNode>, IAliasableNode<ShapeNode>, ITransformableNode<ShapeNode>, IFilterableNode<ShapeNode>, IClippable<ShapeNode>
{
  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Transform Transform { get; set; } = Transform.Identity;
  public string FilterId { get; set; } = null;
  public string ClipPathId { get; set; } = null;

  #endregion

  public override IEnumerable<ISceneNode> YieldNodes()
  {
    yield return this;
  }

  #region [Edit]

  public ShapeNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ShapeNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  public ShapeNode UseValue(Action<Path> edit)
  {
    edit?.Invoke(Value);
    return this;
  }

  public ShapeNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public ShapeNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public ShapeNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public ShapeNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public ShapeNode UseClipPath(string id)
  {
    ClipPathId = id;
    return this;
  }

  #endregion
}