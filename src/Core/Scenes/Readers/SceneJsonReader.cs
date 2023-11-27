namespace Xvg;

public class SceneJsonReader
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
