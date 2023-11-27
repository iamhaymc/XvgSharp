
namespace Xvg;

public class VgAxisModel
{
  public Vector2 PathHead { get; set; }
  public Vector2 PathTail { get; set; }
  public ColorType PathColor { get; set; }

  public Range TickRange { get; set; }
  public int TickCount { get; set; }
  public ColorType TickColor { get; set; }
  public FontFamilyType TickFontFamily { get; set; }
  public FontWeightType TickFontWeight { get; set; }
  public FontStyleType TickFontStyle { get; set; }

  public string Title { get; set; }
  public ColorType TitleColor { get; set; }
  public FontFamilyType TitleFontFamily { get; set; }
  public FontWeightType TitleFontWeight { get; set; }
  public FontStyleType TitleFontStyle { get; set; }

  public bool Metric { get; set; }
  public bool Upright { get; set; }
  public bool Mirror { get; set; }

  public VgAxisModel()
  {
    UseColor(ColorType.Black);
    UseFont(FontFamilyType.SansSerif, FontWeightType.Normal, FontStyleType.Normal);
    UseMetric(true);
    UseUpright(false);
    UseMirror(false);
  }

  public VgAxisModel UsePath(Vector2 head, Vector2 tail)
  {
    PathHead = head;
    PathTail = tail;
    return this;
  }

  public VgAxisModel UseTicks(Range range, int count)
  {
    TickRange = range;
    TickCount = count;
    return this;
  }

  public VgAxisModel UseTitle(string title)
  {
    Title = title;
    return this;
  }

  public VgAxisModel UseColor(ColorType color)
  {
    PathColor = TickColor = TitleColor = color;
    return this;
  }

  public VgAxisModel UseFont(FontFamilyType family, FontWeightType weight, FontStyleType style)
  {
    TickFontFamily = TitleFontFamily = family;
    TickFontWeight = TitleFontWeight = weight;
    TickFontStyle = TitleFontStyle = style;
    return this;
  }

  public VgAxisModel UseMetric(bool truth)
  {
    Metric = truth;
    return this;
  }

  public VgAxisModel UseUpright(bool truth)
  {
    Upright = truth;
    return this;
  }

  public VgAxisModel UseMirror(bool truth)
  {
    Mirror = truth;
    return this;
  }
}

public static class VgSceneBuilderAxisExtensions
{
  public static SceneBuilder AddAxis(this SceneBuilder self, Action<VgAxisModel> edit)
  {
    // Edit model
    VgAxisModel model = new VgAxisModel();
    edit?.Invoke(model);

    // Draw model
    Box box = self.View.ViewBox;
    float unitSize = Math.Min(box.Size.X, box.Size.Y) * 1/600;

    Vector2 pathCenter = model.PathHead.Lerp(model.PathTail, 0.5);
    Vector2 pathForward = model.PathHead.Sub(model.PathTail).Normalize();
    Vector2 pathOrthogonal = pathForward.ToOrthogonal();
    float pathAngle = Radial.ToDegrees(pathForward.ToAngle());

    float pathWidth = unitSize;

    float tickLength = unitSize * 5;
    float tickRotation = pathAngle;
    var tickJustify = TextJustifyType.Middle;
    var tickAlign = TextAlignType.Bottom;
    float tickFontSize = FontSizeStyle.ToSize0(box.Size);

    float titleRotation = pathAngle;
    var titleJustify = TextJustifyType.Middle;
    var titleAlign = TextAlignType.Bottom;
    float titleFontSize = FontSizeStyle.ToSize1(box.Size);

    if (model.Mirror)
    {
      pathOrthogonal = pathOrthogonal.Neg();
      if (!(pathAngle >= 0 && pathAngle <= 90
        || pathAngle >= 270 && pathAngle <= 360))
      {
        tickRotation += 180;
        titleRotation = tickRotation;
        tickAlign = TextAlignType.Top;
        titleAlign = TextAlignType.Top;
      }
    }
    else if (pathAngle >= 180 && pathAngle <= 360)
    {
      tickRotation += 180;
      titleRotation = tickRotation;
    }

    Vector2 tickOffset0 = pathOrthogonal * tickLength;
    Vector2 tickOffset1 = tickOffset0 + pathOrthogonal * tickLength / 2f;
    Vector2 titleOffset = tickOffset1 + pathOrthogonal * titleFontSize;

    if (model.Upright)
    {
      if (pathAngle >= 180 && pathAngle <= 360)
      {
        tickRotation = 0;
        tickAlign = TextAlignType.Middle;
        tickJustify = model.Mirror ? TextJustifyType.End : TextJustifyType.Start;
      }
    }

    self.AddPath(n => n
      .UseValue(p => p.AddLine(model.PathHead, model.PathTail))
      .UseStroke(model.PathColor, StrokeJointType.Miter, StrokeCapType.Butt, pathWidth));

    int maxTickLabelLength = 0;

    double[] ticks = model.TickRange.ToTicks(model.TickCount, fit: true);
    ContinuousScale<Vector2> tickScale = Scale.ToLinear(
      ticks.First(), ticks.Last(), model.PathHead, model.PathTail, Lerp.To);

    foreach (double tick in ticks)
    {
      Vector2 tickPoint0 = tickScale.Scale(tick);
      Vector2 tickPoint1 = tickPoint0 + tickOffset0;
      Vector2 tickLabelPoint = tickPoint0 + tickOffset1;

      string tickText = model.Metric ? TextFormat.ToMetric(tick) : tick.ToString();
      maxTickLabelLength = Math.Max(maxTickLabelLength, tickText.Length);

      self.AddPath(n => n
        .UseValue(p => p.AddLine(tickPoint0, tickPoint1))
        .UseStroke(model.TickColor, StrokeJointType.Miter, StrokeCapType.Butt, pathWidth));

      self.AddText(n => n
        .UseValue(tickText)
        .UsePosition(tickLabelPoint, tickJustify, tickAlign)
        .UseRotation(tickRotation)
        .UseFont(model.TickFontFamily, FontWeightType.Normal, FontStyleType.Normal, tickFontSize)
        .UseFill(model.TickColor, FillRuleType.NonZero));
    }

    int minTickLabelLength = Math.Max(0, maxTickLabelLength - (model.Upright && model.Metric ? 2 : 3));
    titleOffset += pathOrthogonal * (minTickLabelLength * tickFontSize);

    Vector2 titlePoint = pathCenter + titleOffset;

    self.AddText(n => n
      .UseValue(model.Title)
      .UsePosition(titlePoint, titleJustify, titleAlign)
      .UseRotation(titleRotation)
      .UseFont(model.TitleFontFamily, FontWeightType.Normal, FontStyleType.Normal, titleFontSize)
      .UseFill(model.TitleColor, FillRuleType.NonZero));

    return self;
  }
}
