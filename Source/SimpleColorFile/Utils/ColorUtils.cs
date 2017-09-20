//----------------------------------------------------------------------------
//
// <copyright file="ColorUtils.cs" company="Appliberated">
// Copyright (c) 2017 Appliberated
// https://appliberated.com
// Licensed under the MIT. See LICENSE file in the project root for full license information.
// </copyright>
//
//---------------------------------------------------------------------------

namespace SimpleColorFile.Utils
{
    using System;
    using SDColor = System.Drawing.Color;
    using SWMColor = System.Windows.Media.Color;

    /// <summary>
    /// Provides a collection of color helper functions.
    /// </summary>
    public static class ColorUtils
    {
        private static Random random = new Random();

        /// <summary>
        /// Converts the the specified System.Windows.Media.Color structure to a System.Drawing.Color structure.
        /// </summary>
        /// <param name="mediaColor">The System.Windows.Media.Color structure to convert.</param>
        /// <returns>The converted System.Drawing.Color structure.</returns>
        public static SDColor ToSDColor(SWMColor mediaColor) =>
            SDColor.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

        /// <summary>
        /// Translates the specified System.Drawing.Color structure to an HTML string color representation.
        /// </summary>
        /// <param name="color">The System.Drawing.Color structure to translate.</param>
        /// <returns>The string that represents the HTML color.</returns>
        public static string ToHtml(SDColor color) => System.Drawing.ColorTranslator.ToHtml(color);

        /// <summary>
        /// Translates the specified System.Windows.Media.Color structure to an HTML string color representation.
        /// </summary>
        /// <param name="color">The System.Windows.Media.Color structure to translate.</param>
        /// <returns>The string that represents the HTML color.</returns>
        public static string ToHtml(SWMColor color) => System.Drawing.ColorTranslator.ToHtml(ToSDColor(color));

        /// <summary>
        /// Returns a random color value.
        /// </summary>
        /// <returns>A random System.Windows.Media.Color value.</returns>
        public static SWMColor RandomColor() =>
            SWMColor.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
    }
}
