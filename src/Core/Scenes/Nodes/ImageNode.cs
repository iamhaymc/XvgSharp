namespace Xvg;

public class ImageNode : ShapeNode, IFrameableNode<ImageNode>
{
  public override SceneNodeType Type => SceneNodeType.Image;

  public string Url { get; set; }
  public Box? Frame { get; set; }
  public BoxFitType? Fit { get; set; }

  public ImageNode UseUrl(string url)
  {
    Url = url;
    return this;
  }

  public ImageNode UseFrame(Box frame)
  {
    Frame = frame;
    return this;
  }

  public ImageNode UseFit(BoxFitType fit)
  {
    Fit = fit;
    return this;
  }
}
