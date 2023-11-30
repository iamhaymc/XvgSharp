namespace Xvg;

public class ViewNode : ContainerNode,
  IFrameableNode<ViewNode>, IFillableNode<ViewNode>
{
  public override SceneNodeType Type => SceneNodeType.View;

  #region [Properties]

  public Box? Frame { get; set; } = null;
  public Box? Box { get; set; } = null;
  public BoxFitType Fit { get; set; } = FitStyle.Default;
  public FillStyle Fill { get; set; } = new FillStyle();

  #endregion

  #region [Edit]


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

  public ViewNode UseFill(IColor color = null, FillRuleType? rule = null)
  {
    Fill.Color = color;
    Fill.Rule = rule;
    return this;
  }

  #endregion
}
