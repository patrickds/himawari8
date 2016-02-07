using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace himawari8
{
    internal class Himawari8Image
    {
        private const string HIMAWARI8_URL = @"http://himawari8-dl.nict.go.jp/himawari8/img/D531106";
        private const string BLOCK_SEPARATOR = @"_";
        private const string URL_IMAGE_EXTENSION = @".png";
        private const string LEVEL = "4d";
        private const int BLOCK_SIZE = 550;
        private const int NUMBER_OF_IMAGE_BLOCKS = 4;

        private DateTime _time;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private string _baseUrl;
        private bool success = true;

        public Himawari8Image()
        {
            _time = Himawari8UpdateInterval.GetLasHimawari8UpdateTime();
            BuildBaseUrl();
            CreateImage();
        }

        private void BuildBaseUrl()
        {
            var year = _time.ToString("yyyy");
            var month = _time.ToString("MM");
            var day = _time.ToString("dd");
            var time = _time.ToString("HHmmss");

            _baseUrl = string.Concat( HIMAWARI8_URL, "/", LEVEL, "/", BLOCK_SIZE,"/", year,"/", month, "/", day, "/", time);
        }

        private void CreateImage()
        {
            var imageSize = BLOCK_SIZE * NUMBER_OF_IMAGE_BLOCKS;
            _bitmap = new Bitmap(imageSize, imageSize);

            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.Black);

            for (int i = 0; i < NUMBER_OF_IMAGE_BLOCKS; i++)
            {
                for (int j = 0; j < NUMBER_OF_IMAGE_BLOCKS; j++)
                {
                    using (var imageBlock = DownloadImageBlock(i, j))
                    {
                        var x = i * BLOCK_SIZE;
                        var y = j * BLOCK_SIZE;
                        _graphics.DrawImage(imageBlock, x, y, BLOCK_SIZE, BLOCK_SIZE);
                    }
                }
            }
        }

        private Image DownloadImageBlock(int i, int j)
        {
            var currentUrl = string.Concat( _baseUrl, BLOCK_SEPARATOR, i, BLOCK_SEPARATOR, j, URL_IMAGE_EXTENSION);

            try
            {
                var request = WebRequest.Create(currentUrl);
                using (var response = request.GetResponse())
                {
                    Console.WriteLine("Downloading " + currentUrl);
                    return Image.FromStream(response.GetResponseStream());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("\nFailed downloading " + currentUrl);
                success = false;
                return new Bitmap(BLOCK_SIZE, BLOCK_SIZE);
            }
        }

        public void Save(string filename)
        {
            if (!success)
                return;
            
            var directory = Path.GetDirectoryName(filename);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _bitmap.Save(filename);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
            _graphics.Dispose();
        }
    }
}