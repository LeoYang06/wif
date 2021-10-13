/**************************************************************************
*      File Name：MarkupConverter.cs
*    Description：MarkupConverter.cs class description...
*      Copyright：Copyright ? 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Frontier.Wif.Core.Generic;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// MarkupConverter是一个可用于ValueConverter的MarkupExtension。
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupConverter : MarkupExtension, IValueConverter
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        #endregion
    }

    /// <summary>
    /// MarkupConverter是一个可用于ValueConverter的泛型MarkupExtension。默认单例。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return SingletonProvider<T>.Instance;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        #endregion
    }

    /// <summary>
    /// MarkupExtensionConverter是一个可用于ValueConverter的泛型MarkupExtension。默认非单例。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MarkupExtensionConverter<T> : MarkupConverter<T> where T : class, new()
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}