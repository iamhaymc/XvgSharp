namespace Xvg;

public class ViewNode : SceneNode, ILayoutNode<ViewNode>, IFrameableNode<ViewNode>, IFillableNode<ViewNode>
{
  public override SceneNodeType Type => SceneNodeType.View;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public Box Frame { get; set; } = Box.Zero;
  public Box ViewBox { get; set; } = Box.Zero;
  public AspectType Aspect { get; set; } = AspectStyle.Default;
  public Transform Transform { get; set; } = Transform.Identity;
  public ColorType FillColor { get; set; } = FillStyle.DefaultColor;
  public FillRuleType FillRule { get; set; } = FillStyle.DefaultRule;
  public bool AntiAlias { get; set; } = true;

  #endregion

  #region [Edit]

  public ViewNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public ViewNode UseFrame(Box frame)
  {
    Frame = frame;
    return this;
  }

  public ViewNode UseFrame(Vector2 position, Vector2 size)
    => UseFrame(Box.From(position, size));

  public ViewNode UseFrame(Vector2 size)
    => UseFrame(Box.FromSize(size));

  public ViewNode UseBox(Box box)
  {
    ViewBox = box;
    return this;
  }

  public ViewNode UseBox(Vector2 size, bool center)
   => UseBox(Box.From(
     center ? size.X / -2f : 0,
     center ? size.Y / -2f : 0, size.X, size.Y));

  public ViewNode UseBox(Vector2 position, Vector2 size)
     => UseBox(Box.From(position, size));

  public ViewNode UseBox(Vector2 size)
     => UseBox(Box.FromSize(size));

  public ViewNode UseAspect(AspectType aspect)
  {
    Aspect = aspect;
    return this;
  }

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

  public ViewNode UseFill(ColorType color, FillRuleType rule)
  {
    FillColor = color;
    FillRule = rule;
    return this;
  }

  public ViewNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}
