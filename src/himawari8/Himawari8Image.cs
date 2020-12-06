using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace himawari8
{
    //http://himawari8.nict.go.jp/himawari8-image.htm
    internal class Himawari8Image
    {
        private const string HIMAWARI8_URL = @"https://himawari8-dl.nict.go.jp/himawari8/img/D531106";
        private const string BLOCK_SEPARATOR = @"_";
        private const string URL_IMAGE_EXTENSION = @".png";
        private const string LEVEL = "4d";
        private const int BLOCK_SIZE = 550;
        private const int NUMBER_OF_IMAGE_BLOCKS = 4;

        private DateTime _time;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private string _baseUrl;

        public bool SucceededDownloading { get; set; } = true;

        public Himawari8Image()
        {
            _time = Himawari8UpdateInterval.GetSynchronizedSunLightTime();
            BuildBaseUrl();
            CreateImage();
        }

        private void BuildBaseUrl()
        {
            var year = _time.ToString("yyyy");
            var month = _time.ToString("MM");
            var day = _time.ToString("dd");
            var time = _time.ToString("HHmmss");

            _baseUrl = string.Concat(HIMAWARI8_URL, "/", LEVEL, "/", BLOCK_SIZE, "/", year, "/", month, "/", day, "/", time);
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
            var currentUrl = string.Concat(_baseUrl, BLOCK_SEPARATOR, i, BLOCK_SEPARATOR, j, URL_IMAGE_EXTENSION);

            try
            {
                var request = WebRequest.Create(currentUrl);
                var response = request.GetResponse();
                var blockImage = Image.FromStream(response.GetResponseStream());
                response.Dispose();
                Console.WriteLine(currentUrl);
                return blockImage;
            }
            catch (Exception)
            {
                SucceededDownloading = false;
                return new Bitmap(BLOCK_SIZE, BLOCK_SIZE);
            }
        }

        public void Save(string filename)
        {
            if (!SucceededDownloading)
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