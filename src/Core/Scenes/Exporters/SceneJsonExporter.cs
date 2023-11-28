namespace Xvg;

public class SceneJsonExporter
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

public static class SceneJsonExporterExtensions
{
  public static FileInfo ToJsonFile(this Scene self, string path, bool indent = SceneJsonExporter.DefaultIndent, SceneJsonExporter writer = null)
  {
    return (writer ?? new SceneJsonExporter()).WriteJsonFile(self, path, indent);
  }

  public static string ToJsonString(this Scene self, bool indent = SceneJsonExporter.DefaultIndent, SceneJsonExporter writer = null)
  {
    return (writer ?? new SceneJsonExporter()).WriteJsonString(self, indent);
  }
}
