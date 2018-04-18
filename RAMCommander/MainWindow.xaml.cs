using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Lib.ItemsTypes;

namespace RAMCommander
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DirectoryItem _firstDirectoryItem;
        private List<Item> _firstItems;
        private bool _isFirstFocused;
        private DirectoryItem _seconDirectoryItem;
        private List<Item> _secondItems;

        public MainWindow()
        {
            InitializeComponent();

            #region Clickers

            FirstPanelPath.KeyDown += PathOnKeyDown;
            SecondPanelPath.KeyDown += PathOnKeyDown;

            FirstPanel.MouseDoubleClick += PanelOnMouseDoubleClick;
            SecondPanel.MouseDoubleClick += PanelOnMouseDoubleClick;

            FirstPanel.PreviewKeyDown += PanelOnPreviewKeyDown;
            SecondPanel.PreviewKeyDown += PanelOnPreviewKeyDown;

            FirstPanel.AllowDrop = true;
            SecondPanel.AllowDrop = true;

            //TODO: Finish Drag'n'Drop
            //FirstPanel.MouseDown += PanelOnMouseDown;
            //SecondPanel.MouseDown += PanelOnMouseDown;

            //FirstPanel.Drop += PanelOnDrop;
            //SecondPanel.Drop += PanelOnDrop;

            #endregion

            #region Menu

            NewFileMenuItem.Click += (sender, args) => CreateNewFile();
            NewFileFastKey.Click += (sender, args) => CreateNewFile();

            NewFolderMenuItem.Click += (sender, args) => CreateNewFolder();
            NewFolderFastKey.Click += (sender, args) => CreateNewFolder();

            DeleteFileMenuItem.Click += (sender, args) => Delete();
            DeleteFolderMenuItem.Click += (sender, args) => Delete();

            DeleteFastKey.Click += (sender, args) => Delete();
            RenameFastKey.Click += (sender, args) => Rename();

            #endregion


            FillTable(true, @"C:\Users\alexe\Desktop");
            FillTable(false, @"C:\Users\alexe\Code\HSE_Stuff");
            _isFirstFocused = true;
            FirstPanel.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        private void Rename()
        {
            DataGrid currentDataGrid = _isFirstFocused ? FirstPanel : SecondPanel;
            Item currentItem = (Item)currentDataGrid.CurrentItem;
            currentItem.Rename(currentItem.Name + "RENAMED");
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void Delete()
        {
            DataGrid currentDataGrid = _isFirstFocused ? FirstPanel : SecondPanel;
            Item currentItem = (Item) currentDataGrid.CurrentItem;
            currentItem.Delete();
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CreateNewFolder()
        {
            DirectoryItem directoryItem = new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
            directoryItem.Create(directoryItem.FullName, "CreatedFolder", Item.DIRECTORY);
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CreateNewFile()
        {
            DirectoryItem directoryItem = new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
            directoryItem.Create(directoryItem.FullName, "CreatedFile.txt", Item.FILE);
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CheckDataGridFocus(DataGrid dataGrid)
        {
            _isFirstFocused = dataGrid.Name == FirstPanel.Name;
            if (_isFirstFocused)
            {
                FirstPanel.BorderBrush = new SolidColorBrush(Colors.Red);
                FirstPanel.BorderThickness = new Thickness(2);
                SecondPanel.BorderBrush = new SolidColorBrush(Colors.Gray);
                SecondPanel.BorderThickness = new Thickness(1);
            }
            else
            {
                SecondPanel.BorderBrush = new SolidColorBrush(Colors.Red);
                SecondPanel.BorderThickness = new Thickness(2);
                FirstPanel.BorderBrush = new SolidColorBrush(Colors.Gray);
                FirstPanel.BorderThickness = new Thickness(1);
            }
        }

        private void PanelOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DataGrid dataGrid = (DataGrid) sender;

            CheckDataGridFocus(dataGrid);

            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(dataGrid, ((Item) dataGrid.CurrentItem).FullName, DragDropEffects.Copy);
        }

        private void PanelOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            DataGrid dataGrid = (DataGrid) sender;
            CheckDataGridFocus(dataGrid);

            string path = (string) dragEventArgs.Data.GetData(DataFormats.StringFormat);
            Item currentItem = null;
            if (Directory.Exists(path))
                currentItem = new DirectoryItem(path, false);
            else if (File.Exists(path))
                currentItem = new FileItem(path);
            if (currentItem != null) MessageBox.Show($"{currentItem.Name} to {((DataGrid) sender).Name}");
        }

        private void PanelOnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            DataGrid dataGrid = (DataGrid) sender;
            CheckDataGridFocus(dataGrid);

            if (keyEventArgs.Key != Key.Enter && keyEventArgs.Key != Key.Back) return;
            switch (keyEventArgs.Key)
            {
                case Key.Enter:
                    PanelOnMouseDoubleClick(sender, null);
                    break;
                case Key.Back:
                    bool isFirst = ((DataGrid) sender).Name == "FirstPanel";
                    string path = (isFirst ? FirstPanelPath.Text : SecondPanelPath.Text) + @"\..";
                    if (Directory.Exists(path))
                        Directory.Exists(path);
                    else if (File.Exists(path))
                        Process.Start(path);
                    else
                        MessageBox.Show(path);
                    break;
            }
        }

        private void PanelOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DataGrid dataGrid = (DataGrid) sender;
            CheckDataGridFocus(dataGrid);

            bool isFirst = ((DataGrid) sender).Name == "FirstPanel";
            string name = ((Item) ((DataGrid) sender).CurrentItem).Name;
            string path = (isFirst ? FirstPanelPath.Text : SecondPanelPath.Text) + @"\" + name;

            if (Directory.Exists(path))
                FillTable(isFirst, path);
            else if (File.Exists(path))
                Process.Start(path);
            else
                MessageBox.Show(path);
        }

        private void PathOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key != Key.Enter) return;

            string name = ((TextBox) sender).Name;
            string path = ((TextBox) sender).Text;

            if (Directory.Exists(path))
                FillTable(name == "FirstPanelPath", path);
            else
                MessageBox.Show($"Directory {path} don't exists");
        }

        private void FillTable(bool isFirst, string path)
        {
            try
            {
                DirectoryItem testDirectoryItem = new DirectoryItem(path);
                //fails here if no access to directory

                if (isFirst)
                {
                    FirstPanel.ItemsSource = null;
                    _firstDirectoryItem = new DirectoryItem(path);
                    _firstItems = new List<Item>(_firstDirectoryItem.Subs);
                    FirstPanelPath.Text = _firstDirectoryItem.FullName;
                    FirstPanel.ItemsSource = _firstItems;
                    FirstPanel.CurrentItem = FirstPanel.Items[0];
                }
                else
                {
                    SecondPanel.ItemsSource = null;
                    _seconDirectoryItem = new DirectoryItem(path);
                    _secondItems = new List<Item>(_seconDirectoryItem.Subs);
                    SecondPanelPath.Text = _seconDirectoryItem.FullName;
                    SecondPanel.ItemsSource = _secondItems;
                    SecondPanel.CurrentItem = SecondPanel.Items[0];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}