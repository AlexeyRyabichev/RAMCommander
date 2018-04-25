using System.IO;
using System.Windows;
using Lib.Interfaces;

namespace Lib.ItemsTypes
{
    public class FileItem : Item, IFileFunctions
    {
        private readonly FileInfo _fileInfo;

        public FileItem(string path) : this(new FileInfo(path))
        {
        }

        public FileItem(FileInfo fileInfo)
        {
            //TypeImageSource = @"Resources\file48.png";
            //TypeImageSource = Properties.Resources.file48;
            switch (fileInfo.Extension)
            {
                case ".raw":
                    TypeImageSource = Path.GetFullPath("../../Resources/raw48.png");
                    break;
                case ".css":
                    TypeImageSource = Path.GetFullPath("../../Resources/css48.png");
                    break;
                case ".html":
                    TypeImageSource = Path.GetFullPath("../../Resources/html48.png");
                    break;
                case ".c":
                    TypeImageSource = Path.GetFullPath("../../Resources/c48.png");
                    break;
                case ".cpp":
                    TypeImageSource = Path.GetFullPath("../../Resources/cpp48.png");
                    break;
                case ".cs":
                    TypeImageSource = Path.GetFullPath("../../Resources/csharp48.png");
                    break;
                case ".exe":
                    TypeImageSource = Path.GetFullPath("../../Resources/exe48.png");
                    break;
                case ".7z":
                    TypeImageSource = Path.GetFullPath("../../Resources/7z48.png");
                    break;
                case ".zip":
                    TypeImageSource = Path.GetFullPath("../../Resources/zip48.png");
                    break;
                case ".rar":
                    TypeImageSource = Path.GetFullPath("../../Resources/rar48.png");
                    break;
                case ".xls":
                    TypeImageSource = Path.GetFullPath("../../Resources/xls48.png");
                    break;
                case ".psd":
                    TypeImageSource = Path.GetFullPath("../../Resources/psd48.png");
                    break;
                case ".png":
                    TypeImageSource = Path.GetFullPath("../../Resources/png48.png");
                    break;
                case ".jpg":
                    TypeImageSource = Path.GetFullPath("../../Resources/jpg48.png");
                    break;
                case ".doc":
                    TypeImageSource = Path.GetFullPath("../../Resources/doc48.png");
                    break;
                case ".txt":
                    TypeImageSource = Path.GetFullPath("../../Resources/txt48.png");
                    break;
                case ".mp3":
                    TypeImageSource = Path.GetFullPath("../../Resources/audio48.png");
                    break;
                case ".mp4":
                case ".mkv":
                    TypeImageSource = Path.GetFullPath("../../Resources/video48.png");
                    break;
                case ".pdf":
                    TypeImageSource = Path.GetFullPath("../../Resources/pdf48.png");
                    break;
                default:
                    TypeImageSource = Path.GetFullPath("../../Resources/file48.png");
                    break;
            }

            _fileInfo = fileInfo;
            Extension = _fileInfo.Extension.Replace(".", "");
            Size = _fileInfo.Length;
            Name = _fileInfo.Name;
            FullName = _fileInfo.FullName;
            Type = FILE;
            if (_fileInfo.Directory != null) PathToParent = _fileInfo.Directory.FullName;
            if (_fileInfo.Length >> 20 == 0)
                if (_fileInfo.Length >> 10 == 0)
                    SizeItem = _fileInfo.Length + " bytes";
                else
                    SizeItem = (_fileInfo.Length >> 10) + " Kb";
            else
                SizeItem = (_fileInfo.Length >> 20) + " Mb";
            DateModified = _fileInfo.LastWriteTimeUtc.ToString();
            IsChecked = false;
        }
    }
}