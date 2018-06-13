using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidMain.Hosting.Environment
{
    public class PhisicalFileSystem : IFileSystem
    {
        private const int DefaultBufferSize = 4096;

        public bool Exists(string path) => FileExists(path) || DirectoryExists(path);
        public bool FileExists(string filePath) => File.Exists(filePath);
        public bool DirectoryExists(string dirPath) => Directory.Exists(dirPath);

        public long GetFileSize(string filePath) => new FileInfo(filePath).Length;

        public DateTime GetLastModifiedTime(string path) => Exists(path)
            ? File.GetLastWriteTime(path)
            : throw FileNotFound(path);

        public DateTime GetLastModifiedTimeUtc(string path) => Exists(path)
            ? File.GetLastWriteTimeUtc(path)
            : throw FileNotFound(path);

        public void CreateFile(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (!String.IsNullOrEmpty(dirPath))
            {
                CreateDirectory(dirPath);
            }
            File.Create(filePath).Dispose();
        }

        public void CreateDirectory(string dirPath)
        {
            Directory.CreateDirectory(dirPath);
        }

        public void Move(string sourcePath, string destPath)
        {
            // Directory.Move() supports both file and directory
            Directory.Move(sourcePath, destPath);
        }

        public void Delete(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
            else if (DirectoryExists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }

        public string ReadAllText(string filePath, Encoding encoding = null)
        {
            return File.ReadAllText(filePath, encoding ?? Encoding.UTF8);
        }

        public string[] ReadAllLines(string filePath, Encoding encoding = null)
        {
            return File.ReadAllLines(filePath, encoding ?? Encoding.UTF8);
        }

        public void WriteAllText(string filePath, string text, Encoding encoding = null)
        {
            File.WriteAllText(filePath, text, encoding ?? Encoding.UTF8);
        }

        public void WriteAllLines(string filePath, IEnumerable<string> lines, Encoding encoding = null)
        {
            File.WriteAllLines(filePath, lines, encoding ?? Encoding.UTF8);
        }

        public Stream OpenRead(string filePath) => DirectoryExists(filePath)
            ? throw StreamForDirectory(filePath)
            : CreateStream(filePath, FileAccess.Read);

        public Stream OpenWrite(string filePath) => DirectoryExists(filePath)
            ? throw StreamForDirectory(filePath)
            : CreateStream(filePath, FileAccess.Write);

        private static FileStream CreateStream(string path, FileAccess fileAccess)
        {
            return new FileStream(
                path,
                FileMode.Open,
                fileAccess,
                FileShare.ReadWrite,
                DefaultBufferSize,
                FileOptions.Asynchronous);
        }

        private static Exception FileNotFound(string path)
        {
            return new FileNotFoundException($"Could not find file '{path}'.");
        }

        private static Exception StreamForDirectory(string path)
        {
            return new InvalidOperationException($"Could not create a stream for the directory '{path}'.");
        }
    }
}
