using System.Security.Cryptography;
using System.Text;

namespace Xvg;

public static class Mime
{
  public static bool IsBinary(string? mime)
   => !IsText(mime)
   || CommonBinaryTypes.Contains(mime);

  public static bool IsText(string? mime)
     => mime?.StartsWith("text/", StringComparison.InvariantCultureIgnoreCase) == true
     || CommonTextTypes.Contains(mime);

  public static string? FromExtension(string ext)
  {
    return ext switch
    {
      ".bin" => Bin,
      ".css" => Css,
      ".gif" => Gif,
      ".html" => Html,
      ".jpeg" => Jpeg,
      ".jpg" => Jpeg,
      ".json" => Json,
      ".png" => Png,
      ".svg" => Svg,
      ".txt" => Text,
      ".ttf" => Ttf,
      ".webp" => Webp,
      ".woff2" => Woff2,
      ".xml" => Xml,
      _ => throw new NotImplementedException($"Failure to resolve a MIME for the extension {ext}")
    };
  }

  public static string? ToExtension(string mime)
  {
    return mime switch
    {
      Bin => ".bin",
      Css => ".css",
      Gif => ".gif",
      Html => ".html",
      Jpeg => ".jpg",
      Json => ".json",
      Png => ".png",
      Svg => ".svg",
      Text => ".txt",
      Ttf => ".ttf",
      Webp => ".webp",
      Woff2 => ".woff2",
      Xml => ".xml",
      _ => throw new NotImplementedException($"Failure to resolve an extension for the MIME {mime}")
    };
  }

  #region [Constants]

  public const string Bin = "application/octet-stream";
  public const string Css = "text/css";
  public const string Gif = "image/gif";
  public const string Html = "text/html";
  public const string Jpeg = "image/jpeg";
  public const string Json = "application/json";
  public const string Png = "image/png";
  public const string Svg = "image/svg+xml";
  public const string Text = "text/plain";
  public const string Ttf = "font/ttf";
  public const string Webp = "image/webp";
  public const string Woff2 = "font/woff2";
  public const string Xml = "application/xml";

  static readonly string[] CommonBinaryTypes =
    { Bin,Png, Jpeg, Gif, Webp, Ttf, Woff2, };

  static readonly string[] CommonTextTypes =
    { Text, Json, Xml, Html, Css, Svg };

  #endregion
}

public static class Media
{
  public static IMedia FromFile(FileInfo file)
  {
    var mime = Mime.FromExtension(file.Extension) ?? Mime.Bin;
    return Mime.IsText(mime)
        ? TextMedia.FromFile(file, mime)
        : BinaryMedia.FromFile(file, mime);
  }

  public static IMedia FromDataUrlFile(FileInfo file)
  => FromDataUrl(file.ReadText());

  public static IMedia FromDataUrl(string url)
  {
    int datai = url.LastIndexOf(',');
    if (datai < 0)
      throw new ArgumentException("Failure to parse data URL because it is invalid");
    string head = url[5..datai].ToLower();
    int metai = head.IndexOf(';');
    if (metai < 0)
      throw new ArgumentException("Failure to parse data URL because it is invalid");
    string mime = head[..metai].ToLower();
    string data = url.Substring(datai + 1, url.Length - datai - 1);
    return head.Contains("base64")
      ? new BinaryMedia(Convert.FromBase64String(data), mime)
      : new TextMedia(data, mime);
  }

  public static bool HasFileUri(string url)
    => url.StartsWith("file:");

  public static bool HasDataUri(string url)
    => url.StartsWith("data:");

  public static BinaryMedia? AsBinary(this IMedia self)
     => self.Kind == MediaKind.Binary ? (BinaryMedia)self : null;

  public static TextMedia? AsText(this IMedia self)
     => self.Kind == MediaKind.Text ? (TextMedia)self : null;
}

public enum MediaKind
{
  Binary, Text
}

public interface IMedia
{
  MediaKind Kind { get; }
  string Type { get; }
  string Prefix { get; }
  Lazy<string> Hash { get; }
  FileInfo? File { get; }
  byte[] ToBytes();
  string? ToString();
  string ToDataUrl();
}

public abstract class MediaBase<D> : IMedia
{
  public abstract MediaKind Kind { get; }
  public string Type { get; protected set; }
  public string Prefix { get; protected set; }
  public D Data { get; protected set; }
  public Lazy<string> Hash { get; protected set; }
  public FileInfo? File { get; protected set; }

  public MediaBase(D data, string mime, FileInfo? file)
  {
    Data = data;
    Hash = new Lazy<string>(CreateHash);
    Type = mime;
    Prefix = CreatePrefix(mime);
    UseFile(file);
  }

  protected string CreateHash()
     => Convert.ToHexString(SHA256.HashData(ToBytes()));

  protected abstract string CreatePrefix(string mime);

  public abstract byte[] ToBytes();

  public string ToDataUrl()
     => Prefix + ToString();

  public MediaBase<D> UseFile(FileInfo? file)
  {
    File = file;
    return this;
  }

  public FileInfo? ToBinaryFile()
     => File?.WriteBytes(ToBytes());

  public FileInfo? ToTextFile()
     => File?.WriteText(ToString()!);

  public FileInfo? ToDataUrlFile()
     => File?.WriteText(ToDataUrl());
}

public class BinaryMedia : MediaBase<Stream>
{
  public static BinaryMedia FromFile(FileInfo file, string? mime = null)
  {
    mime ??= (Mime.FromExtension(file.Extension) ?? Mime.Bin);
    return new BinaryMedia(file.ReadBytes(), mime, file);
  }

  public override MediaKind Kind => MediaKind.Binary;

  public BinaryMedia(Stream data, string mime = Mime.Bin, FileInfo? file = null)
      : base(data, mime, file)
  { }
  public BinaryMedia(byte[] data, string mime = Mime.Bin, FileInfo? file = null)
      : base(new MemoryStream(data), mime, file)
  { }

  protected override string CreatePrefix(string mime)
      => $"data:{mime};base64,";

  public override byte[] ToBytes()
  {
    using var buffer = new MemoryStream();
    Data.CopyTo(buffer);
    return buffer.ToArray();
  }

  public override string ToString()
     => Convert.ToBase64String(ToBytes());
}

public class TextMedia : MediaBase<string>
{
  public static TextMedia FromFile(FileInfo file, string? mime = null)
  {
    mime ??= (Mime.FromExtension(file.Extension) ?? Mime.Bin);
    return new TextMedia(file.ReadText(), mime, file);
  }

  public override MediaKind Kind => MediaKind.Text;

  public TextMedia(string data, string mime = Mime.Text, FileInfo? file = null)
      : base(data, mime, file)
  { }

  protected override string CreatePrefix(string mime)
      => $"data:{mime},";

  public override byte[] ToBytes()
      => Encoding.UTF8.GetBytes(Data);

  public override string ToString()
     => Data;
}
