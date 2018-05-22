using System;
using System.Threading.Tasks;

namespace Lib.Interfaces
{
    public interface IFunctions
    {
        /// <summary>
        ///     Copy item
        /// </summary>
        /// <param name="progress">Progress of copying [0, 100]</param>
        /// <param name="destination">Destination path</param>
        /// <returns>Progress changes</returns>
        Task Copy(IProgress<double> progress, string destination);

        /// <summary>
        ///     Move item
        /// </summary>
        /// <param name="destination">Destination path</param>
        void Move(string destination);

        /// <summary>
        ///     Delete item
        /// </summary>
        void Delete();

        /// <summary>
        ///     Rename item
        /// </summary>
        /// <param name="newName">New name for item</param>
        void Rename(string newName);

        /// <summary>
        ///     Create item
        /// </summary>
        /// <param name="destination">Destination path</param>
        /// <param name="name">Item name</param>
        /// <param name="type">Item type</param>
        void Create(string destination, string name, string type);
    }
}