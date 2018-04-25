﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Lib.ItemsTypes;
using RAMCommander.Windows;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

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
        private readonly Style _focusedStyle;
        private readonly Style _standardStyle;

        public MainWindow()
        {
            //TODO: PDF Previewers 
            //TODO: User Coloring 
            //TODO: Copying 
            //TODO: Moving 
            //TODO: Renaming 
            //TODO: File type 
            //TODO: Localization 
            //TODO: Panels 

            //TODO: Zoom (additional) 
            InitializeComponent();

            //FirstPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            //SecondPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

            _focusedStyle = new Style(typeof(Control));
            _focusedStyle.Setters.Add(new Setter(BackgroundProperty,
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BBDEFB")))); //fae1c0
            //TODO: Change font to bold

            _standardStyle = new Style(typeof(Control));
            _standardStyle.Setters.Add(new Setter(BackgroundProperty,
                new SolidColorBrush(Colors.White)));
            
            #region Clickers

            FirstPanelPath.KeyDown += PathOnKeyDown;
            SecondPanelPath.KeyDown += PathOnKeyDown;

            FirstPanel.MouseDoubleClick += PanelOnMouseDoubleClick;
            SecondPanel.MouseDoubleClick += PanelOnMouseDoubleClick;

            FirstPanel.KeyDown += PanelOnKeyDown;
            SecondPanel.KeyDown += PanelOnKeyDown;

            #endregion

            #region Drag'n'Drop
            //TODO: Finish Drag'n'Drop

            FirstPanel.AllowDrop = true;
            SecondPanel.AllowDrop = true;

            FirstPanel.PreviewMouseDown += PanelOnPreviewMouseDown;
            SecondPanel.PreviewMouseDown += PanelOnPreviewMouseDown;

            FirstPanel.Drop += PanelOnDrop;
            SecondPanel.Drop += PanelOnDrop;
            #endregion

            #region Menu

            NewFileMenuItem.Click += (sender, args) => CreateNewFile();
            NewFileFastKey.Click += (sender, args) => CreateNewFile();

            NewFolderMenuItem.Click += (sender, args) => CreateNewFolder();
            NewFolderFastKey.Click += (sender, args) => CreateNewFolder();

            DeleteFileMenuItem.Click += (sender, args) => Delete();
            DeleteFolderMenuItem.Click += (sender, args) => Delete();

            DeleteFastKey.Click += (sender, args) => Delete();
            RenameFastKey.Click += (sender, args) => Rename((Item) (_isFirstFocused ? FirstPanel.SelectedItem : SecondPanel.SelectedItem), _isFirstFocused);

            #endregion

            CopyPreview.Click += CopyPreviewOnClick;

            FillTable(true, @"C:\Users");
            FillTable(false, @"C:\");
            _isFirstFocused = true;
            CheckPanelFocus();
            FirstPanel.Focus();
        }

        private void PanelOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            bool flag = false;
            Item item = null;
            try
            {
                item = (Item) ((ListViewItem) keyEventArgs.OriginalSource).Content;
                //fails here if any error
                //flag = true;
            }
            catch (Exception)
            {
                ChangeFocus();
            }
            
            //if (!flag)
            //    return;

            CheckPanelFocus();


            switch (keyEventArgs.Key)
            {
                case Key.Back:
                    string path = (_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text) + @"\..";
                    if (Directory.Exists(path))
                        FillTable(_isFirstFocused, path);
                    else
                        MessageBox.Show("Can't go to parent folder");
                    break;
                case Key.Enter:
                    PanelOnMouseDoubleClick(sender, null);
                    break;
                case Key.Right:
                    ChangeFocus();
                    break;
                case Key.Left:
                    ChangeFocus();
                    break;
                case Key.F2:
                    Rename(item, _isFirstFocused);
                    break;
            }
        }

        private void CopyPreviewOnClick(object o, RoutedEventArgs routedEventArgs)
        {
            OperationWindow operationWindow = new OperationWindow("Copying");
            operationWindow.Show();

            operationWindow.CurrentItemProgress = 50;
            operationWindow.TotalProgress = 25;
        }

        private void Rename(Item item, bool isFirst)
        {
            ChooseNameWindow chooseNameWindow = new ChooseNameWindow("Rename: ", item.Name);
            if (chooseNameWindow.ShowDialog() == true)
            {
                item.Rename(chooseNameWindow.NewName);
                FillTable(isFirst, isFirst ? FirstPanelPath.Text : SecondPanelPath.Text);
            }
        }

        private void Delete()
        {
            ListView currentListView = _isFirstFocused ? FirstPanel : SecondPanel;
            Item currentItem = (Item)currentListView.SelectedItem;
            currentItem.Delete();
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CreateNewFolder()
        {
            DirectoryItem directoryItem =
                new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
            directoryItem.Create(directoryItem.FullName, "CreatedFolder", Item.DIRECTORY);
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CreateNewFile()
        {
            DirectoryItem directoryItem =
                new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
            directoryItem.Create(directoryItem.FullName, "CreatedFile.txt", Item.FILE);
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CheckPanelFocus()
        {
            if (_isFirstFocused)
            {
                FirstGridView.ColumnHeaderContainerStyle = _focusedStyle;
                SecondGridView.ColumnHeaderContainerStyle = _standardStyle;
            }
            else
            {
                FirstGridView.ColumnHeaderContainerStyle = _standardStyle;
                SecondGridView.ColumnHeaderContainerStyle = _focusedStyle;
            }
        }

        private void PanelOnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ListView listView = (ListView) sender;
            _isFirstFocused = listView.Name == FirstPanel.Name;
            CheckPanelFocus();
            


            //if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed && mouseButtonEventArgs.ClickCount == 1)
            //    DragDrop.DoDragDrop(listView, listView.Items.CurrentItem, DragDropEffects.Copy);
        }

        private void PanelOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            //DataGrid dataGrid = (DataGrid)sender;
            //CheckPanelFocus();

            //string path = (string)dragEventArgs.Data.GetData(DataFormats.StringFormat);
            //Item currentItem = null;
            //if (Directory.Exists(path))
            //    currentItem = new DirectoryItem(path, false);
            //else if (File.Exists(path))
            //    currentItem = new FileItem(path);
            //if (currentItem != null) MessageBox.Show($"{currentItem.Name} to {((DataGrid)sender).Name}");
        }

        private void PanelOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ListView listView = (ListView)sender;
            CheckPanelFocus();

            bool isFirst = ((ListView)sender).Name == "FirstPanel";
            string name = ((Item)((ListView)sender).SelectedItem).Name;
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

            string name = ((TextBox)sender).Name;
            string path = ((TextBox)sender).Text;

            if (Directory.Exists(path))
                FillTable(name == "FirstPanelPath", path);
            else
                MessageBox.Show($"Directory {path} don't exists");
        }

        private void ChangeFocus()
        {
            if (_isFirstFocused)
            {
                ((ListViewItem) SecondPanel.ItemContainerGenerator.ContainerFromIndex(0)).Focus();
                FirstPanel.SelectedItem = null;
            }
            else
            {
                ((ListViewItem)FirstPanel.ItemContainerGenerator.ContainerFromIndex(0)).Focus();
                SecondPanel.SelectedItem = null;
            }
            _isFirstFocused = !_isFirstFocused;
            CheckPanelFocus();
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
                    for (int i = 0; i < _firstItems.Count; i++)
                        _firstItems[i].Index = i;
                    FirstPanelPath.Text = _firstDirectoryItem.FullName;
                    FirstPanel.ItemsSource = _firstItems;
                }
                else
                {
                    SecondPanel.ItemsSource = null;
                    _seconDirectoryItem = new DirectoryItem(path);
                    _secondItems = new List<Item>(_seconDirectoryItem.Subs);
                    for (int i = 0; i < _secondItems.Count; i++)
                        _secondItems[i].Index = i;
                    SecondPanelPath.Text = _seconDirectoryItem.FullName;
                    SecondPanel.ItemsSource = _secondItems;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CheckBoxTemplate_OnChecked(object sender, RoutedEventArgs e)
        {
        }

        private void CheckBoxTemplate_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox) sender;
            ListViewItem listViewItem = (ListViewItem) (_isFirstFocused ? FirstPanel : SecondPanel).ItemContainerGenerator.ContainerFromIndex(
                int.Parse(checkBox.Uid));
            if (checkBox.IsChecked != null && (bool) checkBox.IsChecked)
                listViewItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B2EBF2"));
            else
                listViewItem.Background = new SolidColorBrush(Colors.White);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //if 
        }
    }
}