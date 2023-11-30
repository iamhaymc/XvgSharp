using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vanara.PInvoke;
using static Vanara.PInvoke.Gdi32;

namespace Trane.Submittals.Pipeline.Tests
{
  [TestClass]
  public class GdiInvokeTest
  {
    enum SizeUnit
    {
      Millis,
      Inches,
      Pixels
    }

    [TestMethod]
    public void Can_Create_Metafile()
    {

    }

    [TestMethod]
    public void Can_Read_Emf_File()
    {
      using (var mfdc = Gdi32.CreateEnhMetaFile(HDC.NULL, "example.emf", new Vanara.PInvoke.PRECT(0, 0, 640, 480), "My Description!"))
      {
        Gdi32.SetViewportOrgEx(mfdc, -50, -50, out var lastPoint);
        Gdi32.CloseEnhMetaFile(mfdc);
      }

      using var bin = FsFile.BinaryIn("example.emf");
      //using var bin = FsFile.BinaryIn(TestPath.GetAhusubOutputsPath("CSAA_199790100.emf"));

      bin.ReadUInt32().Should().Be((UInt32)EmfRecordType.EMR_HEADER);

      uint MINIMUM_VALID_SIZE = 88;
      uint MINIMUM_VALID_SIZE_1 = 100;
      uint MINIMUM_VALID_SIZE_2 = 108;

      uint headerSize = MINIMUM_VALID_SIZE; // Minimum valid size

      var size = bin.ReadUInt32();
      if (size < MINIMUM_VALID_SIZE)
        throw new Exception("Header size is not minimum valid size");
      else
        headerSize = size;

      var boundsLeft = bin.ReadUInt32();
      var boundsTop = bin.ReadUInt32();
      var boundsRight = bin.ReadUInt32();
      var boundsBottom = bin.ReadUInt32();
      var frameLeft = bin.ReadUInt32();
      var frameTop = bin.ReadUInt32();
      var frameRight = bin.ReadUInt32();
      var frameBottom = bin.ReadUInt32();
      var recordSig = bin.ReadUInt32(); recordSig.Should().Be(0x464D4520);
      var version = bin.ReadUInt32();
      var bytes = bin.ReadUInt32();
      var records = bin.ReadUInt32();
      var handles = bin.ReadUInt16();
      var reserved = bin.ReadUInt16(); reserved.Should().Be(0x0000);
      var nDescription = bin.ReadUInt32();
      var offDescription = bin.ReadUInt32();
      var nPalEntries = bin.ReadUInt32();
      var deviceWidth = bin.ReadUInt32();
      var deviceHeight = bin.ReadUInt32();
      var millisWidth = bin.ReadUInt32();
      var millisHeight = bin.ReadUInt32();
      UInt32? cbPixelFormat = null;
      UInt32? offPixelFormat = null;
      UInt32? bOpenGL = null;
      UInt32? macrometersX = null;
      UInt32? macrometersY = null;
      string description = null;

      if (offDescription >= MINIMUM_VALID_SIZE && (offDescription + (nDescription * 2) <= size))
        headerSize = offDescription;

      if (headerSize >= MINIMUM_VALID_SIZE_1) // Extension 1
      {
        cbPixelFormat = bin.ReadUInt32();
        offPixelFormat = bin.ReadUInt32();
        bOpenGL = bin.ReadUInt32();

        if (offPixelFormat >= 100 && (offPixelFormat + cbPixelFormat <= size) && offPixelFormat < headerSize)
          headerSize = offPixelFormat.Value;
      }

      if (headerSize >= MINIMUM_VALID_SIZE_2) // Extension 2
      {
        macrometersX = bin.ReadUInt32();
        macrometersY = bin.ReadUInt32();
      }

      bool hasDescription = offDescription > 0 && nDescription > 0;
      if (hasDescription && offDescription == bin.BaseStream.Position)
      {
        // Read all chars including the terminator and padding
        // (each char is 2 bytes for UTF-16 LE)
        var dbytes = bin.ReadBytes((int)((nDescription + 1) * 2));
        description = System.Text.Encoding.Unicode.GetString(dbytes).Trim();
      };

      // TODO: read desc using offset and count

      bool hasPixelFormat = offPixelFormat > 0 && cbPixelFormat > 0;
      // TODO: read desc using offset and count

      for (int i = 0; i < records - 1; ++i) // header counts as a record
      {
        var recordType = (EmfRecordType)bin.ReadUInt32();
      }

      Assert.IsTrue(true);
    }
  }
}
