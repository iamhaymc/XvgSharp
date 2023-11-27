namespace Xvg;

public class Scene
{
  public ViewNode Root { get; set; }
  public string Description { get; set; }

  public Scene(ViewNode root = null, string description = null)
  {
    Root = root ?? new ViewNode();
    Description = description;
  }

  public IEnumerable<ISceneNode> YieldNodes()
    => Root.YieldNodes();
}
