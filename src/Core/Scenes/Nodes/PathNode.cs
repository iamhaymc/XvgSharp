namespace Xvg;

public class PathNode : StrokableShapeNode
{
  public override SceneNodeType Type => SceneNodeType.Path;

  public Path Value { get; set; } = new Path();

  public PathNode UseValue(Path path)
  {
    Value = path;
    return this;
  }
}
