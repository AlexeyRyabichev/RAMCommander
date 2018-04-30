using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Lib.ItemsTypes;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for OperationWindow.xaml
    /// </summary>
    public partial class OperationWindow : Window
    {
        private const string CURRENTOPERATION = "Current operation: ";
        private const string CURRFILE = "Current file: ";

        public event EventHandler OnFinish; 

        public OperationWindow(string operationName)
        {
            InitializeComponent();

            CurrentOperationText.Text = CURRENTOPERATION + operationName;
            CurrentItemProgressText.Text = CURRFILE;

            CurrentItemProgressBar.Minimum = 0;
            CurrentItemProgressBar.Maximum = 100;
            CurrentItemProgressBar.Value = 0;
        }   

        public int CurrentItemProgress { get; set; }

        public int TotalProgress { get; set; }

        public async void Delete(List<Item> items)
        {
            foreach (Item item in items)
            {
                switch (item)
                {
                    case DirectoryItem _:
                        Delete(new DirectoryItem(item.FullName, true).Subs);
                        item.Delete();
                        break;
                    case FileItem _:
                        item.Delete();
                        break;
                }
            }

            CurrentItemProgressBar.Value = 100;
            CurrentItemProgressText.Text = "Finished deleting";
            OnFinish?.Invoke(this, null);
        }

        public async void Move(List<Item> items, string destination)
        {
            foreach (Item item in items)
            {
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
                        Copy(new DirectoryItem(item.FullName, true).Subs, Path.Combine(destination, item.Name));
                        break;
                    case FileItem _:
                        var progress = new Progress<double>(d => { CurrentItemProgressBar.Value = d; CurrentItemProgressText.Text = $"{CURRFILE} {item.Name} {d:f2}%"; });
                        await item.Copy(progress, destination);
                        break;
                }

            CurrentItemProgressText.Text = "Finished copying";
            OnFinish?.Invoke(this, null);
        }
    }
}