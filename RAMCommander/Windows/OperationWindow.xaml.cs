using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Lib.ItemsTypes;
using MessageBox = System.Windows.MessageBox;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for OperationWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class OperationWindow : Window
    {
        private static readonly string CURRENTOPERATION = "Current operation: ";
        private static readonly string CURRFILE = "Current file: ";

        public OperationWindow(string operationName)
        {
            InitializeComponent();

            CurrentOperationText.Text = CURRENTOPERATION + operationName;
            CurrentItemProgressText.Text = CURRFILE;

            CurrentItemProgressBar.Minimum = 0;
            CurrentItemProgressBar.Maximum = 100;
            CurrentItemProgressBar.Value = 0;
        }

        public event EventHandler OnFinish;

        public void Archive(Item item, string destination)
        {
            try
            {
                CurrentItemProgressBar.IsIndeterminate = true;
                CurrentItemProgressText.Text = "Archiving, please wait";
                ZipFile.CreateFromDirectory(item.FullName, destination);

                OnFinish?.Invoke(this, null);
            }
            catch (Exception)
            {
                MessageBox.Show("Cant' archive item");
            }
        }

        public void Unarchive(Item item)
        {
            try
            {
                CurrentItemProgressBar.IsIndeterminate = true;
                CurrentItemProgressText.Text = "Unarchiving, please wait";

                ZipFile.ExtractToDirectory(item.FullName, Path.Combine(item.PathToParent, Path.GetFileNameWithoutExtension(item.FullName)));
                OnFinish?.Invoke(this, null);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't unarchive item");
            }
        }

        public void Delete(List<Item> items)
        {
            foreach (Item item in items)
                switch (item)
                {
                    case DirectoryItem _:
                        Delete(new DirectoryItem(item.FullName).Subs);
                        item.Delete();
                        break;
                    case FileItem _:
                        item.Delete();
                        break;
                }

            CurrentItemProgressBar.Value = 100;
            CurrentItemProgressText.Text = "Finished deleting";
            OnFinish?.Invoke(this, null);
        }

        public void Move(List<Item> items, string destination)
        {
            foreach (Item item in items)
                switch (item)
                {
                    case DirectoryItem _:
                        Directory.CreateDirectory(Path.Combine(destination, item.Name));
                        Move(new DirectoryItem(item.FullName).Subs, Path.Combine(destination, item.Name));
                        item.Delete();
                        break;
                    case FileItem _:
                        item.Move(destination);
                        break;
                }

            CurrentItemProgressBar.Value = 100;
            CurrentItemProgressText.Text = "Finished moving";
            OnFinish?.Invoke(this, null);
        }

        public async void Copy(List<Item> items, string destination)
        {
            foreach (var item in items)
                switch (item)
                {
                    case DirectoryItem _:
                        Directory.CreateDirectory(Path.Combine(destination, item.Name));
                        Copy(new DirectoryItem(item.FullName).Subs, Path.Combine(destination, item.Name));
                        break;
                    case FileItem _:
                        var progress = new Progress<double>(d =>
                        {
                            CurrentItemProgressBar.Value = d;
                            CurrentItemProgressText.Text = $"{CURRFILE} {item.Name} {d:f2}%";
                        });
                        await item.Copy(progress, destination);
                        break;
                }

            CurrentItemProgressText.Text = "Finished copying";
            OnFinish?.Invoke(this, null);
        }
    }
}