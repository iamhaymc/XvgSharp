using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Trane.Submittals.Pipeline.Tests
{
  [TestClass]
  public class GdiTest
  {
    [TestMethod]
    public void Can_Create_Emf_File()
    {
      using (GdiCanvas canvas = new GdiCanvas(TestPath.GetTestResultsPath("Can_Create_Emf_File.emf"), GfxSizeL.Sq512.WithUom(GfxSizeUnit.Pixels), "A simple enhanced metafile!"))
      {

        using var pen = canvas.CreatePen(255, 0, 0, 10);
        canvas.UsePen(pen);

        canvas.StartPath();
        canvas.MoveTo(512, 0);
        canvas.LineTo(0, 512);
        canvas.LineTo(0, 0);
        canvas.EndPath();
      }
    }
  }
}
