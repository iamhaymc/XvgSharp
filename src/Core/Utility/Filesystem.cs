using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Xvg;

public static class FsPath
{
  public static string Join(params string[] paths)
  {
    return Path.Combine(paths);
  }

  public static string[] Split(string path)
  {
    return path.Split(Path.DirectorySeparatorChar);
  }

  public static string Resolve(params string[] paths)
  {
    return Path.GetFullPath(Path.Combine(paths));
  }

  public static string Relative(string fromPath, string toPath)
  {
    Uri fromUri = new Uri(fromPath);
    Uri toUri = new Uri(toPath);
    if (fromUri.Scheme != toUri.Scheme) return toPath;
    Uri relativeUri = fromUri.MakeRelativeUri(toUri);
    string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
    if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
      relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    return relativePath;
  }

  private static string Normalize(string path, bool isDirectory = false)
  {
    if (!string.IsNullOrEmpty(path))
    {
      path = path.Trim().Replace(Path.DirectorySeparatorChar, '/');
      if (isDirectory && path.Last() != Path.DirectorySeparatorChar)
        path += Path.DirectorySeparatorChar;
    }
    return path;
  }

  public static string NormalizeFile(string path) => Normalize(path, isDirectory: false);

  public static string NormalizeDir(string path) => Normalize(path, isDirectory: true);

  /// <summary>
  /// Gets the parent directory path
  /// </summary>
  public static string GetParent(string path)
  {
    return Path.GetDirectoryName(path);
  }

  /// <summary>
  /// Gets the file name with the extension
  /// </summary>
  public static string GetName(string path)
  {
    return Path.GetFileName(path);
  }

  /// <summary>
  /// Gets the file name without the extension
  /// </summary>
  public static string GetStem(string path)
  {
    return Path.GetFileNameWithoutExtension(path);
  }

  /// <summary>
  /// Gets the file extension
  /// </summary>
  public static string GetExtension(string path)
  {
    return Path.GetExtension(path);
  }

  public static string WithParent(string path, string parent)
  {
    return Path.Combine(parent, Path.GetFileName(path));
  }

  public static string WithName(string path, string name)
  {
    return Path.Combine(Path.GetDirectoryName(path), name);
  }

  public static string WithStem(string path, string stem)
  {
    return Path.Combine(Path.GetDirectoryName(path), stem + Path.GetExtension(path));
  }

  public static string WithExtension(string path, string suffix)
  {
    return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + suffix);
  }

  public static string GetUniquePathStem(string prefix = "")
  {
    return prefix + Guid.NewGuid().ToString();
  }

  public static string GetUniqueFilePath(string stem, string extension, string parent = null, string prefix = "")
  {
    if (!extension.StartsWith(".")) extension = "." + extension;
    string path = $"{prefix}{stem}-{Guid.NewGuid()}{extension}";
    return !string.IsNullOrEmpty(parent) ? Path.Combine(parent, path) : path;
  }

  public static string GetUniqueTempDirPath(string prefix = "")
  {
    return Path.Combine(Path.GetTempPath(), GetUniquePathStem(prefix));
  }
}

public static class FsFile
{
  private const int DEFAULT_BUFFER_SIZE = 1024;

  public static bool Test(string path)
  {
    return File.Exists(path);
  }

  public static void Move(string fromPath, string toPath)
  {
    File.Move(fromPath, toPath);
  }

  public static void Copy(string fromPath, string toPath)
  {
    Copy(new FileInfo(fromPath), new FileInfo(toPath));
  }

  public static void Copy(FileInfo fromPath, FileInfo toPath)
  {
    File.Copy(fromPath.FullName, toPath.FullName, overwrite: true);
  }

  /// <summary>
  /// Deletes the file if it exists
  /// </summary>
  public static void Delete(string path)
  {
    if (File.Exists(path))
      File.Delete(path);
  }

  public static long Measure(string path)
  {
    return new FileInfo(path).Length;
  }

  /// <summary>
  /// Computes the SHA 256 hash of the file
  /// </summary>
  public static string Hash(string path)
  {
    using (FileStream inputStream = StreamIn(path))
      return Xvg.Hash.GetSha256(inputStream);
  }

  /// <summary>
  /// Creates the parent directory if it doesn't exist
  /// </summary>
  public static string Ensure(string path)
  {
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    return path;
  }

  /// <summary>
  /// Returns an input file stream
  /// </summary>
  public static FileStream StreamIn(string path)
  {
    return File.OpenRead(path);
  }

  /// <summary>
  /// Returns an output file stream
  /// (The parent directory is ensured)
  /// </summary>
  public static FileStream StreamOut(string path)
  {
    return File.OpenWrite(Ensure(path));
  }

  /// <summary>
  /// Returns a binary file reader
  /// </summary>
  public static BinaryReader BinaryIn(string path)
  {
    return new BinaryReader(StreamIn(path), Encoding.Default, leaveOpen: false);
  }

  /// <summary>
  /// Returns a binary file writer
  /// (The parent directory is ensured)
  /// </summary>
  public static BinaryWriter BinaryOut(string path)
  {
    return new BinaryWriter(File.OpenWrite(Ensure(path)), Encoding.Default, leaveOpen: false);
  }

