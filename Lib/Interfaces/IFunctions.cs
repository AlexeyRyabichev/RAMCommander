using Lib.ItemsTypes;

namespace Lib.Interfaces
{
    public interface IFunctions
    {
        void Copy(Item item, string newPathToParent);
        void Move(Item item, string newPathToParent);
        void Delete(Item item);
        void Create(string pathToParent, string name, string type);
    }
}