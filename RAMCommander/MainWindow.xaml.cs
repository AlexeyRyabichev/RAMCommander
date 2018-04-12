using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib;

namespace RAMCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FirstPanelPath.MouseDoubleClick += PanelPathOnMouseDoubleClick;
            SecondPanelPath.MouseDoubleClick += PanelPathOnMouseDoubleClick;

            DirectoryItem firstDirectoryItem = new DirectoryItem(new DirectoryInfo(@"C:\"), true);
            DirectoryItem seconDirectoryItem = new DirectoryItem(new DirectoryInfo(@"C:\Users\alexe\OneDrive\Рабочий стол\HSE_Stuff\KDZ3_DiscreteMath"), true);
            List<Item> firstItems = new List<Item>(firstDirectoryItem.Subs);
            List<Item> secondItems = new List<Item>(seconDirectoryItem.Subs);

            FirstPanel.ItemsSource = firstItems;
            //FirstPanelDirectoriesInfo.ItemsSource = firstItems;
            //SecondPanelDirectoriesInfo.ItemsSource = secondItems;

            //FirstPanelDirectoriesInfo.RowStyle.Resources

        }

        private void PanelPathOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            
        }

        //private void FillTable(bool isFirst)
        //{
        //    DataGrid dataGrid = new DataGrid();
            
        //}
    }
}