  /// <summary>
  /// Returns a text file reader
  /// </summary>
  public static StreamReader TextIn(string path)
  {
    return new StreamReader(StreamIn(path), Encoding.UTF8, detectEncodingFromByteOrderMarks: true, DEFAULT_BUFFER_SIZE, leaveOpen: false);
  }

  /// <summary>
  /// Returns a text file writer
  /// (The parent directory is ensured)
  /// </summary>
  public static StreamWriter TextOut(string path)
  {
    return new StreamWriter(File.OpenWrite(Ensure(path)), Encoding.UTF8, DEFAULT_BUFFER_SIZE, leaveOpen: false);
  }

  /// <summary>
  /// Reads all text from a file
  /// </summary>
  public static string StringIn(string path)
  {
    return File.ReadAllText(path);
  }

  /// <summary>
  /// Writes all text to file
  /// (The parent directory is ensured)
  /// </summary>
  public static void StringOut(string path, string data)
  {
    File.WriteAllText(Ensure(path), data);
  }

  /// <summary>
  /// Returns a specific model if JSON from a file
  /// </summary>
  public static T JsonIn<T>(string path)
  {
    JsonSerializerOptions options = Json.IndentByDefault ? Json.IndentedJsonOptions : Json.UnindentedJsonOptions;
    using (FileStream jsonStream = StreamIn(path))
      return JsonSerializer.Deserialize<T>(jsonStream, options);
  }

  /// <summary>
  /// Returns an generic model of JSON from a file
  /// </summary>
  public static JsonNode JsonIn(string path)
  {
    using (FileStream jsonStream = StreamIn(path))
      return JsonNode.Parse(jsonStream);
  }

  /// <summary>
  /// Writes JSON of a model to a file
  /// (The parent directory is ensured)
  /// </summary>
  public static void JsonOut<T>(string path, T data, bool indent = Json.IndentByDefault)
  {
    JsonSerializerOptions options = indent ? Json.IndentedJsonOptions : Json.UnindentedJsonOptions;
    using (FileStream jsonStream = StreamOut(Ensure(path)))
    using (Utf8JsonWriter jsonWriter = new Utf8JsonWriter(jsonStream))
      JsonSerializer.Serialize(jsonWriter, data, options);
  }
}

public static class FsDir
{
  private const int MAX_CREATE_ATTEMPTS = 3;

  public static bool Test(string path)
  {
    return Directory.Exists(path);
  }

  public static void Ensure(string path)
  {
    Directory.CreateDirectory(path);
  }

  public static void Move(string fromPath, string toPath)
  {
    Directory.Move(fromPath, toPath);
  }

  public static void Copy(string fromPath, string toPath)
  {
    Copy(new DirectoryInfo(fromPath), new DirectoryInfo(toPath));
  }

  public static void Copy(DirectoryInfo fromPath, DirectoryInfo toPath)
  {
    Directory.CreateDirectory(toPath.FullName);
    foreach (FileInfo fi in fromPath.GetFiles())
      fi.CopyTo(Path.Combine(toPath.FullName, fi.Name), true);
    foreach (DirectoryInfo di in fromPath.GetDirectories())
      Copy(di, toPath.CreateSubdirectory(di.Name));
  }

  /// <summary>
  /// Deletes the directory if it exists
  /// </summary>
  public static void Delete(string path)
  {
    Directory.Delete(path);
  }

  public static DirectoryInfo CreateTemporary(string prefix = "")
  {
    int attempts = 0;
    string path = FsPath.GetUniqueTempDirPath(prefix);
    for (; attempts < MAX_CREATE_ATTEMPTS && !Test(path); ++attempts)
      path = FsPath.GetUniqueTempDirPath(prefix);
    if (attempts >= MAX_CREATE_ATTEMPTS)
      new Exception("Failure to create unique temporary directory");
    Directory.CreateDirectory(path);
    return new DirectoryInfo(path);
  }

  public static IEnumerable<FileInfo> FindFiles(string path, string namePattern = "*")
    => Directory.GetFiles(path, namePattern, SearchOption.TopDirectoryOnly).Select(x => new FileInfo(x));

  public static IEnumerable<FileInfo> FindAllFiles(string path, string namePattern = "*")
    => Directory.EnumerateFiles(path, namePattern, SearchOption.AllDirectories).Select(x => new FileInfo(x));

  public static IEnumerable<DirectoryInfo> FindDirs(string path, string namePattern = "*")
    => Directory.GetDirectories(path, namePattern, SearchOption.TopDirectoryOnly).Select(x => new DirectoryInfo(x));

  public static IEnumerable<DirectoryInfo> FindAllDirs(string path, string namePattern = "*")
    => Directory.EnumerateDirectories(path, namePattern, SearchOption.AllDirectories).Select(x => new DirectoryInfo(x));

