using System.Text;

namespace Xvg;

public partial class VgTextWriter
{
  public StringBuilder Buffer { get; set; } = new StringBuilder();

  public VgTextWriter Reset()
  {
    Buffer ??= new StringBuilder();
    Buffer.Clear();
    return this;
  }

  public VgTextWriter Add(string value)
  {
    if (!string.IsNullOrEmpty(value))
      Buffer?.Append(value);
    return this;
  }

  public VgTextWriter AddLine(string value)
  {
    if (!string.IsNullOrEmpty(value))
      Buffer?.AppendLine(value);
    return this;
  }

  public VgTextWriter AddFile(string path)
  {
    string text = File.ReadAllText(path);
    if (!string.IsNullOrEmpty(text))
      Buffer?.AppendLine(text);
    return this;
  }

  public VgTextWriter ToFile(string path)
  {
    File.WriteAllText(path, Buffer?.ToString());
    return this;
  }

  public string Dump()
  {
    string text = Buffer?.ToString();
    Buffer?.Clear();
    return text;
  }

  public override string ToString()
  {
    return Buffer?.ToString();
  }
}
