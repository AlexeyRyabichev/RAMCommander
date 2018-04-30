using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace RAMCommander.Windows
{
    /// <summary>
    /// Interaction logic for ChooseNameWindow.xaml
    /// </summary>
    public partial class ChooseNameWindow : Window
    {
        public string NewName => NameBox.Text;

        public ChooseNameWindow(string operationName, string oldName = "")
        {
            InitializeComponent();
            NameBox.Text = oldName;
            OperationName.Text = operationName;

            NameBox.PreviewKeyDown += NameBoxOnPreviewKeyDown;

            OkButton.Click += (sender, args) => DialogResult = true;
            CancelButton.Click += (sender, args) => DialogResult = false;
        }

        private void NameBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    DialogResult = true;
                    e.Handled = true;
                    break;
                case Key.Escape:
                    DialogResult = false;
                    e.Handled = true;
                    break;
            }
        }
    }
}
