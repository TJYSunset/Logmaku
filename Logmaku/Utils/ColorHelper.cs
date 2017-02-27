using System.Windows.Media;

namespace Logmaku.Utils
{
    public static class ColorHelper
    {
        public static Color CorrectAlphaChannel(this Color primary, Color sample)
        {
            primary.A = sample.A;
            return primary;
        }
    }
}