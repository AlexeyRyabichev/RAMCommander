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

namespace RAMCommander.Windows
{
    /// <summary>
    /// Interaction logic for GroupRenamingWindow.xaml
    /// </summary>
    public partial class GroupRenamingWindow : Window
    {
        public string NewName => NameBox.Text;

        public GroupRenamingWindow()
        {
            InitializeComponent();

            NameBox.PreviewKeyDown += NameBoxOnPreviewKeyDown;

            HelpBox.Text = "Write new name, using this template\n" +
                           "{DATE} - last modify date\n" +
                           "{NAME} - original name\n" +
                           "{PATH} - full path to item\n" +
                           "{SIZE} - item size\n" +
                           "Example: \"{NAME}{SIZE}additionalWord\"";
            
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
