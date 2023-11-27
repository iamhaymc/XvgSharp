namespace Xvg;

public enum SceneNodeType
{
  View, Group, Path, Text, Image, Copy
}

public interface ISceneNode
{
  SceneNodeType Type { get; }
  string Id { get; }
  List<ISceneNode> Nodes { get; }
  IEnumerable<ISceneNode> YieldNodes();
}

public abstract class SceneNode : ISceneNode
{
  public abstract SceneNodeType Type { get; }
  public string Id { get; set; }
  public List<ISceneNode> Nodes { get; set; } = new List<ISceneNode>();

  public SceneNode EnsureId()
  {
    if (string.IsNullOrEmpty(Id))
      Id = Identifier.NextId;
    return this;
  }

  public IEnumerable<ISceneNode> YieldNodes()
  {
    yield return this;
    foreach (ISceneNode child in Nodes)
      foreach (ISceneNode node in child.YieldNodes())
        yield return node;
  }
}

public interface IAbstractableNode<TNode>
{
  public bool Abstract { get; set; }
  TNode UseAbstraction(bool truth);
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
  public TNode UseFrame(Box frame);
  public TNode UseFrame(Vector2 position, Vector2 size);
  public TNode UseFrame(Vector2 size);
}

public interface IFillableNode<TNode>: ISceneNode
  where TNode : ISceneNode
{
  ColorType FillColor { get; set; }
  FillRuleType FillRule { get; set; }
  TNode UseFill(ColorType color, FillRuleType rule);
}

public interface IStrokableNode<TNode> : ISceneNode
  where TNode : ISceneNode
{
  ColorType StrokeColor { get; set; }
  StrokeJointType StrokeJoint { get; set; }
  StrokeCapType StrokeCap { get; set; }
  float StrokeWidth { get; set; }
  TNode UseStroke(ColorType color, StrokeJointType join, StrokeCapType cap, float width);
}

public interface ILayoutNode<TNode> : ITransformableNode<TNode>, IAbstractableNode<TNode>
  where TNode : ISceneNode
{
  bool AntiAlias { get; set; }
  TNode UseAntiAliasing(bool truth);
}

public interface IShapeNode<TNode> : ITransformableNode<TNode>, IAbstractableNode<TNode>
  where TNode : ISceneNode
{
  Vector2 ShadowOffset { get; set; }
  float ShadowSigma { get; set; }
  float ShadowOpacity { get; set; }
  bool AntiAlias { get; set; }
  TNode UseShadow(Vector2 offset, float sigma, float opacity);
  TNode UseAntiAliasing(bool truth);
}