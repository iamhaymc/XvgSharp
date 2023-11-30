using System.Collections.Generic;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// A series of graphics operations
  /// </summary>
  public class GfxProgram
  {
    public List<IGfxOperation> Operations { get; set; }

    public GfxProgram Add(IGfxOperation op) { Operations.Add(op); return this; }
  }

  /// <summary>
  /// A specific graphics operation
  /// </summary>
  public interface IGfxOperation { }

  /// <summary>
  /// A specific graphics operation
  /// </summary>
  public class GfxOperation<TOperand> : IGfxOperation
  {
    public GfxOperationType Type { get; set; }
    public List<TOperand> Operands { get; set; }

    public GfxOperation<TOperand> Add(TOperand op) { Operands.Add(op); return this; }
  }

  /// <remarks>
  /// Based on the GDI metafile functions defined in wingdi.h 
  /// </remarks>
  public enum GfxOperationType
  {
    SETBKCOLOR = 0x0201,
    SETBKMODE = 0x0102,
    SETMAPMODE = 0x0103,
    SETROP2 = 0x0104,
    SETRELABS = 0x0105,
    SETPOLYFILLMODE = 0x0106,
    SETSTRETCHBLTMODE = 0x0107,
    SETTEXTCHAREXTRA = 0x0108,
    SETTEXTCOLOR = 0x0209,
    SETTEXTJUSTIFICATION = 0x020A,
    SETWINDOWORG = 0x020B,
    SETWINDOWEXT = 0x020C,
    SETVIEWPORTORG = 0x020D,
    SETVIEWPORTEXT = 0x020E,
    OFFSETWINDOWORG = 0x020F,
    SCALEWINDOWEXT = 0x0410,
    OFFSETVIEWPORTORG = 0x0211,
    SCALEVIEWPORTEXT = 0x0412,
    LINETO = 0x0213,
    MOVETO = 0x0214,
    EXCLUDECLIPRECT = 0x0415,
    INTERSECTCLIPRECT = 0x0416,
    ARC = 0x0817,
    ELLIPSE = 0x0418,
    FLOODFILL = 0x0419,
    PIE = 0x081A,
    RECTANGLE = 0x041B,
    ROUNDRECT = 0x061C,
    PATBLT = 0x061D,
    SAVEDC = 0x001E,
    SETPIXEL = 0x041F,
    OFFSETCLIPRGN = 0x0220,
    TEXTOUT = 0x0521,
    BITBLT = 0x0922,
    STRETCHBLT = 0x0B23,
    POLYGON = 0x0324,
    POLYLINE = 0x0325,
    ESCAPE = 0x0626,
    RESTOREDC = 0x0127,
    FILLREGION = 0x0228,
    FRAMEREGION = 0x0429,
    INVERTREGION = 0x012A,
    PAINTREGION = 0x012B,
    SELECTCLIPREGION = 0x012C,
    SELECTOBJECT = 0x012D,
    SETTEXTALIGN = 0x012E,
    DRAWTEXT = 0x062F,
    CHORD = 0x0830,
    SETMAPPERFLAGS = 0x0231,
    EXTTEXTOUT = 0x0A32,
    SETDIBTODEV = 0x0D33,
    SELECTPALETTE = 0x0234,
    REALIZEPALETTE = 0x0035,
    ANIMATEPALETTE = 0x0436,
    SETPALENTRIES = 0x0037,
    POLYPOLYGON = 0x0538,
    RESIZEPALETTE = 0x0139,
    DIBBITBLT = 0x0940,
    DIBSTRETCHBLT = 0x0B41,
    DIBCREATEPATTERNBRUSH = 0x0142,
    STRETCHDIB = 0x0F43,
    EXTFLOODFILL = 0x0548,
    RESETDC = 0x014C,
    STARTDOC = 0x014D,
    STARTPAGE = 0x004F,
    ENDPAGE = 0x0050,
    ABORTDOC = 0x0052,
    ENDDOC = 0x005E,
    SETLAYOUT = 0x0149,
    DELETEOBJECT = 0x01F0,
    CREATEPALETTE = 0x00F7,
    CREATEBRUSH = 0x00F8,
    CREATEPATTERNBRUSH = 0x01F9,
    CREATEPENINDIRECT = 0x02FA,
    CREATEFONTINDIRECT = 0x02FB,
    CREATEBRUSHINDIRECT = 0x02FC,
    CREATEBITMAPINDIRECT = 0x02FD,
    CREATEBITMAP = 0x06FE,
    CREATEREGION = 0x06FF,
    UNKNOWN = 0x0529,
  }
}
