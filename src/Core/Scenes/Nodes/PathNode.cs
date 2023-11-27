namespace Xvg;

public class PathNode : SceneNode, IShapeNode<PathNode>, IFillableNode<PathNode>, IStrokableNode<PathNode>, IFilterableNode<PathNode>
{
  public override SceneNodeType Type => SceneNodeType.Path;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public VgPath Value { get; set; } = new VgPath();
  public Transform Transform { get; set; } = Transform.Identity;
  public ColorType FillColor { get; set; } = FillStyle.DefaultColor;
  public FillRuleType FillRule { get; set; } = FillStyle.DefaultRule;
  public ColorType StrokeColor { get; set; } = StrokeStyle.DefaultColor;
  public StrokeJointType StrokeJoint { get; set; } = StrokeStyle.DefaultJoint;
  public StrokeCapType StrokeCap { get; set; } = StrokeStyle.DefaultCap;
  public float StrokeWidth { get; set; } = StrokeStyle.DefaultWidth;
  public Vector2 ShadowOffset { get; set; } = ShadowStyle.DefaultOffset;
  public float ShadowSigma { get; set; } = ShadowStyle.DefaultSigma;
  public float ShadowOpacity { get; set; } = ShadowStyle.DefaultOpacity;
  public string FilterId { get; set; } = null;
  public bool AntiAlias { get; set; } = true;

  #endregion

  #region [Edit]

  public PathNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public PathNode UseValue(Action<VgPath> edit)
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

  public PathNode UseFill(ColorType color, FillRuleType rule)
  {
    FillColor = color;
    FillRule = rule;
    return this;
  }

  public PathNode UseStroke(ColorType color, StrokeJointType joint, StrokeCapType cap, float width)
  {
    StrokeColor = color;
    StrokeWidth = width;
    StrokeJoint = joint;
    StrokeCap = cap;
    return this;
  }

  public PathNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public PathNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}
