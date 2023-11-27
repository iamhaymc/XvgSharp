namespace Xvg;

public class SceneBuilder
{
  public static SceneBuilder Into(Action<ViewNode> edit)
    => new SceneBuilder().IntoView(edit);

  #region [Core]

  public ViewNode View { get; set; }
  public ISceneNode Parent { get; set; }
  public Stack<ISceneNode> Anscestors { get; set; } = new Stack<ISceneNode>();

  public SceneBuilder IntoNode<TNode>(Action<TNode> edit) where TNode : ISceneNode, new()
    => IntoNode(new TNode(), edit);

  public SceneBuilder IntoNode<TNode>(TNode node, Action<TNode> edit) where TNode : ISceneNode
  {
    edit?.Invoke(node);
    if (Parent != null)
    {
      Parent.Nodes.Add(node);
      Anscestors.Push(Parent);
    }
    Parent = node;
    if (Parent is ViewNode)
      View = (ViewNode)Parent;
    return this;
  }

  public SceneBuilder AddNode<TNode>(Action<TNode> edit) where TNode : ISceneNode, new()
    => AddNode(new TNode(), edit);

  public SceneBuilder AddNode<TNode>(TNode node, Action<TNode> edit) where TNode : ISceneNode
  {
    if (Parent == null)
      throw new NoParentNodeError();
    edit?.Invoke(node);
    Parent.Nodes.Add(node);
    return this;
  }

  public SceneBuilder ExitNode()
  {
    if (Anscestors.Count == 0)
      throw new NoAnscestorsError();
    Parent = Anscestors.Pop();
    View = (ViewNode)Anscestors.Reverse().FirstOrDefault(x => x is ViewNode);
    return this;
  }

  public Scene ToScene()
  {
    if (Anscestors.Count > 0)
      throw new OpenAnscestorsError();
    if (Parent?.GetType() != typeof(ViewNode))
      throw new NoRootViewError();
    return new Scene((ViewNode)Parent);
  }

  #endregion

  #region [Extra]

  public SceneBuilder IntoView(Action<ViewNode> edit)
    => IntoNode<ViewNode>(edit);

  public SceneBuilder IntoGroup(Action<GroupNode> edit)
    => IntoNode<GroupNode>(edit);

  public SceneBuilder AddPath(Action<PathNode> edit)
    => AddNode<PathNode>(edit);

  public SceneBuilder AddText(Action<TextNode> edit)
    => AddNode<TextNode>(edit);

  public SceneBuilder AddImage(Action<ImageNode> edit)
    => AddNode<ImageNode>(edit);

  public SceneBuilder AddCopy(Action<CopyNode> edit)
    => AddNode<CopyNode>(edit);

  #endregion

  #region [Exceptions]

  public class NoRootViewError : Exception
  { public NoRootViewError() : base("There is no root view") { } }

  public class NoParentNodeError : Exception
  { public NoParentNodeError() : base("There is no parent node") { } }

  public class NoAnscestorsError : Exception
  { public NoAnscestorsError() : base("There are no ancestor nodes") { } }

  public class OpenAnscestorsError : Exception
  { public OpenAnscestorsError() : base("The are open ancestor nodes") { } }

  #endregion
}