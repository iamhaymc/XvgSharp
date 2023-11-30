namespace Xvg;

public class ViewNode : SceneNode,
  IAliasableNode<ViewNode>, IFrameableNode<ViewNode>, IFillableNode<ViewNode>, IFilterableNode<ViewNode>, IClippable<ViewNode>
{
  public override SceneNodeType Type => SceneNodeType.View;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public Box? Frame { get; set; } = null;
  public Box? Box { get; set; } = null;
  public BoxFitType Fit { get; set; } = FitStyle.Default;
  public Transform Transform { get; set; } = Transform.Identity;
  public ColorKind? FillColor { get; set; } = null;
  public FillRuleType? FillRule { get; set; } = null;
  public string FilterId { get; set; } = null;
  public string ClipPathId { get; set; } = null;

  #endregion

  #region [Edit]

  public ViewNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ViewNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  public ViewNode UseFrame(Box frame)
  {
    Frame = frame;
    return this;
  }

  public ViewNode UseFrame(Vector2 position, Vector2 size)
    => UseFrame(Xvg.Box.From(position, size));

  public ViewNode UseFrame(Vector2 size)
    => UseFrame(Xvg.Box.FromSize(size));

  public ViewNode UseFit(BoxFitType fix)
  {
    Fit = fix;
    return this;
  }

  public ViewNode UseViewBox(Box box)
  {
    Box = box;
    return this;
  }

  public ViewNode UseViewBox(Vector2 size, bool center)
   => UseViewBox(Xvg.Box.From(
     center ? size.X / -2f : 0,
     center ? size.Y / -2f : 0, size.X, size.Y));

  public ViewNode UseViewBox(Vector2 position, Vector2 size)
     => UseViewBox(Xvg.Box.From(position, size));

  public ViewNode UseViewBox(Vector2 size)
     => UseViewBox(Xvg.Box.FromSize(size));

  public ViewNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public ViewNode UseRotation(float degrees)
  {
    throw new NotImplementedException();
  }

  public ViewNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public ViewNode UseFill(ColorKind color, FillRuleType rule)
  {
    FillColor = color;
    FillRule = rule;
    return this;
  }

  public ViewNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public ViewNode UseClipPath(string id)
  {
    ClipPathId = id;
    return this;
  }

  #endregion
}
