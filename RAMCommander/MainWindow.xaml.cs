using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Lib;
using Lib.ItemsTypes;
using Microsoft.VisualBasic.FileIO;
using PainlessHttp.Serializer.JsonNet;
using RAMCommander.Windows;

namespace RAMCommander
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        private readonly Style _focusedStyle;
        private readonly Style _standardStyle;
        private DirectoryItem _firstDirectoryItem;
        private List<Item> _firstItems;
        private bool _isFirstFocused;
        private DirectoryItem _seconDirectoryItem;
        private List<Item> _secondItems;

        public MainWindow()
        {
            Closing += (sender, args) =>
            {
                if (File.Exists(SettingsBackup.SettingsFileName))
                    File.Delete(SettingsBackup.SettingsFileName);
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                SettingsBackup settingsBackup = new SettingsBackup{FirstPanelPath = FirstPanelPath.Text, SecondPanelPath = SecondPanelPath.Text};
                File.WriteAllText(SettingsBackup.SettingsFileName, javaScriptSerializer.Serialize(settingsBackup));
            };
            InitializeComponent();

            FirstPanel.PreviewMouseWheel += PanelOnMouseWheel;
            SecondPanel.PreviewMouseWheel += PanelOnMouseWheel;

            FirstPanelButton.Content = "Refresh";
            SecondPanelButton.Content = "Refresh";

            FirstPanelButton.Click += (sender, args) => FillTable(true, FirstPanelPath.Text);
            SecondPanelButton.Click += (sender, args) => FillTable(false, SecondPanelPath.Text);

            FirstPanel.SelectionChanged += PanelOnSelectionChanged;
            SecondPanel.SelectionChanged += PanelOnSelectionChanged;

            //FirstPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            //SecondPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

            _focusedStyle = new Style(typeof(Control));
            try
            {
                _focusedStyle.Setters.Add(new Setter(BackgroundProperty,
                    new SolidColorBrush(
                        // ReSharper disable once PossibleNullReferenceException
                        (Color)ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColorStatic)))); //fae1c0
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Can't find standart color");
                _focusedStyle.Setters.Add(new Setter(BackgroundProperty,
                    new SolidColorBrush(Colors.Blue)));
            }

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

            DeleteFastKey.Click += DeleteFastKeyOnClick;
            RenameFastKey.Click += RenameFastKeyOnClick;
            CopyFastKey.Click += CopyFastKeyOnClick;
            MoveFastKey.Click += MoveFastKeyOnClick;
            CopyFastKeyWindows.Click += CopyFastKeyWindowsOnClick;
            RenameTemplateFastKey.Click += RenameTemplateFastKeyOnClick;

            SettingsMenuItem.Click += SettingsMenuItemOnClick;

            #endregion
            

            if (File.Exists("settings.json"))
            {
                SettingsBackup unused = (SettingsBackup) NewtonSoft.Deserialize(File.ReadAllText("settings.json"), typeof(SettingsBackup));
                CheckPanelFocus();
            }
            else
                SettingsBackup.SetDefaults();

            FillTable(true, SettingsBackup.FirstPanelPathStatic);
            FillTable(false, SettingsBackup.SecondPanelPathStatic);
            _isFirstFocused = true;
            CheckPanelFocus();
            FirstPanel.Focus();
        }

        private void RenameTemplateFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            GroupRenamingWindow groupRenamingWindow = new GroupRenamingWindow();
            // ReSharper disable once PossibleInvalidOperationException
            if ((bool) groupRenamingWindow.ShowDialog())
            {
                string template = groupRenamingWindow.NewName;
                List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                    .Where(item => item.IsChecked).ToList();
                foreach (Item item in items)
                {
                    string newName = template.Replace("{DATE}", item.DateModified).Replace("{NAME}", item.Name)
                        .Replace("{PATH}", item.FullName).Replace("{SIZE}", item.SizeItem);
                    item.Rename(newName);
                }
            }

            UpdatePanels();
        }

        private void CopyFastKeyWindowsOnClick(object sender, RoutedEventArgs e)
        {
            string newPath = (_isFirstFocused ? SecondPanelPath : FirstPanelPath).Text;
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                .Where(item => item.IsChecked).ToList();

            //Windows built-in copying
            foreach (Item item in items)
                if (item is FileItem)
                    FileSystem.CopyFile(item.FullName, Path.Combine(newPath, item.Name), UIOption.AllDialogs,
                        UICancelOption.DoNothing);
                else if (item is DirectoryItem)
                    FileSystem.CopyDirectory(item.FullName, Path.Combine(newPath, item.Name), UIOption.AllDialogs,
                        UICancelOption.DoNothing);

            UpdatePanels();
        }

        private void MoveFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            string newPath = (_isFirstFocused ? SecondPanelPath : FirstPanelPath).Text;
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                .Where(item => item.IsChecked).ToList();


            OperationWindow operationWindow = new OperationWindow("Moving");
            operationWindow.OnFinish += (o, s) =>
            {
                UpdatePanels();
                operationWindow.Close();
            };
            operationWindow.Show();
            operationWindow.Move(items, newPath);

            UpdatePanels();
        }

        private void RenameFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                .Where(item => item.IsChecked).ToList();

            foreach (Item item in items)
            {
                ChooseNameWindow chooseNameWindow = new ChooseNameWindow("Rename file", item.Name);
                // ReSharper disable once PossibleInvalidOperationException
                if ((bool) chooseNameWindow.ShowDialog())
                    item.Rename(chooseNameWindow.NewName);
            }

            UpdatePanels();
        }

        private void PanelOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ListView listView = (ListView) sender;
            _isFirstFocused = listView.Name == FirstPanel.Name;
            SettingsBackup settingsBackup = new SettingsBackup();

            if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;

            if (e.Delta > 0)
            {
                if (_isFirstFocused)
                {
                    settingsBackup.FontSizeFirstPanel += 5;
                    settingsBackup.ImageSizeFirstPanel += 10;
                }
                else
                {
                    settingsBackup.FontSizeSecondPanel += 5;
                    settingsBackup.ImageSizeSecondPanel += 10;
                }

                listView.FontSize = _isFirstFocused ? settingsBackup.FontSizeFirstPanel : settingsBackup.FontSizeSecondPanel;

                listView.ItemsSource = null;

                foreach (Item item in _isFirstFocused ? _firstItems : _secondItems) item.ImageSize = (_isFirstFocused ? settingsBackup.ImageSizeFirstPanel : settingsBackup.ImageSizeSecondPanel);
                listView.ItemsSource = _isFirstFocused ? _firstItems : _secondItems;
            }
            else if (e.Delta < 0)
            {
                if (_isFirstFocused)
                {
                    if (settingsBackup.FontSizeFirstPanel > 5)
                        settingsBackup.FontSizeFirstPanel -= 5;
                    if (settingsBackup.ImageSizeFirstPanel > 10)
                        settingsBackup.ImageSizeFirstPanel -= 10;
                }
                else
                {
                    if (settingsBackup.FontSizeSecondPanel > 5)
                        settingsBackup.FontSizeSecondPanel -= 5;
                    if (settingsBackup.ImageSizeSecondPanel > 10)
                        settingsBackup.ImageSizeSecondPanel -= 10;
                }

                listView.FontSize = _isFirstFocused ? settingsBackup.FontSizeFirstPanel : settingsBackup.FontSizeSecondPanel;

                listView.ItemsSource = null;

                foreach (Item item in _isFirstFocused ? _firstItems : _secondItems)
                    item.ImageSize = (_isFirstFocused ? settingsBackup.ImageSizeFirstPanel : settingsBackup.ImageSizeSecondPanel);
                listView.ItemsSource = _isFirstFocused ? _firstItems : _secondItems;
            }
        }

        private void SettingsMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            // ReSharper disable once PossibleInvalidOperationException
            if ((bool) settingsWindow.DialogResult) UpdatePanels();
        }

        private void DeleteFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                .Where(item => item.IsChecked).ToList();
            OperationWindow operationWindow = new OperationWindow("Deleting");
            operationWindow.OnFinish += (o, s) =>
            {
                UpdatePanels(); 
                operationWindow.Close();
            };
            operationWindow.Show();
            operationWindow.Delete(items);

            UpdatePanels();
        }

        private void CopyFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            string newPath = (_isFirstFocused ? SecondPanelPath : FirstPanelPath).Text;
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>()
                .Where(item => item.IsChecked).ToList();

            OperationWindow operationWindow = new OperationWindow("Copying");
            operationWindow.OnFinish += (o, s) =>
            {
                UpdatePanels();
                operationWindow.Close();
            };
            operationWindow.Show();
            operationWindow.Copy(items, newPath);
        }

        private void UpdatePanels()
        {
            FillTable(true, FirstPanelPath.Text);
            FillTable(false, SecondPanelPath.Text);
        }

        private void PanelOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Item item = (Item) (_isFirstFocused ? FirstPanel : SecondPanel).SelectedItem;

                (_isFirstFocused ? FirstInfo : SecondInfo).Text =
                    "Path: " + item.FullName + (item is FileItem ? $"\tSize: {item.SizeItem}" : "") +
                    (item is BackItem ? "" : $"\t Last accessed: {item.LastAccessed}");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void PanelOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
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

        private void Rename(Item item, bool isFirst)
        {
            ChooseNameWindow chooseNameWindow = new ChooseNameWindow("Rename: ", item.Name);
            if (chooseNameWindow.ShowDialog() == true)
            {
                item.Rename(chooseNameWindow.NewName);
                FillTable(isFirst, isFirst ? FirstPanelPath.Text : SecondPanelPath.Text);
            }
        }

        private void CreateNewFolder()
        {
            DirectoryItem directoryItem =
                new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);

            ChooseNameWindow chooseNameWindow = new ChooseNameWindow("Create new folder");
            if (chooseNameWindow.ShowDialog() == true)
            {
                directoryItem.Create(directoryItem.FullName, chooseNameWindow.NewName, Item.DIRECTORY);
            }
            FillTable(_isFirstFocused, _isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
        }

        private void CreateNewFile()
        {
            DirectoryItem directoryItem =
                new DirectoryItem(_isFirstFocused ? FirstPanelPath.Text : SecondPanelPath.Text);
            ChooseNameWindow chooseNameWindow = new ChooseNameWindow("Create new file");
            if (chooseNameWindow.ShowDialog() == true)
            {
                directoryItem.Create(directoryItem.FullName, chooseNameWindow.NewName, Item.FILE);
            }
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
            CheckPanelFocus();

            bool isFirst = ((ListView) sender).Name == "FirstPanel";
            string name = ((Item) ((ListView) sender).SelectedItem).Name;
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
                FillTable(name == "FirstPanelPathStatic", path);
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
                ((ListViewItem) FirstPanel.ItemContainerGenerator.ContainerFromIndex(0)).Focus();
                SecondPanel.SelectedItem = null;
            }

            _isFirstFocused = !_isFirstFocused;
            CheckPanelFocus();
        }

        private void FillTable(bool isFirst, string path)
        {
            try
            {
                DirectoryItem unused = new DirectoryItem(path);
                //fails here if no access to directory

                if (isFirst)
                {
                    FirstPanel.ItemsSource = null;
                    _firstDirectoryItem = new DirectoryItem(path);
                    _firstItems = new List<Item>(_firstDirectoryItem.Subs);
                    for (int i = 0; i < _firstItems.Count; i++)
                    {
                        _firstItems[i].Index = i;
                        _firstItems[i].ImageSize = SettingsBackup.ImageSizeFirstPanelStatic;
                    }

                    FirstPanelPath.Text = _firstDirectoryItem.FullName;
                    FirstPanel.FontSize = SettingsBackup.FontSizeFirstPanelStatic;
                    FirstPanel.ItemsSource = _firstItems;
                }
                else
                {
                    SecondPanel.ItemsSource = null;
                    _seconDirectoryItem = new DirectoryItem(path);
                    _secondItems = new List<Item>(_seconDirectoryItem.Subs);
                    for (int i = 0; i < _secondItems.Count; i++)
                    {
                        _secondItems[i].Index = i;
                        _secondItems[i].ImageSize = SettingsBackup.ImageSizeSecondPanelStatic;
                    }

                    SecondPanelPath.Text = _seconDirectoryItem.FullName;
                    SecondPanel.FontSize = SettingsBackup.FontSizeSecondPanelStatic;
                    SecondPanel.ItemsSource = _secondItems;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CheckBoxTemplate_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox) sender;
            ListViewItem listViewItem =
                (ListViewItem) (_isFirstFocused ? FirstPanel : SecondPanel).ItemContainerGenerator.ContainerFromIndex(
                    int.Parse(checkBox.Uid));
            if (checkBox.IsChecked != null && (bool) checkBox.IsChecked)
                try
                {
                    listViewItem.Background =
                        // ReSharper disable once PossibleNullReferenceException
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(SettingsBackup.SelectedItemColorStatic));
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Can't find standart color");
                    listViewItem.Background =
                        new SolidColorBrush(Colors.Blue);
                }
            else
                listViewItem.Background = new SolidColorBrush(Colors.White);
        }

        private void ColumnSort(object sender, RoutedEventArgs e)
        {
            string header = ((GridViewColumnHeader) sender).Content.ToString();

            if (header == "Date modified")
                header = "DateModified";
            if (header == "Type")
                header = "Extension";

            SortDescription desc = new SortDescription(header, ListSortDirection.Descending);
            SortDescription asc = new SortDescription(header, ListSortDirection.Ascending);

            if ((_isFirstFocused ? FirstPanel : SecondPanel).Items.SortDescriptions.Contains(desc))
            {
                (_isFirstFocused ? FirstPanel : SecondPanel).Items.SortDescriptions.Clear();
                (_isFirstFocused ? FirstPanel : SecondPanel).Items.SortDescriptions.Add(asc);
            }
            else
            {
                (_isFirstFocused ? FirstPanel : SecondPanel).Items.SortDescriptions.Clear();
                (_isFirstFocused ? FirstPanel : SecondPanel).Items.SortDescriptions.Add(desc);
            }
        }
    }
}