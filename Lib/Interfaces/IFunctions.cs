using System;
using System.Threading.Tasks;

namespace Lib.Interfaces
{
    public interface IFunctions
    {
        Task Copy(IProgress<double> progress, string destination);
        //void Copy(string newPathToParent);
        void Move(string newPathToParent);
        void Delete();
        void Rename(string newName);
        void Create(string pathToParent, string name, string type);
    }
}