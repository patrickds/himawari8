using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace himawari8
{
    class Program
    {
        static void Main()
        {
#if !DEBUG
            SetRunAtWindowsStartup();
#endif
            var tenMinutesInterval = 600000;
            while (true)
            {
                UpdateWallpaper();
                Thread.Sleep(tenMinutesInterval);
            }
        }

        private static void UpdateWallpaper()
        {
            var myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var temporaryPath = Path.Combine(myPicturesPath, "Himawari8", "earth.png");

            var image = new Himawari8Image();
            image.Save(temporaryPath);
            image.Dispose();

            Wallpaper.Set(temporaryPath, WallpaperStyle.Centered);
        }

        private static void SetRunAtWindowsStartup()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (key.GetValue(nameof(himawari8)) == null)
            {
                key.SetValue(nameof(himawari8), Application.ExecutablePath.ToString());
            }
        }
    }
}