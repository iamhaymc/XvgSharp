using System.Text;
using System.Xml.Linq;

namespace Xvg;

public class SceneSvgExporter
{
  public const bool DefaultIndent = true;

  public bool _embedImages;
  public bool _embedFonts;

  private Dictionary<string, string> _fontUrls { get; set; }

  public SceneSvgExporter()
  {
    UseImageEmbedding(true);
    UseFontEmbedding(false);
    _fontUrls = new Dictionary<string, string>();
  }

  public SceneSvgExporter UseImageEmbedding(bool truth)
  {
    _embedImages = truth;
    return this;
  }

  public SceneSvgExporter UseFontEmbedding(bool truth)
  {
    _embedFonts = truth;
    return this;
  }

  public SceneSvgExporter BindFont(string family, string url)
  {
    _fontUrls[family] = url;
    return this;
  }

  public FileInfo WriteSvgFile(Scene scene, string path, bool indent = DefaultIndent)
  {
    FsFile.StringOut(path, WriteSvgString(scene, indent));
    return new FileInfo(path);
  }

  public string WriteSvgString(Scene scene, bool indent = DefaultIndent)
  {
    XDocument doc = WriteSvgDocument(scene);
    return doc.ToString(indent ? SaveOptions.None : SaveOptions.DisableFormatting);
  }

  public XDocument WriteSvgDocument(Scene scene)
  {
    return ExportScene(scene);
  }

  private XDocument ExportScene(Scene scene)
  {
    XDocument xSvg = XSvgDocument.Create()
      .SetSvgAttribute("width", scene.Root.Frame?.Width)
      .SetSvgAttribute("height", scene.Root.Frame?.Height)
      .SetSvgAttribute("viewBox", SerializeViewBox(scene.Root.Box))
      .SetSvgAttribute("preserveAspectRatio", SerializeFitStyle(scene.Root.Fit));

    if (!string.IsNullOrEmpty(scene.Description))
    {
      xSvg.NewSvgElement("desc").SetValue(scene.Description);
    }

    if (_embedFonts)
    {
      StringBuilder styleText = new StringBuilder();
      HashSet<string> seenFonts = new HashSet<string>();
      foreach (TextNode textNode in scene.Root
        .Enumerate()
        .Where(n => n.Type == SceneNodeType.Text)
        .Cast<TextNode>())
      {
        string fontFamily = textNode.FontFamily.ToSvgStyle();
        if (fontFamily != null
          && !seenFonts.Contains(fontFamily)
          && _fontUrls.TryGetValue(fontFamily, out string fontUrl))
        {
          string fontSource = Media.HasDataUri(fontUrl) ? fontUrl : Media.FromFile(new FileInfo(fontUrl)).ToDataUrl();
          styleText.AppendLine($"@font-face {{");
          styleText.AppendLine($" font-family: \"{fontFamily}\";");
          styleText.AppendLine($" src: url(\"{fontSource}\");");
          styleText.AppendLine($"}}");
          seenFonts.Add(fontFamily);
        }
      }
      if (styleText.Length != 0)
        xSvg.NewSvgElement("style")
          .SetSvgAttribute("type", "text/css")
          .SetSvgTextContent("\n" + styleText.ToString());
    }

    if (scene.Root.Fill.Color != null)
    {
      // The fill rect is contained by the view so it uses the the view box metrics.
      // Fallback to the view frame metrics if the view box is not defined.
      xSvg.NewSvgElement("rect")
        .SetSvgAttribute("x", scene.Root.Box?.X ?? scene.Root.Frame?.X)
        .SetSvgAttribute("y", scene.Root.Box?.Y ?? scene.Root.Frame?.Y)
        .SetSvgAttribute("width", scene.Root.Box?.Width ?? scene.Root.Frame?.Width)
        .SetSvgAttribute("height", scene.Root.Box?.Height ?? scene.Root.Frame?.Height)
        .SetSvgAttribute("fill", scene.Root.Fill.Color.ToHex());
    }

    foreach (ISceneNode child in scene.Root.Nodes)
      ExportAnyNode(child, scene.Root, xSvg.Root);

    return xSvg;
  }

