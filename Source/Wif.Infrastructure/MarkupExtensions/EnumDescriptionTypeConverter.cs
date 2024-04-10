/**************************************************************************
*      File Name：EnumBindingSourceExtension.cs
*    Description：EnumBindingSourceExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2024/03/08
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// 表示枚举值转描述值的类型转换器。
    /// </summary>
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EnumDescriptionTypeConverter(Type type) : base(type)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description) ? attributes[0].Description : value.ToString();
                    }
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}