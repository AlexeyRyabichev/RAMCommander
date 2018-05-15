﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Lib.Interfaces;

namespace Lib.ItemsTypes
{
    public abstract class Item : IItem, IFunctions
    {
        public static string DIRECTORY = "Directory";
        public static string FILE = "File";
        public static string BACK = "Back";

        public async Task Copy(IProgress<double> progress, string destination)
        {
            while (File.Exists(Path.Combine(destination, Name)))
            {
                Name = Path.GetFileNameWithoutExtension(Path.Combine(PathToParent, Name)) + "_Copy." + Extension;
            }

            byte[] buffer = new byte[1024 * 1024]; // 1mb
            bool cancelFlag = false;

            using (FileStream sourceFileStream = new FileStream(FullName, FileMode.Open, FileAccess.Read))
            {
                long fileLength = sourceFileStream.Length;
                using (FileStream destFileStream =
                    new FileStream(Path.Combine(destination, Name), FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize;

                    while ((currentBlockSize = sourceFileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        double percentage = 0;
                        await Task.Run(() =>
                        {
                            totalBytes += currentBlockSize;

                            percentage = totalBytes * 100.0 / fileLength; 

                            destFileStream.Write(buffer, 0, currentBlockSize);

                            cancelFlag = false;
                        });
                        progress.Report(percentage);

                        if (cancelFlag)
                            break;
                    }
                }
            }
        }

        public void Move(string destination)
        {
            Item item = this;
            if (item is FileItem)
                if (Directory.Exists(destination) && File.Exists(item.FullName))
                    if (File.Exists(Path.Combine(destination, item.Name)))
                        MessageBox.Show("File already exists");
                    else
                        File.Move(item.FullName, Path.Combine(destination, item.Name));
                else
                    MessageBox.Show("File or directory don't exists");
            else if (item is DirectoryItem)
                if (Directory.Exists(destination) && Directory.Exists(item.FullName))
                    if (Directory.Exists(Path.Combine(destination, item.Name)))
                        MessageBox.Show("Directory already exists");
                    else
                        Directory.Move(item.FullName, Path.Combine(destination, item.Name));
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
                    if (new DirectoryItem(item.FullName, true).Subs != null)
                    {
                        foreach (Item sub in new DirectoryItem(item.FullName, true).Subs)
                            sub.Delete();

                        Directory.Delete(item.FullName);
                    }
                    else
                    {
                        Directory.Delete(item.FullName);
                    }
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
                        File.Move(item.FullName, Path.Combine(PathToParent, newName.Replace(":", "-").Replace("/", "_")));
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

        public void Create(string destination, string name, string type)
        {
            if (type == FILE)
                if (Directory.Exists(destination))
                    if (!File.Exists(Path.Combine(destination, name)))
                        File.Create(Path.Combine(destination, name));
                    else
                        MessageBox.Show("File already exists");
                else
                    MessageBox.Show("Directory don't exists");
            else if (type == DIRECTORY)
                if (Directory.Exists(destination))
                    if (!Directory.Exists(Path.Combine(destination, name)))
                        Directory.CreateDirectory(Path.Combine(destination, name));
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
        public string Extension { get; protected set; }
        public string LastAccessed { get; protected set; }
        public long Size { get; protected set; }
        public bool IsChecked { get; set; }
        public int Index { get; set; }
        public int ImageSize { get; set; }
        public List<Item> Subs { get; protected set; }
    }
}