using System.IO;

namespace Xvg
{
  public class EmfWriter : GfxWriter
  {
    private readonly BinaryWriter _writer;

    public EmfWriter(BinaryWriter writer)
    {
      _writer = writer;
    }

    public EmfWriter WriteHeader(EmfHeader header)
    {
      // TODO: write header size
      _writer.Write(header.Bounds.Left);
      _writer.Write(header.Bounds.Top);
      _writer.Write(header.Bounds.Right);
      _writer.Write(header.Bounds.Bottom);
      _writer.Write(header.Frame.Left);
      _writer.Write(header.Frame.Top);
      _writer.Write(header.Frame.Right);
      _writer.Write(header.Frame.Bottom);
      _writer.Write(header.Signature);
      _writer.Write(header.Version);
      _writer.Write(header.Bytes);
      _writer.Write(header.Records);
      _writer.Write(header.Handles);
      _writer.Write(header.Reserved);
      _writer.Write(header.Description);
      _writer.Write(header.OffDescription);
      _writer.Write(header.PalEntries);
      _writer.Write(header.Device.W);
      _writer.Write(header.Device.H);
      _writer.Write(header.Millimeters.W);
      _writer.Write(header.Millimeters.H);
      _writer.Write(header.PixelFormat);
      _writer.Write(header.OffPixelFormat);
      _writer.Write(header.OpenGL);
      _writer.Write(header.Micrometers.W);
      _writer.Write(header.Micrometers.H);
      // TODO: write description
      // TODO: write pixelformat
      return this;
    }
  }
}
