namespace Xvg;

public class PathNode : SceneNode,
  IAliasableNode<PathNode>, IFillableNode<PathNode>, IStrokableNode<PathNode>, IFilterableNode<PathNode>, IClippable<PathNode>
{
  public override SceneNodeType Type => SceneNodeType.Path;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Path Value { get; set; } = new Path();
  public Transform Transform { get; set; } = Transform.Identity;
  public FillStyle Fill { get; set; } = new FillStyle();
  public StrokeStyle Stroke { get; set; } = new StrokeStyle();
  public string FilterId { get; set; } = null;
  public string ClipPathId  { get; set; } = null;

  #endregion

  #region [Edit]

  public PathNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public PathNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  public PathNode UseValue(Action<Path> edit)
  {
    edit?.Invoke(Value);
    return this;
  }

  public PathNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public PathNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public PathNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public PathNode UseFill(IColor color = null, FillRuleType? rule = null)
  {
    Fill.Color = color;
    Fill.Rule = rule;
    return this;
  }

  public PathNode UseStroke(IColor color = null, StrokeJointType? joint = null, StrokeCapType? cap = null, float? width = null)
  {
    Stroke.Color = color;
    Stroke.Width = width;
    Stroke.Joint = joint;
    Stroke.Cap = cap;
    return this;
  }

  public PathNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public PathNode UseClipPath(string id)
  {
    ClipPathId = id;
    return this;
  }

  #endregion
}
