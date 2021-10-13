/**************************************************************************
*      File Name：MarkupMultiConverter.cs
*    Description：MarkupMultiConverter.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
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
    /// Defines the <see cref="MarkupMultiConverter" />
    /// </summary>
    [MarkupExtensionReturnType(typeof(MarkupMultiConverter))]
    public abstract class MarkupMultiConverter : MarkupExtension, IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/></param>
        /// <returns>The <see cref="object"/></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="values">The values<see cref="object[]"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        protected abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetTypes">The targetTypes<see cref="Type[]"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object[]"/></returns>
        protected abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter,
                CultureInfo culture);

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="values">The values<see cref="object[]"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(values, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetTypes">The targetTypes<see cref="Type[]"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object[]"/></returns>
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter,
                CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetTypes, parameter, culture);
            }
            catch
            {
                return new object[0];
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="MarkupMultiConverter{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MarkupExtensionReturnType(typeof(MarkupMultiConverter))]
    public abstract class MarkupMultiConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/></param>
        /// <returns>The <see cref="object"/></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return SingletonProvider<T>.Instance;
        }

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="values">The values<see cref="object[]"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        protected abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetTypes">The targetTypes<see cref="Type[]"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object[]"/></returns>
        protected abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter,
                CultureInfo culture);

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="values">The values<see cref="object[]"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert(values, targetType, parameter, culture);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetTypes">The targetTypes<see cref="Type[]"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object[]"/></returns>
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter,
                CultureInfo culture)
        {
            try
            {
                return ConvertBack(value, targetTypes, parameter, culture);
            }
            catch
            {
                return new object[0];
            }
        }

        #endregion
    }
}