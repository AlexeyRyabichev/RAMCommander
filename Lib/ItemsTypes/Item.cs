using System.Collections.Generic;
using System.IO;
using System.Windows;
using Lib.Interfaces;

namespace Lib.ItemsTypes
{
    public abstract class Item : IItem, IFunctions
    {
        public static string DIRECTORY = "Directory";
        public static string FILE = "File";
        public static string BACK = "Back";

        public void Copy(string newPathToParent)
        {
            Item item = this;
            if (item is FileItem)
                if (Directory.Exists(newPathToParent) && File.Exists(item.FullName))
                    if (File.Exists(Path.Combine(newPathToParent, item.Name)))
                        MessageBox.Show("File already exists");
                    else
                        File.Copy(item.FullName, Path.Combine(newPathToParent, item.Name));
                else
                    MessageBox.Show("File or directory don't exists");
            else if (item is DirectoryItem)
                if (Directory.Exists(newPathToParent) && Directory.Exists(item.FullName))
                    if (Directory.Exists(Path.Combine(newPathToParent, item.Name)))
                        MessageBox.Show("Directory already exists");
                    else
                        MessageBox.Show("Copying of directories coming soon");
                //Directory.Copy(item.FullName, Path.Combine(newPathToParent, item.Name));
                else
                    MessageBox.Show("Directory don't exists");
        }

        public void Move(string newPathToParent)
        {
            Item item = this;
            if (item is FileItem)
                if (Directory.Exists(newPathToParent) && File.Exists(item.FullName))
                    if (File.Exists(Path.Combine(newPathToParent, item.Name)))
                        MessageBox.Show("File already exists");
                    else
                        File.Move(item.FullName, Path.Combine(newPathToParent, item.Name));
                else
                    MessageBox.Show("File or directory don't exists");
            else if (item is DirectoryItem)
                if (Directory.Exists(newPathToParent) && Directory.Exists(item.FullName))
                    if (Directory.Exists(Path.Combine(newPathToParent, item.Name)))
                        MessageBox.Show("Directory already exists");
                    else
                        Directory.Move(item.FullName, Path.Combine(newPathToParent, item.Name));
                else
                    MessageBox.Show("Directory don't exists");
        }

        public void Delete()
        {
            Item item = this;
            if (item is FileItem)
                if (File.Exists(item.FullName))
                    File.Delete(item.FullName);
                else
                    MessageBox.Show("File don't exists");
            else if (item is DirectoryItem)
                if (Directory.Exists(item.FullName))
                    Directory.Delete(item.FullName);
                else
                    MessageBox.Show("Directory don't exists");
        }

        public void Rename(string newName)
        {
            Item item = this;
            if (item is FileItem)
                if (File.Exists(item.FullName))
                    if (File.Exists(Path.Combine(PathToParent, newName)))
                        MessageBox.Show("File already exists");
                    else
                        File.Move(item.FullName, Path.Combine(PathToParent, newName));
                else
                    MessageBox.Show("File don't exists");
            else if (item is DirectoryItem)
                if (Directory.Exists(item.FullName))
                    if (Directory.Exists(Path.Combine(PathToParent, newName)))
                        MessageBox.Show("Directory already exists");
                    else
                        Directory.Move(item.FullName, Path.Combine(PathToParent, newName));
                else
                    MessageBox.Show("Directory don't exists");
        }

        public void Create(string pathToParent, string name, string type)
        {
            if (type == FILE)
                if (Directory.Exists(pathToParent))
                    if (!File.Exists(Path.Combine(pathToParent, name)))
                        File.Create(Path.Combine(pathToParent, name));
                    else
                        MessageBox.Show("File already exists");
                else
                    MessageBox.Show("Directory don't exists");
            else if (type == DIRECTORY)
                if (Directory.Exists(pathToParent))
                    if (!Directory.Exists(Path.Combine(pathToParent, name)))
                        Directory.CreateDirectory(Path.Combine(pathToParent, name));
                    else
                        MessageBox.Show("Directory already exists");
                else
                    MessageBox.Show("Directory don't exists");
        }

        public string Name { get; protected set; }
        public string FullName { get; protected set; }
        public string SizeItem { get; protected set; }
        public string DateModified { get; protected set; }
        public string TypeImageSource { get; protected set; }
        public string Type { get; protected set; }
        public string PathToParent { get; protected set; }
        public bool IsChecked { get; set; }
        public int Index { get; set; }
        public List<Item> Subs { get; protected set; }
    }
}