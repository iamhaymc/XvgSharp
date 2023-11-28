namespace Xvg;

public class SceneEmfExporter
{
  public FileInfo WriteEmfFile(Scene scene, string path)
  {
    using FileStream stream = FsFile.StreamOut(path);
    WriteEmfStream(scene, stream);
    return new FileInfo(path);
  }

  public Stream WriteEmfStream(Scene scene, Stream output)
  {
    throw new NotImplementedException();
  }
}

public static class SceneEmfWriterExtensions
{
  public static FileInfo ToEmfFile(this Scene self, string path, SceneEmfExporter writer = null)
  {
    return (writer ?? new SceneEmfExporter()).WriteEmfFile(self, path);
  }

  public static Stream ToEmfStream(this Scene self, Stream stream, SceneEmfExporter writer = null)
  {
    return (writer ?? new SceneEmfExporter()).WriteEmfStream(self, stream);
  }
}
