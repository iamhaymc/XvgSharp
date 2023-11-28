using System.Text;

namespace Xvg;

public partial class TextWriter
{
  public StringBuilder Buffer { get; set; } = new StringBuilder();

  public TextWriter Reset()
  {
    Buffer ??= new StringBuilder();
    Buffer.Clear();
    return this;
  }

  public TextWriter Add(string value)
  {
    if (!string.IsNullOrEmpty(value))
      Buffer?.Append(value);
    return this;
  }

  public TextWriter AddLine(string value)
  {
    if (!string.IsNullOrEmpty(value))
      Buffer?.AppendLine(value);
    return this;
  }

  public TextWriter AddFile(string path)
  {
    string text = File.ReadAllText(path);
    if (!string.IsNullOrEmpty(text))
      Buffer?.AppendLine(text);
    return this;
  }

  public TextWriter ToFile(string path)
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
