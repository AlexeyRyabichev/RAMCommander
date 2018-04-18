using Lib.ItemsTypes;

namespace Lib.Interfaces
{
    public interface IFunctions
    {
        void Copy(string newPathToParent);
        void Move(string newPathToParent);
        void Delete();
        void Rename(string newName);
        void Create(string pathToParent, string name, string type);
    }
}