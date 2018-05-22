using System.IO;

namespace Lib.ItemsTypes
{
    public class FileItem : Item
    {
        private readonly FileInfo _fileInfo;

        public FileItem(string path) : this(new FileInfo(path))
        {
        }

        public FileItem(FileInfo fileInfo)
        {
            //Name
            //FullName
            //SizeItem
            //DateModified
            //TypeImageSource
            //Type
            //PathToParent
            //Extension
            //LastAccessed
            //Size
            //IsChecked
            //Index
            //ImageSize
            //Subs

            _fileInfo = fileInfo;

            Name = _fileInfo.Name;
            FullName = _fileInfo.FullName;
            SizeItem = SetSizeItem();
            DateModified = _fileInfo.LastWriteTimeUtc.ToString();
            TypeImageSource = SetTypeImageSource();
            Type = FILE;
            PathToParent = _fileInfo.Directory?.FullName;
            Extension = _fileInfo.Extension.Replace(".", "");
            LastAccessed = _fileInfo.LastAccessTimeUtc.ToString();
            Size = _fileInfo.Length;
            IsChecked = false;
            //Index = null;
            //ImageSize = null;
            Subs = null;
        }

        private string SetTypeImageSource()
        {
            switch (_fileInfo.Extension)
            {
                case ".raw":
                    return Path.GetFullPath("../../Resources/raw48.png");
                case ".css":
                    return Path.GetFullPath("../../Resources/css48.png");
                case ".html":
                    return Path.GetFullPath("../../Resources/html48.png");
                case ".c":
                    return Path.GetFullPath("../../Resources/c48.png");
                case ".cpp":
                    return Path.GetFullPath("../../Resources/cpp48.png");
                case ".cs":
                    return Path.GetFullPath("../../Resources/csharp48.png");
                case ".exe":
                    return Path.GetFullPath("../../Resources/exe48.png");
                case ".7z":
                    return Path.GetFullPath("../../Resources/7z48.png");
                case ".zip":
                    return Path.GetFullPath("../../Resources/zip48.png");
                case ".rar":
                    return Path.GetFullPath("../../Resources/rar48.png");
                case ".xls":
                    return Path.GetFullPath("../../Resources/xls48.png");
                case ".psd":
                    return Path.GetFullPath("../../Resources/psd48.png");
                case ".png":
                    return Path.GetFullPath("../../Resources/png48.png");
                case ".jpg":
                    return Path.GetFullPath("../../Resources/jpg48.png");
                case ".doc":
                    return Path.GetFullPath("../../Resources/doc48.png");
                case ".txt":
                    return Path.GetFullPath("../../Resources/txt48.png");
                case ".mp3":
                    return Path.GetFullPath("../../Resources/audio48.png");
                case ".mp4":
                case ".mkv":
                    return Path.GetFullPath("../../Resources/video48.png");
                case ".pdf":
                    return Path.GetFullPath("../../Resources/pdf48.png");
                default:
                    return Path.GetFullPath("../../Resources/file48.png");
            }
        }

        private string SetSizeItem()
        {
            if (_fileInfo.Length >> 20 != 0) return (_fileInfo.Length >> 20) + " Mb";

            if (_fileInfo.Length >> 10 != 0) return (_fileInfo.Length >> 10) + " Kb";

            return _fileInfo.Length + " bytes";
        }
    }
}