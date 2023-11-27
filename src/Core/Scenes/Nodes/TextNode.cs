namespace Xvg;

public class TextNode : SceneNode, 
  IAliasableNode<TextNode>, IFillableNode<TextNode>, IFilterableNode<TextNode>
{
  public override SceneNodeType Type => SceneNodeType.Text;

  #region [Properties]

  public bool Abstract { get; set; } = false;
  public bool AntiAlias { get; set; } = true;
  public string Value { get; set; }
  public FontFamilyType FontFamily { get; set; } = Xvg.FontStyle.DefaultFamily;
  public FontWeightType FontWeight { get; set; } = Xvg.FontStyle.DefaultWeight;
  public FontStyleType FontStyle { get; set; } = Xvg.FontStyle.DefaultStyle;
  public float FontSize { get; set; } = Xvg.FontStyle.DefaultSize;
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Transform Transform { get; set; } = Transform.Identity;
  public TextJustifyType Justify { get; set; } = TextStyle.DefaultJustify;
  public TextAlignType Align { get; set; } = TextStyle.DefaultAlign;
  public ColorType FillColor { get; set; } = FillStyle.DefaultColor;
  public FillRuleType FillRule { get; set; } = FillStyle.DefaultRule;
  public string FilterId { get; set; } = null;

  #endregion

  #region [Edit]

  public TextNode UseAbstraction(bool truth)
  {
    Abstract = truth;
    return this;
  }

  public TextNode UseValue(string value)
  {
    Value = value;
    return this;
  }

  public TextNode UseFont(FontFamilyType family, FontWeightType weight, FontStyleType style, float size)
  {
    FontFamily = family;
    FontWeight = weight;
    FontStyle = style;
    FontSize = size;
    return this;
  }

  public TextNode UsePosition(Vector2 position, TextJustifyType justify, TextAlignType align)
  {
    Position = position;
    Justify = justify;
    Align = align;
    return this;
  }

  public TextNode UseTranslation(Vector2 translation)
  {
    throw new NotImplementedException();
  }

  public TextNode UseRotation(float degrees)
  {
    Transform = Transform.WithRotation(degrees);
    return this;
  }

  public TextNode UseScale(Vector2 scale)
  {
    throw new NotImplementedException();
  }

  public TextNode UseFill(ColorType color, FillRuleType rule)
  {
    FillColor = color;
    FillRule = rule;
    return this;
  }

  public TextNode UseFilter(string filterId)
  {
    FilterId = filterId;
    return this;
  }

  public TextNode UseAntiAliasing(bool truth)
  {
    throw new NotImplementedException();
  }

  #endregion
}
