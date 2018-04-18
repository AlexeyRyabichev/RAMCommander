using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lib;
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
        private DirectoryItem _seconDirectoryItem;
        private List<Item> _secondItems;

        public MainWindow()
        {
            InitializeComponent();

            FirstPanelPath.KeyDown += PathOnKeyDown;
            SecondPanelPath.KeyDown += PathOnKeyDown;

            FirstPanel.MouseDoubleClick += PanelOnMouseDoubleClick;
            SecondPanel.MouseDoubleClick += PanelOnMouseDoubleClick;

            FirstPanel.PreviewKeyDown += PanelOnPreviewKeyDown;
            SecondPanel.PreviewKeyDown += PanelOnPreviewKeyDown;

            FirstPanel.AllowDrop = true;
            SecondPanel.AllowDrop = true;

            FirstPanel.MouseDown += PanelOnMouseDown;
            SecondPanel.MouseDown += PanelOnMouseDown;

            FirstPanel.Drop += PanelOnDrop;
            SecondPanel.Drop += PanelOnDrop;

            FillTable(true, @"C:\");
            FillTable(false, @"C:\Users\alexe\OneDrive\Рабочий стол\HSE_Stuff\KDZ3_DiscreteMath");
        }

        private void PanelOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DataGrid dataGrid = (DataGrid) sender;

            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(dataGrid, ((Item)dataGrid.CurrentItem).FullName, DragDropEffects.Copy);
        }

        private void PanelOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            string path = (string) dragEventArgs.Data.GetData(DataFormats.StringFormat);
            Item currentItem = null;
            if (Directory.Exists(path))
                currentItem = new DirectoryItem(path, false);
            else if (File.Exists(path))
                currentItem = new FileItem(path);
            if (currentItem != null) MessageBox.Show($"{currentItem.Name} to {((DataGrid)sender).Name}");
        }

        private void PanelOnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
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
                DirectoryItem testDirectoryItem = new DirectoryItem(path, true);
                //fails here if no access to directory

                if (isFirst)
                {
                    FirstPanel.ItemsSource = null;
                    _firstDirectoryItem = new DirectoryItem(path, true);
                    _firstItems = new List<Item>(_firstDirectoryItem.Subs);
                    FirstPanelPath.Text = _firstDirectoryItem.FullName;
                    FirstPanel.ItemsSource = _firstItems;
                    FirstPanel.CurrentItem = FirstPanel.Items[0];
                }
                else
                {
                    SecondPanel.ItemsSource = null;
                    _seconDirectoryItem = new DirectoryItem(path, true);
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