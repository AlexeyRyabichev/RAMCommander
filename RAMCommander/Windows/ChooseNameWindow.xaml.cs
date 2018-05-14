using System.Windows;
using System.Windows.Input;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for ChooseNameWindow.xaml
    /// </summary>
    public partial class ChooseNameWindow : Window
    {
        public ChooseNameWindow(string operationName, string oldName = "")
        {
            InitializeComponent();
            NameBox.Text = oldName;
            OperationName.Text = operationName;

            NameBox.PreviewKeyDown += NameBoxOnPreviewKeyDown;

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