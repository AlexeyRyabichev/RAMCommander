using System.IO;
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

            TypeImageSource = Path.GetFullPath("../../Resources/file48.png");

            _fileInfo = fileInfo;
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