  private void ExportViewNode(ViewNode node, ViewNode viewNode, XElement parent)
  {
    string transform = SerializeTransform(node.Transform);

    XElement xView = parent.NewSvgElement("svg")
        .SetSvgAttribute("x", node.Frame?.X)
        .SetSvgAttribute("y", node.Frame?.Y)
        .SetSvgAttribute("width", node.Frame?.Width)
        .SetSvgAttribute("height", node.Frame?.Height)
        .SetSvgAttribute("transform", transform)
        .SetSvgAttribute("viewBox", SerializeViewBox(node.Box))
        .SetSvgAttribute("preserveAspectRatio", SerializeFitStyle(node.Fit));

    if (node.Fill.Color != null)
    {
      // The fill rect is contained by the view so it uses the the view box metrics.
      // Fallback to the view frame metrics if the view box is not defined.
      xView.NewSvgElement("rect")
        .SetSvgAttribute("x", node.Box?.X ?? node.Frame?.X)
        .SetSvgAttribute("y", node.Box?.Y ?? node.Frame?.Y)
        .SetSvgAttribute("width", node.Box?.Width ?? node.Frame?.Width)
        .SetSvgAttribute("height", node.Box?.Height ?? node.Frame?.Height)
        .SetSvgAttribute("fill", node.Fill.Color.ToHex());
    }

    foreach (ISceneNode child in node.Nodes)
      ExportAnyNode(child, node, xView);
  }

  private void ExportImageNode(ImageNode node, ViewNode viewNode, XElement parent)
  {
    string transform = SerializeTransform(node.Transform,
      origin: (node.Frame?.Min ?? Vector2.Zero)
            + (node.Frame?.HalfSize ?? Vector2.Zero));

    string source = !_embedImages ? node.Url : Media.FromFile(new FileInfo(node.Url)).ToDataUrl();

    string filterId = GenerateFilters(node, viewNode, parent);

    parent.NewSvgElement("image")
      .SetSvgAttribute("x", node.Frame?.X)
      .SetSvgAttribute("y", node.Frame?.Y)
      .SetSvgAttribute("width", node.Frame?.Width.ToString() ?? "100%")
      .SetSvgAttribute("height", node.Frame?.Height.ToString() ?? "100%")
      .SetSvgAttribute("transform", transform)
      .SetSvgAttribute("preserveAspectRatio", SerializeFitStyle(node.Fit))
      .SetSvgAttribute("filter", filterId != null ? $"url(#{filterId})" : null)
      .SetSvgAttribute("shape-rendering", SerializeShapeRendering(node.AntiAlias))
      .SetSvgAttribute("href", source);
  }

  private void ExportPathNode(PathNode node, ViewNode viewNode, XElement parent)
  {
    string transform = SerializeTransform(node.Transform);

    string filterId = GenerateFilters(node, viewNode, parent);

    parent.NewSvgElement("path")
      .SetSvgAttribute("id", node.Id)
      .SetSvgAttribute("transform", transform)
      .SetSvgAttribute("fill", node.Fill.Color?.ToHex() ?? "none")
      .SetSvgAttribute("fill-rule", node.Fill.Rule?.ToSvgStyle())
      .SetSvgAttribute("stroke", node.Stroke.Color?.ToHex())
      .SetSvgAttribute("stroke-width", node.Stroke.Width)
      .SetSvgAttribute("stroke-linecap", node.Stroke.Cap?.ToSvgStyle())
      .SetSvgAttribute("stroke-linejoin", node.Stroke.Joint?.ToSvgStyle())
      .SetSvgAttribute("filter", filterId != null ? $"url(#{filterId})" : null)
      .SetSvgAttribute("shape-rendering", SerializeShapeRendering(node.AntiAlias))
      .SetSvgAttribute("d", node.Value?.ToSvgString());
  }

  private void ExportTextNode(TextNode node, ViewNode viewNode, XElement parent)
  {
    string transform = SerializeTransform(node.Transform,
      offset: node.Position);

    string filterId = GenerateFilters(node, viewNode, parent);

    parent.NewSvgElement("text")
      .SetSvgAttribute("id", node.Id)
      .SetSvgAttribute("transform", transform)
      .SetSvgAttribute("fill", node.Fill.Color?.ToHex() ?? "none")
      .SetSvgAttribute("fill-rule", node.Fill.Rule?.ToSvgStyle())
      .SetSvgAttribute("font-family", node.FontFamily.ToSvgStyle())
      .SetSvgAttribute("font-weight", node.FontWeight.ToSvgStyle())
      .SetSvgAttribute("font-style", node.FontStyle.ToSvgStyle())
      .SetSvgAttribute("font-size", node.FontSize)
      .SetSvgAttribute("text-anchor", node.Justify.ToSvgStyle())
      .SetSvgAttribute("alignment-baseline", node.Align.ToSvgStyle())
      .SetSvgAttribute("filter", filterId != null ? $"url(#{filterId})" : null)
      .SetSvgAttribute("shape-rendering", SerializeShapeRendering(node.AntiAlias))
      .SetSvgAttribute("text-rendering", SerializeTextRendering(node.AntiAlias))
      .SetSvgTextContent(node.Value);
  }

