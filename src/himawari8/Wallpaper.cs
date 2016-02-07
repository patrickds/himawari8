using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace himawari8
{
    public static class Wallpaper
    {
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        private const string WALLPAPER_STYLE_KEY = "WallpaperStyle";
        private const string TILE_WALLPAPER_KEY = "TileWallpaper";

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static void Set(string wallpaperPath, WallpaperStyle style)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            key.SetValue(WALLPAPER_STYLE_KEY, style.GetWallpaperStyleKeyValue());
            key.SetValue(TILE_WALLPAPER_KEY, style.GetTileWallpaperKeyValue());

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                wallpaperPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
