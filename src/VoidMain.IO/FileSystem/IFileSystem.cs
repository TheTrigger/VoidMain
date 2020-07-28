using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidMain.IO.FileSystem
{
    public interface IFileSystem
    {
        string CurrentDirectory { get; set; }

        bool FileExists(string filePath);
        bool DirectoryExists(string dirPath);

        long GetFileSize(string filePath);
        DateTime GetLastModifiedTime(string path);
        DateTime GetLastModifiedTimeUtc(string path);

        Stream CreateFile(string filePath);
        void CreateDirectory(string dirPath);

        void Move(string sourcePath, string destPath);
        void DeleteFile(string filePath);
        void DeleteDirectory(string dirPath, bool recursive = false);

        string ReadAllText(string filePath, Encoding? encoding = null);
        string[] ReadAllLines(string filePath, Encoding? encoding = null);

        void WriteAllText(string filePath, string text, Encoding? encoding = null);
        void WriteAllLines(string filePath, IEnumerable<string> lines, Encoding? encoding = null);

        Stream OpenRead(string filePath);
        Stream OpenWrite(string filePath);
    }
}