  private void ExportAnyNode(ISceneNode node, ViewNode viewNode, XElement parent)
  {
    switch (node.Type)
    {
      case SceneNodeType.View:
        ExportViewNode((ViewNode)node, viewNode, parent);
        break;
      case SceneNodeType.Filter:
        throw new NotImplementedException();
      case SceneNodeType.Group:
        throw new NotImplementedException();;
      case SceneNodeType.Image:
        ExportImageNode((ImageNode)node, viewNode, parent);
        break;
      case SceneNodeType.Path:
        ExportPathNode((PathNode)node, viewNode, parent);
        break;
      case SceneNodeType.Text:
        ExportTextNode((TextNode)node, viewNode, parent);
        break;
      case SceneNodeType.Copy:
        throw new NotImplementedException();
      default:
        break;
    }
  }

  private string GenerateFilters(ISceneNode node, ViewNode viewNode, XElement parent)
  {
    string filterId = Identifier.NextId;

    //XElement xFilter = parent.NewSvgElement("filter")
    //  .SetSvgAttribute("id", filterId)
    //  .SetSvgAttribute("filterUnits", "userSpaceOnUse")
    //  .SetSvgAttribute("x", viewNode.ViewBox.X)
    //  .SetSvgAttribute("y", viewNode.ViewBox.Y)
    //  .SetSvgAttribute("width", viewNode.ViewBox.Width)
    //  .SetSvgAttribute("height", viewNode.ViewBox.Height);

    //xFilter.NewSvgElement("feDropShadow")
    //  .SetSvgAttribute("dx", node.ShadowOffset.X)
    //  .SetSvgAttribute("dy", node.ShadowOffset.Y)
    //  .SetSvgAttribute("stdDeviation", node.ShadowSigma)
    //  .SetSvgAttribute("flood-color", "black")
    //  .SetSvgAttribute("flood-opacity", node.ShadowOpacity);

    return filterId;
  }

  private string SerializeViewBox(Box? box)
  {
    string value = !box.HasValue ? null :
      string.Join(" ",
        box.Value.X, box.Value.Y,
        box.Value.Width, box.Value.Height)
      .Trim();
    return string.IsNullOrEmpty(value) ? null : value;
  }

  private string SerializeFitStyle(BoxFitType? type)
    => type?.ToSvgStyle() ?? FitStyle.SvgXMidYMidMeet;

  private string SerializeShapeRendering(bool antialias)
    => antialias ? "auto" : "crispEdges";

  private string SerializeTextRendering(bool antialias)
    => antialias ? "auto" : "geometricPrecision";

  private string SerializeTransform(Transform transform, Vector2? offset = null, Vector2? origin = null)
    => string
      .Join(" ",
        SerializeTranslation(transform, offset),
        SerializeRotation(transform, origin),
        SerializeScale(transform))
      .Trim();

  private string SerializeTranslation(Transform transform, Vector2? offset = null)
  {
    Vector2 translation = transform.Translation + (offset ?? Vector2.Zero);
    return $"translate({translation.X} {translation.Y})";
  }

  private string SerializeRotation(Transform transform, Vector2? origin = null)
    => $"rotate({transform.Rotation} {origin?.X ?? 0} {origin?.Y ?? 0})";

  private string SerializeScale(Transform transform)
    => $"scale({transform.Scale.X} {transform.Scale.Y})";
}

public static class SceneSvgWriterExtensions
{
  public static FileInfo ToSvgFile(this Scene self, string path, bool indent = SceneSvgExporter.DefaultIndent, SceneSvgExporter writer = null) 
    => (writer ?? new SceneSvgExporter()).WriteSvgFile(self, path, indent);

  public static string ToSvgString(this Scene self, bool indent = SceneSvgExporter.DefaultIndent, SceneSvgExporter writer = null) 
    => (writer ?? new SceneSvgExporter()).WriteSvgString(self, indent);

  public static XDocument ToSvgDocument(this Scene self, SceneSvgExporter writer = null) 
    => (writer ?? new SceneSvgExporter()).WriteSvgDocument(self);
}
