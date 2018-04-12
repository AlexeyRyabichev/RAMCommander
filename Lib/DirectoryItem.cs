using System.Collections.Generic;
using System.IO;

namespace Lib
{
    public class DirectoryItem : Item
    {
        private readonly DirectoryInfo _directoryInfo;
        public List<FileItem> Files;
        public List<DirectoryItem> Directories;

        public DirectoryItem(DirectoryInfo directoryInfo, bool parseSubs)
        {
            Type = @"C:\Users\alexe\OneDrive\Рабочий стол\Images\AI.png";
            _directoryInfo = directoryInfo;
            Name = _directoryInfo.Name;
            SizeItem = "";
            DateModified = _directoryInfo.LastWriteTimeUtc.ToString();

            if (parseSubs)
            {
                Directories = ParseDirectories(_directoryInfo.GetDirectories());
                Files = ParseFiles(_directoryInfo.GetFiles());
                Subs = new List<Item>();
                Subs.AddRange(Directories);
                Subs.AddRange(Files);
            }
        }

        private List<DirectoryItem> ParseDirectories(DirectoryInfo[] directoryInfos)
        {
            List<DirectoryItem> items = new List<DirectoryItem>();

            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                items.Add(new DirectoryItem(directoryInfo, false));
            }

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