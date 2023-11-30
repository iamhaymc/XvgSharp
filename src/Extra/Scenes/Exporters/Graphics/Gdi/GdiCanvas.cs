using System;
using Vanara.PInvoke;
using Gdi = Vanara.PInvoke.Gdi32;

namespace Trane.Submittals.Pipeline
{
  public class GdiCanvas : GfxCanvas, IDisposable
  {
    private HDC _dc = HDC.NULL;
    private Gdi.SafeHDC _emf = Gdi.SafeHDC.Null;
    private PRECT _frame = null;
    private string _description = null;
    private bool _open = false;

    private IGfxBrush _brush = null;
    private bool _ownBrush = false;

    private IGfxPen _pen = null;
    private bool _ownPen = false;

    public GdiCanvas(string filePath, GfxSizeL frame, string description = null)
    {
      Open(filePath, frame, description);
    }

    public GdiCanvas Open(string filePath, GfxSizeL frame, string description = null)
    {
      Close();
      EnsureDisplayDevice();
      GfxRectL frameRect = CreateFrame(frame.W, frame.H, frame.Uom ?? GfxSizeUnit.Millis);
      _frame = new PRECT((int)frameRect.Left, (int)frameRect.Top, (int)frameRect.Right, (int)frameRect.Bottom);
      _description = description ?? "An enhanced metafile";
      _emf = Gdi.CreateEnhMetaFile(HDC.NULL, filePath, _frame, _description);
      _open = true;
      return this;
    }

    private HDC EnsureDisplayDevice()
    {
      if (_dc == HDC.NULL)
        _dc = Gdi.CreateDC("DISPLAY", "DISPLAY", null, IntPtr.Zero);
      return _dc;
    }

    protected override GfxSizeD EnsureDisplaySizeMillis()
    {
      EnsureDisplayDevice();
      return new GfxSizeD(Gdi.GetDeviceCaps(_dc, Gdi.DeviceCap.HORZSIZE), Gdi.GetDeviceCaps(_dc, Gdi.DeviceCap.VERTSIZE));
    }

    protected override GfxSizeD EnsureDisplayResolutionPixels()
    {
      EnsureDisplayDevice();
      return new GfxSizeD(Gdi.GetDeviceCaps(_dc, Gdi.DeviceCap.HORZRES), Gdi.GetDeviceCaps(_dc, Gdi.DeviceCap.VERTRES));
    }

    public override IGfxBrush CreateBrush(byte r, byte g, byte b)
    {
      var handle = Gdi.CreateSolidBrush(new COLORREF(r, g, b));
      return new GdiBrush(new GfxRgbaColor(r, g, b, 255), handle);
    }

    public override IGfxPen CreatePen(byte r, byte g, byte b, int w)
    {
      var handle = Gdi.CreatePen((uint)Gdi.PenStyle.PS_SOLID, w, new COLORREF(r, g, b));
      return new GdiPen(new GfxRgbaColor(r, g, b, 255), w, handle);
    }

    public override IGfxCanvas UseBrush(IGfxBrush brush)
    {
      if (_brush != null && _ownBrush)
      {
        _brush.Dispose();
        _brush = null;
        _ownBrush = false;
      }
      if (brush is GdiBrush)
      {
        var gdiBrush = (GdiBrush)brush;
        _brush = gdiBrush;
        _ownBrush = false;
        Gdi.SelectObject(_emf, gdiBrush.Handle);
      }
      else
      {
        var gdiBrush = (GdiBrush)CreateBrush(brush.Color);
        _brush = gdiBrush;
        _ownBrush = true;
        Gdi.SelectObject(_emf, gdiBrush.Handle);
      }
      return this;
    }

    public override IGfxCanvas UsePen(IGfxPen pen)
    {
      if (_pen != null && _ownPen)
      {
        _pen.Dispose();
        _pen = null;
        _ownPen = false;
      }
      if (pen is GdiPen)
      {
        var gdiPen = (GdiPen)pen;
        _pen = gdiPen;
        _ownPen = false;
        Gdi.SelectObject(_emf, gdiPen.Handle);
      }
      else
      {
        var gdiPen = (GdiPen)CreateBrush(pen.Color);
        _pen = gdiPen;
        _ownPen = true;
        Gdi.SelectObject(_emf, gdiPen.Handle);
      }
      return this;
    }

    public override IGfxCanvas MoveTo(int x, int y)
    {
      if (!Gdi.MoveToEx(_emf, x, y, out POINT lastPoint))
        throw new GdiOperationException("MoveToEx");
      return this;
    }

    public override IGfxCanvas StartPath()
    {
      if (!Gdi.BeginPath(_emf))
        throw new GdiOperationException("BeginPath");
      return this;
    }

    public override IGfxCanvas LineTo(int x, int y)
    {
      if (!Gdi.LineTo(_emf, x, y))
        throw new GdiOperationException("LineTo");
      return this;
    }

    public override IGfxCanvas BezierTo(int c0x, int c0y, int c1x, int c1y, int x, int y)
    {
      if (!Gdi.PolyBezierTo(_emf, new[] { new POINT(c0x, c0y), new POINT(c1x, c1y), new POINT(x, y) }, 3))
        throw new GdiOperationException("PolyBezierTo");
      return this;
    }

    public override IGfxCanvas EndPath()
    {
      Gdi.EndPath(_emf);
      if (!Gdi.StrokePath(_emf))
        throw new GdiOperationException("EndPath");
      return this;
    }

    public override IGfxCanvas Close()
    {
      if (_open)
      {
        if (_emf != Gdi.SafeHDC.Null)
        {
          Gdi.CloseEnhMetaFile(_emf);
        }
        _open = false;
      }
      return this;
    }

    public override void Dispose()
    {
      Close();
      if (_emf != Gdi.SafeHDC.Null)
      {
        Gdi.DeleteDC(_emf);
        _emf = Gdi.SafeHDC.Null;
      }
      if (_dc != HDC.NULL)
      {
        Gdi.DeleteDC(_dc);
        _dc = HDC.NULL;
      }
    }
  }

  public class GdiOperationException : Exception
  { public GdiOperationException(string operation) : base($"The GDI operation failed: {operation}") { } }
}
