namespace Xvg;

public class PathJsonImporter
{
  public Path ReadJsonFile(string fspath)
  {
    return FsFile.JsonIn<Path>(fspath);
  }

  public Path ReadJsonString(string json)
  {
    return Json.FromJson<Path>(json);
  }
}
