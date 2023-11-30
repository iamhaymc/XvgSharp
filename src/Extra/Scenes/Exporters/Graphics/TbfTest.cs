using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FluentAssertions;

namespace Trane.Submittals.Pipeline.Tests
{
  [TestClass]
  public class TbfReaderTest
  {
    [TestMethod]
    public void Can_Parse_Master_Tbf_Entry()
    {
      string tbfZipPath = TestPath.GetPipelineOutputsPath("ASCXF000005.tbf");
      string tbfEntryPath = "ASCXF0~3.TBF";

      using (TbfReader tbfReader = TbfReader.FromEntry(tbfZipPath, tbfEntryPath))
      {
        var tags = tbfReader.YieldTags().ToArray();
        tags.Length.Should().Be(1672);
      }
    }

    [TestMethod]
    public void Can_Parse_Parts_Tbf_Entry()
    {
      string tbfZipPath = TestPath.GetPipelineOutputsPath("PL~PART15360-617a1a69-62b1-4532-86de-a6cdd66a3255.tbf");
      string tbfEntryPath = "TBF_DDP";

      using (TbfReader tbfReader = TbfReader.FromEntry(tbfZipPath, tbfEntryPath))
      {
        var tags = tbfReader.YieldTags().ToArray();
        tags.Length.Should().Be(433);
      }
    }

    [TestMethod]
    public void Master_Tbf_Entry_Contains_Notice_And_Warning()
    {
      string tbfZipPath = TestPath.GetPipelineOutputsPath("ASCXF000005.tbf");
      string tbfEntryPath = "ASCXF0~3.TBF";

      using (TbfReader tbfReader = TbfReader.FromEntry(tbfZipPath, tbfEntryPath))
      {
        var tags = tbfReader.YieldTags().Where(t => t.Type == TbfTagType.TR_STRING).ToArray();

        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("NOTICE"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("USE COPPER CONDUCTORS ONLY!"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("AVIS"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("N'UTILISER QUE DES CONDUCTEURS EN CUIVRE!"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("WARNING"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("HAZARDOUS VOLTAGE!"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("AVERTISSEMENT"));
        tags.Should().Contain(t => ((TbfStringTagToken)t).Data.Contains("TENSION DANGEREUSE!"));
      }
    }

    [TestMethod]
    public void Parts_Tbf_Contains_Header()
    {
      string tbfZipPath = TestPath.GetPipelineOutputsPath("PL~PART15360-617a1a69-62b1-4532-86de-a6cdd66a3255.tbf");
      string tbfEntryPath = "TBF_DDP";

      using (TbfReader tbfReader = TbfReader.FromEntry(tbfZipPath, tbfEntryPath))
      {
        var drawDefTag = tbfReader.ParseTag();
        Assert.AreEqual(TbfTagCode.TR_DRAWDEF, drawDefTag.Code);
        Assert.AreEqual(TbfTagType.TR_NODATA, drawDefTag.Type);
        Assert.AreEqual(null, ((TbfBinaryTagToken)drawDefTag).Data);

        var versionTag = tbfReader.ParseTag();
        Assert.AreEqual(TbfTagCode.TR_TBFVERSION, versionTag.Code);
        Assert.AreEqual(TbfTagType.TR_STRING, versionTag.Type);
        Assert.AreEqual("2.0", ((TbfStringTagToken)versionTag).Data);

        var dataExtentsTag = tbfReader.ParseTag();
        Assert.AreEqual(TbfTagCode.TR_EXTENTS, dataExtentsTag.Code);
        Assert.AreEqual(TbfTagType.TR_6REAL, dataExtentsTag.Type);
        ((TbfFloatTagToken)dataExtentsTag).Data.Should().BeEquivalentTo(new float[] { 0, 0, 0, 10, 10, 10 });

        var endDrawDefTag = tbfReader.ParseTag();
        Assert.AreEqual(TbfTagCode.TR_END, endDrawDefTag.Code);
        Assert.AreEqual(TbfTagType.TR_STRING, endDrawDefTag.Type);
        Assert.AreEqual("DRAWDEF", ((TbfStringTagToken)endDrawDefTag).Data);

        // TODO? ...
      }
    }
  }
}
