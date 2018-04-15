using System.IO;

namespace Lib
{
    public class FileItem : Item
    {
        private readonly FileInfo _fileInfo;

        public FileItem(FileInfo fileInfo)
        {
            TypeImageSource = @"C:\Users\alexe\OneDrive\Рабочий стол\RAMcommander\Res\file48.png";
            _fileInfo = fileInfo;
            Name = _fileInfo.Name;
            FullName = _fileInfo.FullName;
            Type = FILE;
            if (_fileInfo.Length >> 20 == 0)
            {
                if (_fileInfo.Length >> 10 == 0)
                    SizeItem = _fileInfo.Length + " bytes";
                else
                    SizeItem = (_fileInfo.Length >> 10) + " Kb";

            }
            else
                SizeItem = (_fileInfo.Length >> 20) + " Mb";
            DateModified = _fileInfo.LastWriteTimeUtc.ToString();
        }
    }
}