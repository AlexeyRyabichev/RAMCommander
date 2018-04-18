using System.IO;

namespace Lib.ItemsTypes
{
    public class BackItem : Item
    {
        public BackItem()
        {
            Type = BACK;
            //ImageSourceConverter converter = new ImageSourceConverter();
            //TypeImageSource = (ImageSource) converter.ConvertFromString(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "Resources\\back48.png");
            //TypeImageSource = (Properties.Resources.back48);
            //TypeImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/back48.png"));
            //TypeImageSource = new BitmapImage(new Uri("C:\\Users\\alexe\\Code\\RAMCommander\\Lib\\Resources\\folder48.png", UriKind.Absolute));
            //TypeImageSource = new BitmapImage(new Uri("pack://application:,,,/" + GetType().Assembly.GetName().Name + "/Res/back48.png"));
            //TypeImageSource = "pack://application:,,,/" + GetType().Assembly.GetName().Name + "/Resources/back48.png";
            //TypeImageSource = @"C:\Users\alexe\Code\RAMCommander\RAMCommander\Resources\back48.png";
            //TypeImageSource = @"..\..\Resources\back48.png";
            //TypeImageSource = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "\\Resources\\back48.png");
            //TypeImageSource = Properties.Resources.back48;
            //TypeImageSource = @"C:\Users\alexe\Code\RAMCommander\Res\back48.png";
            TypeImageSource = Path.GetFullPath("../../Resources/back48.png");
            Name = @"\..";
        }
    }
}