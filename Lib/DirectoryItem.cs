using System.Collections.Generic;
using System.IO;

namespace Lib
{
    public class DirectoryItem : Item
    {
        private readonly DirectoryInfo _directoryInfo;
        public List<DirectoryItem> Directories;
        public List<FileItem> Files;

        public DirectoryItem(string path, bool parseSubs) : this(new DirectoryInfo(path), parseSubs)
        {
        }

        public DirectoryItem(DirectoryInfo directoryInfo, bool parseSubs)
        {
            TypeImageSource = @"C:\Users\alexe\OneDrive\Рабочий стол\RAMcommander\Res\folder48.png";
            _directoryInfo = directoryInfo;
            Name = _directoryInfo.Name;
            FullName = _directoryInfo.FullName;
            Type = DIRECTORY;
            SizeItem = "";
            DateModified = _directoryInfo.LastWriteTimeUtc.ToString();

            if (!parseSubs) return;

            Directories = ParseDirectories(_directoryInfo.GetDirectories());
            Files = ParseFiles(_directoryInfo.GetFiles());
            Subs = new List<Item> {new BackItem()};
            Subs.AddRange(Directories);
            Subs.AddRange(Files);
        }

        private List<DirectoryItem> ParseDirectories(DirectoryInfo[] directoryInfos)
        {
            List<DirectoryItem> items = new List<DirectoryItem>();

            foreach (DirectoryInfo directoryInfo in directoryInfos) items.Add(new DirectoryItem(directoryInfo, false));

            return items;
        }

        private List<FileItem> ParseFiles(FileInfo[] fileInfos)
        {
            List<FileItem> items = new List<FileItem>();

            foreach (FileInfo fileInfo in fileInfos) items.Add(new FileItem(fileInfo));

            return items;
        }
    }
}