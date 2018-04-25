using System.Collections.Generic;
using System.IO;
using Lib.Interfaces;

namespace Lib.ItemsTypes
{
    public class DirectoryItem : Item, IDirectoriesFunctions
    {
        private readonly DirectoryInfo _directoryInfo;
        public List<DirectoryItem> Directories;
        public List<FileItem> Files;

        public DirectoryItem(string path, bool parseSubs = true) : this(new DirectoryInfo(path), parseSubs)
        {
        }

        public DirectoryItem(DirectoryInfo directoryInfo, bool parseSubs = true)
        {
            //TypeImageSource = @"C:\Users\alexe\OneDrive\Рабочий стол\RAMcommander\Res\folder48.png";
            //TypeImageSource = Properties.Resources.folder48;
            TypeImageSource = Path.GetFullPath("../../Resources/folder48.png");
            _directoryInfo = directoryInfo;
            Name = _directoryInfo.Name;
            FullName = _directoryInfo.FullName;
            Type = DIRECTORY;
            SizeItem = "";
            DateModified = _directoryInfo.LastWriteTimeUtc.ToString();
            if (directoryInfo.Parent != null) PathToParent = directoryInfo.Parent.FullName;

            if (!parseSubs) return;

            Directories = ParseDirectories(_directoryInfo.GetDirectories());
            Files = ParseFiles(_directoryInfo.GetFiles());
            Subs = new List<Item> {new BackItem()};
            Subs.AddRange(Directories);
            Subs.AddRange(Files);
            IsChecked = false;
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