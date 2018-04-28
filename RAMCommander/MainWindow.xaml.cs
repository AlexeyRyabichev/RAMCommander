using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Lib.ItemsTypes;
using RAMCommander.Windows;
using Colors = Lib.Colors;

namespace RAMCommander
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Style _focusedStyle;
        private readonly Style _standardStyle;
        private DirectoryItem _firstDirectoryItem;
        private List<Item> _firstItems;
        private bool _isFirstFocused;
        private DirectoryItem _seconDirectoryItem;
        private List<Item> _secondItems;
        public string ImageRefreshSource;

        public MainWindow()
        {
            //TODO: Zoom (additional) 

            Closing += (sender, args) =>
            {
                if (File.Exists("instances.txt"))
                    File.Delete("instance.txt");
                File.WriteAllLines("instance.txt",
                    new[]
                    {
                        FirstPanelPath.Text, SecondPanelPath.Text, Colors.ActivePanelColor, Colors.SelectedItemColor
                    });
            };
            InitializeComponent();

            FirstPanelButton.Content = "Refresh";
            SecondPanelButton.Content = "Refresh";

            FirstPanelButton.Click += (sender, args) => FillTable(true, FirstPanelPath.Text);
            SecondPanelButton.Click += (sender, args) => FillTable(false, SecondPanelPath.Text);

            FirstPanel.SelectionChanged += PanelOnSelectionChanged;
            SecondPanel.SelectionChanged += PanelOnSelectionChanged;

            //FirstPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            //SecondPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

            _focusedStyle = new Style(typeof(Control));
            _focusedStyle.Setters.Add(new Setter(BackgroundProperty,
                new SolidColorBrush((Color) ColorConverter.ConvertFromString(Colors.ActivePanelColor)))); //fae1c0
            //TODO: Change font to bold

            _standardStyle = new Style(typeof(Control));
            _standardStyle.Setters.Add(new Setter(BackgroundProperty,
                new SolidColorBrush(System.Windows.Media.Colors.White)));

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
            RenameFastKey.Click += (sender, args) =>
                Rename((Item) (_isFirstFocused ? FirstPanel.SelectedItem : SecondPanel.SelectedItem), _isFirstFocused);
            CopyFastKey.Click += CopyFastKeyOnClick;

            SettingsMenuItem.Click += SettingsMenuItemOnClick;

            #endregion

            string firstPath = @"C:\";
            string secondPath = @"C:\";

            if (File.Exists("instance.txt"))
            {
                firstPath = File.ReadAllLines("instance.txt")[0];
                secondPath = File.ReadAllLines("instance.txt")[1];
                Colors.ActivePanelColor = File.ReadAllLines("instance.txt")[2];
                Colors.SelectedItemColor = File.ReadAllLines("instance.txt")[3];
                CheckPanelFocus();
            }

            FillTable(true, firstPath);
            FillTable(false, secondPath);
            _isFirstFocused = true;
            CheckPanelFocus();
            FirstPanel.Focus();
        }

        private void SettingsMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            if ((bool) settingsWindow.DialogResult) CheckPanelFocus();
        }

        private void DeleteFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            foreach (Item item in (_isFirstFocused ? FirstPanel : SecondPanel).Items)
                if (item.IsChecked)
                    item.Delete();

            UpdatePanels();
        }

        private void CopyFastKeyOnClick(object sender, RoutedEventArgs e)
        {
            string newPath = (_isFirstFocused ? SecondPanelPath : FirstPanelPath).Text;
            //List<Item>  items = new List<Item>();
            //foreach (Item item in (_isFirstFocused ? FirstPanel : SecondPanel).Items)
            //{
            //    if (item.IsChecked)
            //        items.Add(item);
            //}
            List<Item> items = (_isFirstFocused ? FirstPanel : SecondPanel).Items.Cast<Item>().Where(item => item.IsChecked).ToList();

            OperationWindow operationWindow = new OperationWindow("Copying");
            operationWindow.OnFinish += (o, s) => UpdatePanels();
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
            Item currentItem = (Item) currentListView.SelectedItem;
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
            Style FocusedStyle = new Style(typeof(Control));
            FocusedStyle.Setters.Add(new Setter(BackgroundProperty,
                new SolidColorBrush((Color) ColorConverter.ConvertFromString(Colors.ActivePanelColor)))); //fae1c0

            if (_isFirstFocused)
            {
                FirstGridView.ColumnHeaderContainerStyle = FocusedStyle;
                SecondGridView.ColumnHeaderContainerStyle = _standardStyle;
            }
            else
            {
                FirstGridView.ColumnHeaderContainerStyle = _standardStyle;
                SecondGridView.ColumnHeaderContainerStyle = FocusedStyle;
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
            ListView listView = (ListView) sender;
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
            ListViewItem listViewItem =
                (ListViewItem) (_isFirstFocused ? FirstPanel : SecondPanel).ItemContainerGenerator.ContainerFromIndex(
                    int.Parse(checkBox.Uid));
            if (checkBox.IsChecked != null && (bool) checkBox.IsChecked)
                listViewItem.Background =
                    new SolidColorBrush((Color) ColorConverter.ConvertFromString(Colors.SelectedItemColor));
            else
                listViewItem.Background = new SolidColorBrush(System.Windows.Media.Colors.White);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //if 
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