using System;
using System.Windows;
using System.Windows.Data;
using Frontier.Wif.Utils.Extensions;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// 枚举到描述转换器。
    /// </summary>
    public class EnumToDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var enumItem = value as Enum;
            string description = enumItem.GetDescription();
            return description;
        }

        /// <summary>
        /// 反向转换。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}