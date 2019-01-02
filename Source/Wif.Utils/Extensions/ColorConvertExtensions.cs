using System.Windows.Media;

namespace Frontier.Wif.Utils.Extensions
{
    /// <summary>
    /// 颜色类型转换帮助类。
    /// </summary>
    public static class ColorConvertExtensions
    {
        #region Methods

        /// <summary>
        /// The ToBrush
        /// </summary>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <returns>The <see cref="Brush"/></returns>
        public static Brush ToBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// The ToBrush
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string"/></param>
        /// <returns>The <see cref="Brush"/></returns>
        public static Brush ToBrush(this string htmlColor)
        {
            var brushConverter = new BrushConverter();
            var brush = (Brush) brushConverter.ConvertFromString(htmlColor);
            return brush;
        }

        /// <summary>
        /// The ToColor
        /// </summary>
        /// <param name="brush">The brush<see cref="Brush"/></param>
        /// <returns>The <see cref="Color"/></returns>
        public static Color ToColor(this Brush brush)
        {
            var color = ((SolidColorBrush) brush).Color;
            return color;
        }

        /// <summary>
        /// The ToColor
        /// </summary>
        /// <param name="htmlColor">The htmlColor<see cref="string"/></param>
        /// <returns>The <see cref="Color"/></returns>
        public static Color ToColor(this string htmlColor)
        {
            var convertFromString = ColorConverter.ConvertFromString(htmlColor);
            if (convertFromString != null)
            {
                var color = (Color) convertFromString;
                return color;
            }

            return default;
        }

        #endregion
    }
}