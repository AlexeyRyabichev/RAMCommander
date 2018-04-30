using System;

namespace Lib
{
    [Serializable]
    public static class SettingsBackup
    {
        public static string ActivePanelColor = Properties.Resources.ActivePanelColorDefault;
        public static string SelectedItemColor = Properties.Resources.SelectedItemColorDefault;
        public static int ImageSizeFirstPanel = 20;
        public static int ImageSizeSecondPanel = 20;

        public static void SetDeafaults()
        {
            ActivePanelColor = Properties.Resources.ActivePanelColorDefault;
            SelectedItemColor = Properties.Resources.SelectedItemColorDefault;
            ImageSizeFirstPanel = 20;
            ImageSizeSecondPanel = 20;
        }

    }
}