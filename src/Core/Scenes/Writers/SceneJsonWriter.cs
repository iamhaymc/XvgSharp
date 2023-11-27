namespace Xvg;

public class SceneJsonWriter
{
  public const bool DefaultIndent = true;

  public FileInfo WriteJsonFile(Scene scene, string path, bool indent = DefaultIndent)
  {
    FsFile.JsonOut<Scene>(path, scene, indent);
    return new FileInfo(path);
  }

  public string WriteJsonString(Scene scene, bool indent = DefaultIndent)
  {
    throw new NotImplementedException();
  }
}

public static class SceneJsonWriterExtensions
{
  public static FileInfo ToJsonFile(this Scene self, string path, bool indent = SceneJsonWriter.DefaultIndent, SceneJsonWriter writer = null)
  {
    return (writer ?? new SceneJsonWriter()).WriteJsonFile(self, path, indent);
  }

  public static string ToJsonString(this Scene self, bool indent = SceneJsonWriter.DefaultIndent, SceneJsonWriter writer = null)
  {
    return (writer ?? new SceneJsonWriter()).WriteJsonString(self, indent);
  }
}
