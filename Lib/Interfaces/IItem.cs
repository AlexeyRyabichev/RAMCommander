﻿using System.Collections.Generic;
using Lib.ItemsTypes;

namespace Lib.Interfaces
{
    public interface IItem
    {
        string Name { get; }
        string FullName { get; }
        string SizeItem { get; }
        string DateModified { get; }
        string TypeImageSource { get; }
        string Type { get; }
        string PathToParent { get; }
        string Extension { get; }
        string LastAccessed { get; }
        long Size { get; }
        bool IsChecked { get; set; }
        int Index { get; set; }
        List<Item> Subs { get; }
    }
}