/**************************************************************************
*      File Name：ColorConvertExtensions.cs
*    Description：ColorConvertExtensions.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.Drawing;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 颜色类型转换帮助类。
    /// </summary>
    public static class ColorConvertExtensions
    {
        /// <summary>
        /// The ToBrush
        /// </summary>
        /// <param name="color">The color<see cref="System.Windows.Media.Color" /></param>
        /// <returns>The <see cref="System.Windows.Media.Brush" /></returns>
        public static Brush ToBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// The ToBrush
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string" /></param>
        /// <returns>The <see cref="Brush" /></returns>
        public static Brush ToBrush(this string htmlColor)
        {
            var brushConverter = new BrushConverter();
            var brush = (Brush) brushConverter.ConvertFromString(htmlColor);
            return brush;
        }

        /// <summary>
        /// The ToColor
        /// </summary>
        /// <param name="brush">The brush<see cref="Brush" /></param>
        /// <returns>The <see cref="Color" /></returns>
        public static Color ToColor(this Brush brush)
        {
            var color = ((SolidColorBrush) brush).Color;
            return color;
        }

        /// <summary>
        /// The ToColor
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string" /></param>
        /// <returns>The <see cref="Color" /></returns>
        public static Color ToColor(this string htmlColor)
        {
            var convertFromString = ColorConverter.ConvertFromString(htmlColor);
            if (convertFromString == null)
                return default;
            var color = (Color) convertFromString;
            return color;
        }

        /// <summary>
        /// The ToDrawingColor
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string" /></param>
        /// <returns>The <see cref="System.Drawing.Color" /></returns>
        public static System.Drawing.Color ToDrawingColor(this string htmlColor)
        {
            return ColorTranslator.FromHtml(htmlColor);
        }
    }
}