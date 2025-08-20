using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Forta.Core.Utils
{
    public static class ImageLoader
    {
        public static ImageSource FromResource(Assembly asm, string resourceName)
        {
            using (var s = asm.GetManifestResourceStream(resourceName))
            {
                if (s == null) return null;
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = s;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
        }
    }
}

