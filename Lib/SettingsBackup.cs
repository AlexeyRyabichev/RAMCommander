using System;
using System.Runtime.Serialization;
using Lib.Properties;

namespace Lib
{
    public class SettingsBackup
    {
        public const string SettingsFileName = "settings.json";
        public static string ActivePanelColorStatic = Resources.ActivePanelColorDefault;
        public static string SelectedItemColorStatic = Resources.SelectedItemColorDefault;
        public static int ImageSizeFirstPanelStatic = 20;
        public static int ImageSizeSecondPanelStatic = 20;
        public static string FirstPanelPathStatic = @"C:\";
        public static string SecondPanelPathStatic = @"C:\";
        public static int FontSizeFirstPanelStatic = 10;
        public static int FontSizeSecondPanelStatic = 10;

        public SettingsBackup()
        {
        }

        public string ActivePanelColor
        {
            get => ActivePanelColorStatic;
            set => ActivePanelColorStatic = value;
        }

        public string SelectedItemColor
        {
            get => SelectedItemColorStatic;
            set => SelectedItemColorStatic = value;
        }

        public int ImageSizeFirstPanel
        {
            get => ImageSizeFirstPanelStatic;
            set => ImageSizeFirstPanelStatic = value;
        }

        public int FontSizeFirstPanel
        {
            get => FontSizeFirstPanelStatic;
            set => FontSizeFirstPanelStatic = value;
        }

        public int FontSizeSecondPanel
        {
            get => FontSizeSecondPanelStatic;
            set => FontSizeSecondPanelStatic = value;
        }

        public int ImageSizeSecondPanel
        {
            get => ImageSizeSecondPanelStatic;
            set => ImageSizeSecondPanelStatic = value;
        }

        public string FirstPanelPath
        {
            get => FirstPanelPathStatic;
            set => FirstPanelPathStatic = value;
        }

        public string SecondPanelPath
        {
            get => SecondPanelPathStatic;
            set => SecondPanelPathStatic = value;
        }

        public SettingsBackup(string activePanelColor, string selectedItemColor, int imageSizeFirstPanel, int imageSizeSecondPanel, string firstPanelPath, string secondPanelPath, int fontSizeFirstPanel, int fontSizeSecondPanel)
        {
            ActivePanelColor = activePanelColor;
            SelectedItemColor = selectedItemColor;
            ImageSizeFirstPanel = imageSizeFirstPanel;
            ImageSizeSecondPanel = imageSizeSecondPanel;
            FirstPanelPath = firstPanelPath;
            SecondPanelPath = secondPanelPath;
            FontSizeFirstPanel = fontSizeFirstPanel;
            FontSizeSecondPanel = fontSizeSecondPanel;
        }

        public static void SetDefaults()
        {
            ActivePanelColorStatic = Resources.ActivePanelColorDefault;
            SelectedItemColorStatic = Resources.SelectedItemColorDefault;
            ImageSizeFirstPanelStatic = 20;
            ImageSizeSecondPanelStatic = 20;
            FirstPanelPathStatic = @"C:\";
            SecondPanelPathStatic = @"C:\";
            FontSizeFirstPanelStatic = 10;
            FontSizeSecondPanelStatic = 10;
        }
    }
}