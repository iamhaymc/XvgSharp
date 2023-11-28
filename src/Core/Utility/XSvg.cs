using System.Xml.Linq;

namespace Xvg;

public static class XSvgDocument
{
  public static readonly XNamespace SvgNamespace = @"http://www.w3.org/2000/svg";
  public static readonly string SvgVersion = "1.1";

  public static XDocument Create()
  {
    XElement root = XSvgElement.Create("svg")
      .SetSvgAttribute("version", SvgVersion)
      .SetSvgAttribute("xmlns", SvgNamespace);
    return new XDocument(root);
  }

  public static XDocument SetSvgAttribute(this XDocument self, string key, object value)
  {
    self.Root.SetSvgAttribute(key, value);
    return self;
  }

  public static XDocument SetSvgTextContent(this XDocument self, string text)
  {
    self.Root.SetSvgTextContent(text);
    return self;
  }

  public static XElement NewSvgElement(this XDocument self, string childTag)
  {
    return self.Root.NewSvgElement(childTag);
  }
}

public static class XSvgElement
{
  public static XElement Create(string tag)
  {
    return new XElement(XSvgDocument.SvgNamespace + tag);
  }

  public static XElement SetSvgAttribute(this XElement self, string key, object value)
  {
    self.SetAttributeValue(key, value);
    return self;
  }

  public static XElement SetSvgTextContent(this XElement self, string text)
  {
    self.Value = text;
    return self;
  }

  public static XElement NewSvgElement(this XElement self, string tag)
  {
    XElement child = Create(tag);
    self.Add(child);
    return child;
  }
}
