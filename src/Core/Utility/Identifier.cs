namespace Xvg;

public static class Identifier
{
  public static string NextId
    => "id_" + Guid.NewGuid().ToString().Replace("-", "");
}
