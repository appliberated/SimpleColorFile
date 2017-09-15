using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace SimpleColorFile.Utils
{
    public static class ColorUtils
    {
        public static SDColor ToSDColor(SWMColor mediaColor) =>
            SDColor.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

        public static SWMColor ToSWMColor(SDColor drawingColor) =>
            SWMColor.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);

        public static string ToHtml(SDColor color) => System.Drawing.ColorTranslator.ToHtml(color);
    }
}
