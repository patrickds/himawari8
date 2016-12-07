using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace himawari8
{
    class Program
    {
        private static System.Threading.Timer _timer;
        static void Main()
        {
#if !DEBUG
            SetRunAtWindowsStartup();
#endif
            UpdateWallpaper();
            StartTask();
            IdleLoop();
        }

        private static void StartTask()
        {
            _timer = new System.Threading.Timer(
                (state) => { UpdateWallpaper(); },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(10));
        }

        private static void IdleLoop()
        {
            bool createdNew;
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "CF2D4313-33DE-489D-9721-6AFF69841DEA", out createdNew);
            var signaled = false;

            if (!createdNew)
            {
                waitHandle.Set();
                return;
            }

            do
            {
                signaled = waitHandle.WaitOne(TimeSpan.FromSeconds(5));
            } while (!signaled);
        }

        private static void UpdateWallpaper()
        {
            var myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var temporaryPath = Path.Combine(myPicturesPath, "Himawari8", "earth.png");

            var image = new Himawari8Image();
            image.Save(temporaryPath);
            image.Dispose();

            if (image.SucceededDownloading)
            {
                Wallpaper.Set(temporaryPath, WallpaperStyle.Centered);
            }
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