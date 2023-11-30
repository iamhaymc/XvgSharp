using System.Collections;

namespace Xvg;

public enum SceneNodeType
{
  View, Group, Path, Text, Image, Copy,
}

#region [Node Interfaces]

public interface ISceneNode : IEnumerable<ISceneNode>
{
  /// <summary>
  /// The type of the node
  /// </summary>
  SceneNodeType Type { get; }
  /// <summary>
  /// The unique ID of the node
  /// </summary>
  string Id { get; }

  /// <summary>
  /// Yields all nodes
  /// </summary>
  IEnumerable<ISceneNode> Enumerate();
}

public interface IAbstractableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// Determines whether the node is a definition of an instance
  /// </summary>
  bool Abstract { get; set; }

  /// <summary>
  /// Edits abstraction properties
  /// </summary>
  TNode UseAbstraction(bool truth);
}

public interface IAliasableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// Smooths visual aliasing
  /// </summary>
  bool AntiAlias { get; set; }

  /// <summary>
  /// Edits anti-aliasing properties
  /// </summary>
  TNode UseAntiAliasing(bool truth);
}

public interface ITransformableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// A translation, rotation, and scale to apply to the node
  /// </summary>
  Transform Transform { get; set; }

  /// <summary>
  /// Edits the translation
  /// </summary>
  TNode UseTranslation(Vector2 translation);
  /// <summary>
  /// Edits the rotation
  /// </summary>
  TNode UseRotation(float degrees);
  /// <summary>
  /// Edits the scale
  /// </summary>
  TNode UseScale(Vector2 scale);
}

public interface IFrameableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// The position and size of the node's rectangular boundary
  /// </summary>
  Box? Frame { get; set; }
  /// <summary>
  /// The method for scaling the node's content to its boundary
  /// </summary>
  BoxFitType? Fit { get; set; }

  /// <summary>
  /// Edits frame properties
  /// </summary>
  TNode UseFrame(Box frame);
  /// <summary>
  /// Edits fit properties
  /// </summary>
  TNode UseFit(BoxFitType fit);
}

public interface IFillableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// The inside color and related properties
  /// </summary>
  FillStyle Fill { get; set; }

  /// <summary>
  /// Edits fill properties
  /// </summary>
  TNode UseFill(IColor color = null, FillRuleType? rule = null);
}

public interface IStrokableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// The outline color and related properties
  /// </summary>
  StrokeStyle Stroke { get; set; }

  /// <summary>
  /// Edits stroke properties
  /// </summary>
  TNode UseStroke(IColor color = null, StrokeJointType? join = null, StrokeCapType? cap = null, float? width = null);
}

public interface IFilterableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// Visual post processing features
  /// </summary>
  public FilterStyle Filter { get; set; }

  /// <summary>
  /// Edits shadow properties
  /// </summary>
  public TNode UseShadow(Vector2? offset = null, float? sigma = null, float? opacity = null, bool enable = true);
}

public interface IClippable<TNode> : ISceneNode
  where TNode : ISceneNode
{
  /// <summary>
  /// A shape to subtract from the node's shape
  /// </summary>
  Path Clip { get; set; }

  /// <summary>
  /// Edits clip properties
  /// </summary>
  TNode UseClip(Path path);
}

#endregion

#region [Node Bases]

public abstract class SceneNode : ISceneNode
{
  /// <inheritdoc/>
  public abstract SceneNodeType Type { get; }
  /// <inheritdoc/>
  public string Id { get; set; }
  /// <inheritdoc/>
  public SceneNode(string id = null) => Id = id ?? Identifier.NextId;

  /// <inheritdoc/>
  public virtual IEnumerable<ISceneNode> Enumerate()
  {
    yield return this;
  }
  public IEnumerator<ISceneNode> GetEnumerator() => Enumerate().GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

public abstract class VisualNode : SceneNode, IAliasableNode<VisualNode>, IFilterableNode<VisualNode>
{
  /// <inheritdoc/>
  public bool AntiAlias { get; set; } = true;
  /// <inheritdoc/>
  public FilterStyle Filter { get; set; } = new FilterStyle();

