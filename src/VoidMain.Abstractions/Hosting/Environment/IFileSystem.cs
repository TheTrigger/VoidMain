using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VoidMain.Hosting.Environment
{
    public interface IFileSystem
    {
        bool Exists(string path);
        bool FileExists(string filePath);
        bool DirectoryExists(string dirPath);

        long GetFileSize(string filePath);
        DateTime GetLastModifiedTime(string path);
        DateTime GetLastModifiedTimeUtc(string path);

        void CreateFile(string filePath);
        void CreateDirectory(string dirPath);

        void Move(string sourcePath, string destPath);
        void Delete(string path);

        string ReadAllText(string filePath, Encoding encoding = null);
        string[] ReadAllLines(string filePath, Encoding encoding = null);

        void WriteAllText(string filePath, string text, Encoding encoding = null);
        void WriteAllLines(string filePath, IEnumerable<string> lines, Encoding encoding = null);

        Stream OpenRead(string filePath);
        Stream OpenWrite(string filePath);
    }
}
