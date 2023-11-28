namespace Xvg;

public class SceneJsonImporter
{
  public Scene ReadJsonFile(string path)
  {
    return FsFile.JsonIn<Scene>(path);
  }

  public Scene ReadJsonString(string json)
  {
    return Json.FromJson<Scene>(json);
  }
}
