using System.Windows;
using System.Windows.Input;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for GroupRenamingWindow.xaml
    /// </summary>
    public partial class GroupRenamingWindow : Window
    {
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

        public string NewName => NameBox.Text;

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