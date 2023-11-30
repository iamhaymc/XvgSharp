using System.Collections;
using System.Diagnostics;

namespace Xvg;

public enum SceneNodeType
{
  Filter, View, Group, Path, Text, Image, Copy,
}

public interface ISceneNode : IEnumerable<ISceneNode>
{
  SceneNodeType Type { get; }
  string Id { get; }
  IEnumerable<ISceneNode> Enumerate();
}

public interface IAbstractableNode<TNode>
{
  bool Abstract { get; set; }
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
  Box? Frame { get; set; }
  TNode UseFrame(Box frame);
  TNode UseFrame(Vector2 position, Vector2 size);
  TNode UseFrame(Vector2 size);
  TNode UseFit(BoxFitType fit);
}

public interface IFillableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  FillStyle Fill { get; set; }
  TNode UseFill(IColor color = null, FillRuleType? rule = null);
}

public interface IStrokableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  StrokeStyle Stroke { get; set; }
  TNode UseStroke(IColor color = null, StrokeJointType? join = null, StrokeCapType? cap = null, float? width = null);
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
  #region [Properties]

  public abstract SceneNodeType Type { get; }
  public string Id { get; set; }

  #endregion

  public SceneNode EnsureId()
  {
    if (string.IsNullOrEmpty(Id))
      Id = Identifier.NextId;
    return this;
  }

  public IEnumerator<ISceneNode> GetEnumerator() => Enumerate().GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
  public virtual IEnumerable<ISceneNode> Enumerate()
  {
    yield return this;
  }
}

public abstract class ContainerNode : SceneNode,
    IAbstractableNode<ContainerNode>, IAliasableNode<ContainerNode>, ITransformableNode<ContainerNode>, 
    IFilterableNode<ContainerNode>, IClippable<ContainerNode>
{
  #region [Properties]

  public List<ISceneNode> Nodes { get; set; } = new List<ISceneNode>();

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Transform Transform { get; set; } = Transform.Identity;
  public string FilterId { get; set; } = null;
  public string ClipPathId { get; set; } = null;

  #endregion

  public override IEnumerable<ISceneNode> Enumerate()
  {
    yield return this;
    foreach (ISceneNode child in Nodes)
      foreach (ISceneNode node in child)
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
    AntiAlias = truth;
    return this;
  }

  public ContainerNode UseTranslation(Vector2 translation)
  {
    Transform.Translation = translation;
    return this;
  }

  public ContainerNode UseRotation(float degrees)
  {
    Transform.Rotation = degrees;
    return this;
  }

  public ContainerNode UseScale(Vector2 scale)
  {
    Transform.Scale = scale;
    return this;
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
    IAbstractableNode<ShapeNode>, IAliasableNode<ShapeNode>, ITransformableNode<ShapeNode>, 
    IFilterableNode<ShapeNode>, IClippable<ShapeNode>
{
  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Transform Transform { get; set; } = Transform.Identity;
  public string FilterId { get; set; } = null;
  public string ClipPathId { get; set; } = null;

  #endregion

  #region [Edit]

  public ShapeNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ShapeNode UseAntiAliasing(bool truth)
  {
    AntiAlias = truth;
    return this;
  }

  public ShapeNode UseTranslation(Vector2 translation)
  {
    Transform.Translation = translation;
    return this;
  }

  public ShapeNode UseRotation(float degrees)
  {
    Transform.Rotation = degrees;
    return this;
  }

  public ShapeNode UseScale(Vector2 scale)
  {
    Transform.Scale = scale;
    return this;
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