  public static IEnumerable<FileSystemInfo> FindItems(string path, string namePattern = "*")
    => Directory.GetFileSystemEntries(path, namePattern, SearchOption.TopDirectoryOnly)
    .Select(x => File.Exists(x) ? (FileSystemInfo)new DirectoryInfo(x) : new FileInfo(x));

  public static IEnumerable<FileSystemInfo> FindAllItems(string path, string namePattern = "*")
    => Directory.GetFileSystemEntries(path, namePattern, SearchOption.AllDirectories)
    .Select(x => File.Exists(x) ? (FileSystemInfo)new DirectoryInfo(x) : new FileInfo(x));
}

public static class FileSystemInfoExtensions
{
  public static bool IsDir(this FileSystemInfo self)
    => self.GetType() == typeof(DirectoryInfo);

  public static DirectoryInfo AsDir(this FileSystemInfo self)
    => (DirectoryInfo)self;

  public static DirectoryInfo? TryAsDir(this FileSystemInfo self)
    => self.IsDir() ? (DirectoryInfo)self : null;

  public static bool IsFile(this FileSystemInfo self)
    => self.GetType() == typeof(FileInfo);

  public static FileInfo AsFile(this FileSystemInfo self)
    => (FileInfo)self;

  public static FileInfo TryAsFile(this FileSystemInfo self)
    => self.IsFile() ? (FileInfo)self : null;

  public static DirectoryInfo JoinDir(this FileSystemInfo self, params string[] parts)
    => new DirectoryInfo(Path.Combine(new[] { self.FullName }.Concat(parts).ToArray()));

  public static FileInfo JoinFile(this FileSystemInfo self, params string[] parts)
    => new FileInfo(Path.Combine(new[] { self.FullName }.Concat(parts).ToArray()));
}

public static class DirectoryInfoExtensions
{
  public static DirectoryInfo TryEnsure(this DirectoryInfo self)
  {
    if (!self.Exists)
      self.Create();
    return self;
  }

  public static DirectoryInfo TryDelete(this DirectoryInfo self)
  {
    if (self.Exists)
      self.Delete(true);
    return self;
  }
}

public static class FileInfoExtensions
{
  public static byte[] ReadBytes(this FileInfo self)
     => File.ReadAllBytes(self.FullName);

  public static FileInfo WriteBytes(this FileInfo self, byte[] data)
  {
    self.Directory?.TryEnsure();
    File.WriteAllBytes(self.FullName, data);
    return self;
  }

  public static string ReadText(this FileInfo self)
     => File.ReadAllText(self.FullName);

  public static FileInfo WriteText(this FileInfo self, string data)
  {
    self.Directory?.TryEnsure();
    File.WriteAllText(self.FullName, data);
    return self;
  }
}

  public interface IFileRepo
  {
    Task<bool> Test(string path);
    Task Delete(string path);
    Task<Stream> StreamIn(string path);
    Task StreamOut(string path, Stream data);
    Task<string> TextIn(string path);
    Task TextOut(string path, string data);
    //Task<bool> ZipEntryIn(string zipPath, string entryName, Stream entryData);
    //Task ZipEntryOut(string zipPath, string entryName, string entryPath);
    //Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, string>> entryPathMap);
    //Task ZipEntryOut(string zipPath, string entryName, Stream entryData);
    //Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, Stream>> entryDataMap);
  }

  public class LocalFileRepo : IFileRepo
  {
    public Task<bool> Test(string path)
    {
      return Task.FromResult(FsFile.Test(path));
    }

    public Task Delete(string path)
    {
      FsFile.Delete(path);
      return Task.CompletedTask;
    }

    public Task<Stream> StreamIn(string path)
    {
      return Task.FromResult((Stream)FsFile.StreamIn(path));
    }

    public Task StreamOut(string path, Stream data)
    {
      using (var ostream = FsFile.StreamOut(path))
        data.CopyTo(ostream);
      return Task.CompletedTask;
    }

    public async Task<string> TextIn(string path)
    {
      using (var fstream = new StreamReader(await StreamIn(path)))
        return fstream.ReadToEnd();
    }

    public Task TextOut(string path, string data)
    {
      using (var buffer = new MemoryStream(Encoding.UTF8.GetBytes(data)))
        return StreamOut(path, buffer);
    }

    //public Task<bool> ZipEntryIn(string zipPath, string entryName, Stream entryData)
    //{
    //  return Task.FromResult(FsZip.EntryIn(zipPath, entryName, entryData));
    //}

    //public Task ZipEntryOut(string zipPath, string entryName, string entryPath)
    //{
    //  FsZip.EntryOut(zipPath, entryName, entryPath);
    //  return Task.CompletedTask;
    //}

    //public Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, string>> entryPathMap)
    //{
    //  FsZip.EntryOut(zipPath, entryPathMap);
    //  return Task.CompletedTask;
    //}

    //public Task ZipEntryOut(string zipPath, string entryName, Stream entryData)
    //{
    //  FsZip.EntryOut(zipPath, entryName, entryData);
    //  return Task.CompletedTask;
    //}

    //public Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, Stream>> entryDataMap)
    //{
    //  FsZip.EntryOut(zipPath, entryDataMap);
    //  return Task.CompletedTask;
    //}
  }