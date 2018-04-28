using System;
using System.Drawing;

namespace Lib
{
    [Serializable]
    public static class Colors
    {
        public static string ActivePanelColor = Properties.Resources.ActivePanelColorDefault;
        public static string SelectedItemColor = Properties.Resources.SelectedItemColorDefault;

        public static void SetDeafaults()
        {
            ActivePanelColor = Properties.Resources.ActivePanelColorDefault;
            SelectedItemColor = Properties.Resources.SelectedItemColorDefault;
        }

    }
}