namespace Xvg;

public class SceneEmfWriter
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
  public static FileInfo ToEmfFile(this Scene self, string path, SceneEmfWriter writer = null)
  {
    return (writer ?? new SceneEmfWriter()).WriteEmfFile(self, path);
  }

  public static Stream ToEmfStream(this Scene self, Stream stream, SceneEmfWriter writer = null)
  {
    return (writer ?? new SceneEmfWriter()).WriteEmfStream(self, stream);
  }
}
