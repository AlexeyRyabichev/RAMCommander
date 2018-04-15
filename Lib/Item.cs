using System;
using System.Collections.Generic;

namespace Lib
{
    public abstract class Item : IItem
    {
        public string Name { get; protected set; }
        public string FullName { get; protected set; }
        public string SizeItem { get; protected set; }
        public string DateModified { get; protected set; }
        public string TypeImageSource { get; protected set; }
        public string Type { get; protected set; }
        public List<Item> Subs { get; protected set; }

        public static string DIRECTORY = "Directory";
        public static string FILE = "File";
        public static string BACK = "Back";
    }
}