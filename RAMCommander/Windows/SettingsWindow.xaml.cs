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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private int _choosedItemId = 0;
        public SettingsWindow()
        {
            InitializeComponent();

            ColorPickerCanvas.SelectedColor = (Color?)ColorConverter.ConvertFromString(Lib.Colors.ActivePanelColor);

            ResetButton.Click += (sender, args) =>
            {
                Lib.Colors.SetDeafaults();
                switch (_choosedItemId)
                {
                    case 0:
                        ColorPickerCanvas.SelectedColor =
                            (Color?)ColorConverter.ConvertFromString(Lib.Colors.ActivePanelColor);
                        break;
                    case 1:
                        ColorPickerCanvas.SelectedColor =
                            (Color?)ColorConverter.ConvertFromString(Lib.Colors.SelectedItemColor);
                        break;
                }
            };

            ActivatedPanelColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 0; 
                ColorPickerCanvas.SelectedColor = (Color?) ColorConverter.ConvertFromString(Lib.Colors.ActivePanelColor);
            };

            SelectedItemColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 1;
                ColorPickerCanvas.SelectedColor = (Color?)ColorConverter.ConvertFromString(Lib.Colors.SelectedItemColor);
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
                    Lib.Colors.ActivePanelColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
                case 1:
                    Lib.Colors.SelectedItemColor = ColorPickerCanvas.SelectedColor.ToString();
                    break;
            }
        }
    }
}
