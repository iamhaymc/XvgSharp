namespace Xvg;

public class PathJsonExporter
{
  public const bool DefaultIndent = true;

  public FileInfo WriteJsonFile(Path vgpath, string fspath, bool indent = DefaultIndent)
  {
    FsFile.JsonOut<Path>(fspath, vgpath, indent);
    return new FileInfo(fspath);
  }

  public string WriteJsonString(Path vgpath, bool indent = DefaultIndent)
  {
    throw new NotImplementedException();
  }
}

public static class PathJsonExporterExtensions
{
  public static FileInfo ToJsonFile(this Path self, string fspath, bool indent = PathJsonExporter.DefaultIndent, PathJsonExporter writer = null)
  {
    return (writer ?? new PathJsonExporter()).WriteJsonFile(self, fspath, indent);
  }

  public static string ToJsonString(this Path self, bool indent = PathJsonExporter.DefaultIndent, PathJsonExporter writer = null)
  {
    return (writer ?? new PathJsonExporter()).WriteJsonString(self, indent);
  }
}
