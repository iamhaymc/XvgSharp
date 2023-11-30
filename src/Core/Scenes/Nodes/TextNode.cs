namespace Xvg;

public class TextNode : FillableShapeNode
{
  public override SceneNodeType Type => SceneNodeType.Text;

  public TextStyle Text { get; set; } = new TextStyle();
  public FontStyle Font { get; set; } = new FontStyle();

  public TextNode UseValue(string value)
  {
    Text.Value = value;
    return this;
  }

  public TextNode UsePosition(Vector2? position = null, TextJustifyType? justify = null, TextAlignType? align = null)
  {
    if (position != null)
      Text.Position = position;
    if (justify != null)
      Text.Justify = justify;
    if (align != null)
      Text.Align = align;
    return this;
  }

  public TextNode UseFont(FontFamilyType? family = null, FontWeightType? weight = null, FontStyleType? style = null, float? size = null)
  {
    if (family != null)
      Font.Family = family;
    if (weight != null)
      Font.Weight = weight;
    if (style != null)
      Font.Style = style;
    if (size != null)
      Font.Size = size;
    return this;
  }
}