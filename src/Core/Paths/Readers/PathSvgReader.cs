namespace Xvg;

public class VgPathSvgReader : VgTextReader
{
  public VgPath Path { get; set; }
  public Vector2 Position { get; set; }
  public Vector2 Position0 { get; set; }

  public static VgPath ReadSvgString(string svg)
      => new VgPathSvgReader().ReadSvgString(svg);

  public VgPath ReadSvgString(string svg, VgPath path = null)
  {
    Reset(svg);
    Path = path ?? new VgPath();
    Position = Position0 = Vector2.Zero;
    while (ParseStep()) { };
    return Path;
  }

  private bool ParseStep()
  {
    SkipSpace();
    switch (PullCharacter())
    {
      case 'M': return ParseMoveTo(relative: false);
      case 'm': return ParseMoveTo(relative: true);
      case 'L': return ParseLineTo(relative: false);
      case 'l': return ParseLineTo(relative: true);
      case 'H': return ParseLineHTo(relative: false);
      case 'h': return ParseLineHTo(relative: true);
      case 'V': return ParseLineVTo(relative: false);
      case 'v': return ParseLineVTo(relative: true);
      case 'Q': return ParseBezier2To(relative: false);
      case 'q': return ParseBezier2To(relative: true);
      case 'T': return ParseBezier2STo(relative: false);
      case 't': return ParseBezier2STo(relative: true);
      case 'C': return ParseBezier3To(relative: false);
      case 'c': return ParseBezier3To(relative: true);
      case 'S': return ParseBezier3STo(relative: false);
      case 's': return ParseBezier3STo(relative: true);
      case 'A': return ParseArcTo(relative: false);
      case 'a': return ParseArcTo(relative: true);
      case 'Z': return ParseClose();
      case 'z': return ParseClose();
      default: return false;
    }
  }

  private bool ParseMoveTo(bool relative)
  {
    Vector2? p0 = ParseVector2();
    if (!p0.HasValue)
      return false;
    Path.MoveTo(p0.Value, relative);
    Position = Position0 = relative ? Position + p0.Value : p0.Value;
    return true;
  }

  private bool ParseLineTo(bool relative)
  {
    Vector2? p0 = ParseVector2();
    if (!p0.HasValue)
      return false;
    Path.LineTo(p0.Value, relative);
    Position = relative ? Position + p0.Value : p0.Value;
    return true;
  }

  private bool ParseLineHTo(bool relative)
  {
    float? x = ParseNumber32();
    if (!x.HasValue)
      return false;
    float? y = Position.Y;
    if (!y.HasValue)
      return false;
    Vector2 p0 = new Vector2(relative ? Position.X + x.Value : x.Value, y.Value);
    Path.LineTo(p0, false);
    Position = p0;
    return true;
  }

  private bool ParseLineVTo(bool relative)
  {
    float? x = Position.X;
    if (!x.HasValue)
      return false;
    float? y = ParseNumber32();
    if (!y.HasValue)
      return false;
    Vector2 p0 = new Vector2(x.Value, relative ? Position.Y + y.Value : y.Value);
    Path.LineTo(p0, false);
    Position = p0;
    return true;
  }

  private bool ParseBezier2To(bool relative)
  {
    Vector2? c = ParseVector2();
    if (!c.HasValue)
      return false;
    Vector2? p = ParseVector2();
    if (!p.HasValue)
      return false;
    Path.Bezier2To(c.Value, p.Value, relative);
    Position = relative ? Position + p.Value : p.Value;
    return true;
  }

  private bool ParseBezier2STo(bool relative)
  {
    Vector2 c = Position;
    IVgPathStep tail = Path.ToTail();
    if (tail.IsBezier2To())
    {
      VgBezier2ToStep bezier2Step = (VgBezier2ToStep)tail;
      c += bezier2Step.Point1 - bezier2Step.Point0;
    }
    Vector2? p = ParseVector2();
    if (!p.HasValue)
      return false;
    p = relative ? Position + p : p;
    Path.Bezier2To(c, p.Value, false);
    Position = p.Value;
    return true;
  }

  private bool ParseBezier3To(bool relative)
  {
    Vector2? c0 = ParseVector2();
    if (!c0.HasValue)
      return false;
    Vector2? c1 = ParseVector2();
    if (!c1.HasValue)
      return false;
    Vector2? p = ParseVector2();
    if (!p.HasValue)
      return false;
    Path.Bezier3To(c0.Value, c1.Value, p.Value, relative);
    Position = relative ? Position + p.Value : p.Value;
    return true;
  }

  private bool ParseBezier3STo(bool relative)
  {
    Vector2 c0 = Position;
    IVgPathStep tail = Path.ToTail();
    if (tail.IsBezier3To())
    {
      VgBezier3ToStep bezier3Step = (VgBezier3ToStep)tail;
      c0 += bezier3Step.Point2 - bezier3Step.Point1;
    }
    Vector2? c1 = ParseVector2();
    if (!c1.HasValue)
      return false;
    c1 = relative ? Position + c1 : c1;
    Vector2? p = ParseVector2();
    if (!p.HasValue)
      return false;
    p = relative ? Position + p : p;
    Path.Bezier3To(c0, c1.Value, p.Value, false);
    Position = p.Value;
    return true;
  }

  private bool ParseArcTo(bool relative)
  {
    Vector2? r = ParseVector2();
    if (!r.HasValue)
      return false;
    float? a = ParseNumber32();
    if (!a.HasValue)
      return false;
    bool? l = ParseBoolean();
    if (!l.HasValue)
      return false;
    bool? s = ParseBoolean();
    if (!s.HasValue)
      return false;
    Vector2? p = ParseVector2();
    if (!p.HasValue)
      return false;
    Path.ArcTo(p.Value, r.Value, a.Value, l.Value, s.Value, relative);
    Position = relative ? Position + p.Value : p.Value;
    return true;
  }

  private bool ParseClose()
  {
    Path.Close();
    Position = Position0;
    return true;
  }
}
