namespace SimpleColorFile.Utils
{
    using System;
    using SDColor = System.Drawing.Color;
    using SWMColor = System.Windows.Media.Color;

    public static class ColorUtils
    {
        private static Random random = new Random();

        public static SDColor ToSDColor(SWMColor mediaColor) =>
            SDColor.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

        public static SWMColor ToSWMColor(SDColor drawingColor) =>
            SWMColor.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);

        public static string ToHtml(SDColor color) => System.Drawing.ColorTranslator.ToHtml(color);

        public static string ToHtml(SWMColor color) => System.Drawing.ColorTranslator.ToHtml(ToSDColor(color));

        public static SWMColor RandomColor() =>
            SWMColor.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
    }
}
