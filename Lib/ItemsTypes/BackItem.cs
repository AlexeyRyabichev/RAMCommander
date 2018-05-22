using System.IO;

namespace Lib.ItemsTypes
{
    public class BackItem : Item
    {
        /// <summary>
        ///     Create item of BackItem class
        /// </summary>
        public BackItem()
        {
            Name = @"\..";
            FullName = null;
            SizeItem = null;
            DateModified = null;
            TypeImageSource = Path.GetFullPath("../../Resources/back48.png");
            Type = BACK;
            PathToParent = null;
            Extension = null;
            LastAccessed = null;
            //Size = null;
            IsChecked = false;
            //Index = null;
            //ImageSize = null;
            Subs = null;
        }
    }
}