namespace Xvg;

public class ViewNode : FillableContainerNode, IFrameableNode<ViewNode>
{
  public override SceneNodeType Type => SceneNodeType.View;

  public Box? Frame { get; set; }
  public BoxFitType? Fit { get; set; }
  public Box? Box { get; set; } = null;

  public ViewNode UseFrame(Box frame)
  {
    Frame = frame;
    return this;
  }

  public ViewNode UseFit(BoxFitType fix)
  {
    Fit = fix;
    return this;
  }

  public ViewNode UseBox(Box box)
  {
    Box = box;
    return this;
  }
}
