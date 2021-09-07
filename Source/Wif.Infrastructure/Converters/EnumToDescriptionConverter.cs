/**************************************************************************
*      File Name：EnumToDescriptionConverter.cs
*    Description：EnumToDescriptionConverter.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows;
using System.Windows.Data;
using Frontier.Wif.Infrastructure.MarkupExtensions;
using Frontier.Wif.Utilities.Extensions;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// 枚举到描述转换器。
    /// </summary>
    public class EnumToDescriptionConverter : MarkupConverter<EnumToDescriptionConverter>
    {
        /// <summary>
        /// 正向转换。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
        protected override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}