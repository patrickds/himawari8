namespace himawari8
{
    public enum WallpaperStyle
    {
        Tiled,
        Centered,
        Stretched
    }

    public static class WallpaperStyleExtensions
    {
        public static string GetWallpaperStyleKeyValue(this WallpaperStyle style)
        {
            if (style == WallpaperStyle.Stretched)
            {
                return "2";
            }
            else if (style == WallpaperStyle.Centered)
            {
                return "1";
            }
            else
            {
                return "1";
            }
        }

        public static string GetTileWallpaperKeyValue(this WallpaperStyle style)
        {
            if (style == WallpaperStyle.Stretched)
            {
                return "0";
            }
            else if (style == WallpaperStyle.Centered)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
    }
}
