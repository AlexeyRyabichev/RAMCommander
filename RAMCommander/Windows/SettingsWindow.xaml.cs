using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Lib;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Windows.Media.FontFamily;

namespace RAMCommander.Windows
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private int _choosedItemId;

        public SettingsWindow(FontFamily currentFont)
        {
            InitializeComponent();

            ColorPickerCanvas.SelectedColor =
                (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColorStatic);

            ResetButton.Click += (sender, args) =>
            {
                SettingsBackup.SetDefaults();
                switch (_choosedItemId)
                {
                    case 0:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColorStatic);
                        break;
                    case 1:
                        ColorPickerCanvas.SelectedColor =
                            (Color?) ColorConverter.ConvertFromString(SettingsBackup.SelectedItemColorStatic);
                        break;
                }
            };

            ActivatedPanelColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 0;
                ColorPickerCanvas.SelectedColor =
                    (Color?) ColorConverter.ConvertFromString(SettingsBackup.ActivePanelColorStatic);
            };

            SelectedItemColorButton.Click += (sender, args) =>
            {
                _choosedItemId = 1;
                ColorPickerCanvas.SelectedColor =
                    (Color?) ColorConverter.ConvertFromString(SettingsBackup.SelectedItemColorStatic);
            };
            ChooseFontFamilyButton.Click += (sender, args) =>
            {
                FontDialog dialog = new FontDialog {Font = new Font(currentFont.Source, 20)};
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    SettingsBackup.FontFamilyStatic = new FontFamily(dialog.Font.Name);
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
                    SettingsBackup.ActivePanelColorStatic = ColorPickerCanvas.SelectedColor.ToString();
                    break;
                case 1:
                    SettingsBackup.SelectedItemColorStatic = ColorPickerCanvas.SelectedColor.ToString();
                    break;
            }
        }
    }
}