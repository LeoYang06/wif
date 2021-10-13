/**************************************************************************
*      File Name：InverseBooleanConverter.cs
*    Description：InverseBooleanConverter.cs class description...
*      Copyright：Copyright © 2021 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2021/8/30
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using Frontier.Wif.Infrastructure.MarkupExtensions;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// 接受一个bool值，并返回其反值。
    /// </summary>
    public class InverseBooleanConverter : MarkupConverter
    {
        /// <summary>
        /// 将一个布尔值转换为其反值。
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>The inverted boolean value.</returns>
        protected override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var boolValue = (bool)value;
            return !boolValue;
        }

        /// <summary>
        /// 将一个布尔值转换为其反值。
        /// </summary>
        /// <param name="value">The parameter is not used.</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>The parameter is not used.</returns>
        protected override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var boolValue = (bool)value;
            return !boolValue;
        }
    }
}