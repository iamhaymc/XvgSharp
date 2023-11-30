namespace Xvg;

public class CopyNode : ShapeNode
{
  public override SceneNodeType Type => SceneNodeType.Copy;

  public string ReferenceId { get; set; }
}