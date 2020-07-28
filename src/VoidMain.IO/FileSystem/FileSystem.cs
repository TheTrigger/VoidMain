using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidMain.IO.FileSystem
{
    public class FileSystem : IFileSystem
    {
        private const int DefaultBufferSize = 4096;

        public string CurrentDirectory
        {
            get => Directory.GetCurrentDirectory();
            set => Directory.SetCurrentDirectory(value);
        }

        public bool FileExists(string filePath)
            => File.Exists(filePath);

        public bool DirectoryExists(string dirPath)
            => Directory.Exists(dirPath);

        public long GetFileSize(string filePath)
            => new FileInfo(filePath).Length;

        public DateTime GetLastModifiedTime(string path)
            => File.GetLastWriteTime(path);

        public DateTime GetLastModifiedTimeUtc(string path)
            => File.GetLastWriteTimeUtc(path);

        public Stream CreateFile(string filePath)
            => File.Create(filePath);

        public void CreateDirectory(string dirPath)
            => Directory.CreateDirectory(dirPath);

        public void Move(string sourcePath, string destPath)
            => Directory.Move(sourcePath, destPath);

        public void DeleteFile(string filePath)
            => File.Delete(filePath);

        public void DeleteDirectory(string dirPath, bool recursive = false)
            => Directory.Delete(dirPath, recursive);

        public string ReadAllText(string filePath, Encoding? encoding = null)
            => File.ReadAllText(filePath, encoding ?? Encoding.UTF8);

        public string[] ReadAllLines(string filePath, Encoding? encoding = null)
            => File.ReadAllLines(filePath, encoding ?? Encoding.UTF8);

        public void WriteAllText(string filePath, string text, Encoding? encoding = null)
            => File.WriteAllText(filePath, text, encoding ?? Encoding.UTF8);

        public void WriteAllLines(string filePath, IEnumerable<string> lines, Encoding? encoding = null)
            => File.WriteAllLines(filePath, lines, encoding ?? Encoding.UTF8);

        public Stream OpenRead(string filePath) => DirectoryExists(filePath)
            ? throw StreamForDirectory(filePath)
            : CreateStream(filePath, FileAccess.Read);

        public Stream OpenWrite(string filePath) => DirectoryExists(filePath)
            ? throw StreamForDirectory(filePath)
            : CreateStream(filePath, FileAccess.Write);

        private static FileStream CreateStream(string filePath, FileAccess fileAccess)
            => new FileStream(filePath, FileMode.Open, fileAccess,
                FileShare.ReadWrite, DefaultBufferSize, FileOptions.Asynchronous);

        private static Exception StreamForDirectory(string path)
            => new InvalidOperationException($"Unable to create a stream for the directory '{path}'.");
    }
}
