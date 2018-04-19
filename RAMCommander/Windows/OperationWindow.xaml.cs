using System.Windows;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for OperationWindow.xaml
    /// </summary>
    public partial class OperationWindow : Window
    {
        private const string CURRENTOPERATION = "Current operation: ";
        private int _currentItemProgress;
        private int _totalProgress;

        public OperationWindow(string operationName)
        {
            InitializeComponent();

            CurrentOperationText.Text = CURRENTOPERATION + operationName;
            TotalProgressText.Text = "Total progress";
            CurrentItemProgressText.Text = "Current Item progress";

            TotalProgressBar.Minimum = 0;
            TotalProgressBar.Maximum = 100;
            TotalProgressBar.Value = 0;

            CurrentItemProgressBar.Minimum = 0;
            CurrentItemProgressBar.Maximum = 100;
            CurrentItemProgressBar.Value = 0;
        }

        public string CurrentOperation { get; set; }

        public int CurrentItemProgress
        {
            get => _currentItemProgress;
            set
            {
                _currentItemProgress = value;
                CurrentItemProgressBar.Value = value;
            }
        }

        public int TotalProgress
        {
            get => _totalProgress;
            set
            {
                _totalProgress = value;
                TotalProgressBar.Value = value;
            }
        }
    }
}