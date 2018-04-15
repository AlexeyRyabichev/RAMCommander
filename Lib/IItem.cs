using System;
using System.Collections.Generic;

namespace Lib
{
    public interface IItem
    {
        string Name { get; }
        string FullName { get; }
        string SizeItem { get; }
        string DateModified { get; }
        string TypeImageSource { get; }
        string Type { get; }
        List<Item> Subs { get; }
    }
}