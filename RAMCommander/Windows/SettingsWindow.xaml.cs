using System.Windows;
using System.Windows.Media;
using Lib;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private int _choosedItemId;

        public SettingsWindow()
        {
            InitializeComponent();

            ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColor);

            ResetButton.Click += (sender, args) =>
            {
                SettingsBackup.SetDeafaults();
                switch (_choosedItemId)
                {
                    case 0:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColor);
                        break;
                    case 1:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(SettingsBackup.SelectedItemColor);
                        break;
                }
            };

            ActivatedPanelColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 0;
                ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColor);
            };

            SelectedItemColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 1;
                ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(SettingsBackup.SelectedItemColor);
            };

            OkButton.Click += (sender, args) => DialogResult = true;
            CancelButton.Click += (sender, args) => DialogResult = false;

            ColorPickerCanvas.SelectedColorChanged += ColorPickerCanvasOnSelectedColorChanged;
        }

        private void ColorPickerCanvasOnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            switch (_choosedItemId)
            {
                case 0:
                    SettingsBackup.ActivePanelColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
                case 1:
                    SettingsBackup.SelectedItemColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
            }
        }
    }
}