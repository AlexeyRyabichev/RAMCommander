using System.IO;

namespace Lib
{
    public class FileItem : Item
    {
        private readonly FileInfo _fileInfo;

        public FileItem(FileInfo fileInfo)
        {
            Type = @"C:\Users\alexe\OneDrive\Рабочий стол\Images\APK.png";
            _fileInfo = fileInfo;
            Name = _fileInfo.Name;
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