  /// <inheritdoc/>
  public VisualNode UseAntiAliasing(bool truth)
  {
    AntiAlias = truth;
    return this;
  }
  /// <inheritdoc/>
  public VisualNode UseShadow(Vector2? offset = null, float? sigma = null, float? opacity = null, bool enable = true)
  {
    if (offset.HasValue)
      Filter.ShadowOffset = offset.Value;
    if (sigma.HasValue)
      Filter.ShadowSigma = sigma.Value;
    if (opacity.HasValue)
      Filter.ShadowOpacity = opacity.Value;
    Filter.ShadowEnable = enable;
    return this;
  }
}

public abstract class ShapeNode : VisualNode, IAbstractableNode<ShapeNode>, ITransformableNode<ShapeNode>, IClippable<ShapeNode>
{
  /// <inheritdoc/>
  public bool Abstract { get; set; } = false;
  /// <inheritdoc/>
  public Transform Transform { get; set; } = Transform.Identity;
  /// <inheritdoc/>
  public Path Clip { get; set; } = null;

  /// <inheritdoc/>
  public ShapeNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }
  /// <inheritdoc/>
  public ShapeNode UseTranslation(Vector2 translation)
  {
    Transform.Translation = translation;
    return this;
  }
  /// <inheritdoc/>
  public ShapeNode UseRotation(float degrees)
  {
    Transform.Rotation = degrees;
    return this;
  }
  /// <inheritdoc/>
  public ShapeNode UseScale(Vector2 scale)
  {
    Transform.Scale = scale;
    return this;
  }
  /// <inheritdoc/>
  public ShapeNode UseClip(Path clip)
  {
    Clip = clip;
    return this;
  }
}

public abstract class ContainerNode : ShapeNode
{
  /// <summary>
  /// Children of the node
  /// </summary>
  public List<ISceneNode> Nodes { get; set; } = new List<ISceneNode>();

  /// <inheritdoc/>
  public override IEnumerable<ISceneNode> Enumerate()
  {
    yield return this;
    foreach (ISceneNode child in Nodes)
      foreach (ISceneNode node in child)
        yield return node;
  }
}

public abstract class FillableShapeNode : ShapeNode, IFillableNode<FillableShapeNode>
{
  /// <inheritdoc/>
  public FillStyle Fill { get; set; } = new FillStyle();

  /// <inheritdoc/>
  public FillableShapeNode UseFill(IColor color = null, FillRuleType? rule = null)
  {
    if (color != null)
      Fill.Color = color;
    if (rule != null)
      Fill.Rule = rule;
    return this;
  }
}

public abstract class FillableContainerNode : ContainerNode, IFillableNode<FillableContainerNode>
{
  /// <inheritdoc/>
  public FillStyle Fill { get; set; } = new FillStyle();

  /// <inheritdoc/>
  public FillableContainerNode UseFill(IColor color = null, FillRuleType? rule = null)
  {
    if (color != null)
      Fill.Color = color;
    if (rule != null)
      Fill.Rule = rule;
    return this;
  }
}

public abstract class StrokableShapeNode : FillableShapeNode, IStrokableNode<StrokableShapeNode>
{
  /// <inheritdoc/>
  public StrokeStyle Stroke { get; set; } = new StrokeStyle();

  /// <inheritdoc/>
  public StrokableShapeNode UseStroke(IColor color = null, StrokeJointType? joint = null, StrokeCapType? cap = null, float? width = null)
  {
    if (color != null)
      Stroke.Color = color;
    if (joint != null)
      Stroke.Joint = joint;
    if (cap != null)
      Stroke.Cap = cap;
    if (width != null)
      Stroke.Width = width;
    return this;
  }
}

public abstract class StrokableContainerNode : FillableContainerNode, IStrokableNode<StrokableContainerNode>
{
  /// <inheritdoc/>
  public StrokeStyle Stroke { get; set; } = new StrokeStyle();

  /// <inheritdoc/>
  public StrokableContainerNode UseStroke(IColor color = null, StrokeJointType? joint = null, StrokeCapType? cap = null, float? width = null)
  {
    if (color != null)
      Stroke.Color = color;
    if (joint != null)
      Stroke.Joint = joint;
    if (cap != null)
      Stroke.Cap = cap;
    if (width != null)
      Stroke.Width = width;
    return this;
  }
}

#endregion