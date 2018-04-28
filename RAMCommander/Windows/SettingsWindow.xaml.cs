using System.Windows;
using System.Windows.Media;
using Colors = Lib.Colors;

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

            ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(Colors.ActivePanelColor);

            ResetButton.Click += (sender, args) =>
            {
                Colors.SetDeafaults();
                switch (_choosedItemId)
                {
                    case 0:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(Colors.ActivePanelColor);
                        break;
                    case 1:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(Colors.SelectedItemColor);
                        break;
                }
            };

            ActivatedPanelColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 0;
                ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(Colors.ActivePanelColor);
            };

            SelectedItemColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 1;
                ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(Colors.SelectedItemColor);
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
                    Colors.ActivePanelColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
                case 1:
                    Colors.SelectedItemColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
            }
        }
    }
}