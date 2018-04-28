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

        public async void Copy(List<Item> items, string destination)
        {
            foreach (var item in items)
                if (item is DirectoryItem)
                {
                    Directory.CreateDirectory(Path.Combine(destination, item.Name));
                    Copy(new DirectoryItem(item.FullName, true).Subs, Path.Combine(destination, item.Name));
                }
                else if (item is FileItem)
                {
                    var progress = new Progress<double>(d => { CurrentItemProgressBar.Value = d; CurrentItemProgressText.Text = $"{CURRFILE} {item.Name} {d:f2}%"; });
                    await item.Copy(progress, destination);
                }

            OnFinish?.Invoke(this, null);
        }
    }
}