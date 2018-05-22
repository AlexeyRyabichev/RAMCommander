using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lib.ItemsTypes
{
    public class DirectoryItem : Item
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryItem(string path, bool parseSubs = true) : this(new DirectoryInfo(path), parseSubs)
        {
        }

        public DirectoryItem(DirectoryInfo directoryInfo, bool parseSubs = true)
        {
            _directoryInfo = directoryInfo;

            Name = _directoryInfo.Name;
            FullName = _directoryInfo.FullName;
            SizeItem = "";
            DateModified = _directoryInfo.LastWriteTimeUtc.ToString();
            TypeImageSource = Path.GetFullPath("../../Resources/folder48.png");
            Type = DIRECTORY;
            PathToParent = _directoryInfo.Parent?.FullName;
            Extension = null;
            LastAccessed = _directoryInfo.LastAccessTimeUtc.ToString();
            Size = int.MaxValue;
            IsChecked = false;
            //Index = null;
            //ImageSize = null;
            Subs = null;

            if (!parseSubs) return; // Breakpoint

            Subs = new List<Item> {new BackItem()};
            Subs.AddRange(_directoryInfo.GetDirectories()
                .Select(subDirectoryInfo => new DirectoryItem(subDirectoryInfo, false))
                .ToList()); // Equal ParseDirectories()
            Subs.AddRange(_directoryInfo.GetFiles().Select(subFileInfo => new FileItem(subFileInfo))
                .ToList()); // Equal ParseFiles()

            //DirectorySecurity directorySecurity = _directoryInfo.GetAccessControl();
            //directorySecurity.GetSecurityDescriptorSddlForm(AccessControlSections.All);
        }

        /*
        private List<DirectoryItem> ParseDirectories()
        {
            DirectoryInfo[] directoryInfos = _directoryInfo.GetDirectories();

            return directoryInfos.Select(subDirectoryInfo => new DirectoryItem(subDirectoryInfo, false)).ToList();
        }

        private List<FileItem> ParseFiles()
        {
            FileInfo[] fileInfos = _directoryInfo.GetFiles();

            return fileInfos.Select(fileInfo => new FileItem(fileInfo)).ToList();
        }
        */
    }
}