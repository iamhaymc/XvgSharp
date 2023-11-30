using System.Collections;

namespace Xvg;

public class Scene : IEnumerable<ISceneNode>
{
  public ViewNode Root { get; set; }
  public string Description { get; set; }

  public Scene(ViewNode root = null, string description = null)
  {
    Root = root ?? new ViewNode();
    Description = description;
  }

  public IEnumerator<ISceneNode> GetEnumerator() => Root.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
