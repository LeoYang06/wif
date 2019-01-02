﻿using System;
using System.Globalization;
using Frontier.Wif.Infrastructure.MarkupExtensions;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// Defines the <see cref="BoolToValueConverter{T, TParameter}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParameter"></typeparam>
    public abstract class BoolToValueConverter<T, TParameter> : MarkupExtensionConverter<T> where T : class, new()
    {
        #region Properties

        /// <summary>
        /// Gets the FalseValue
        /// 获取或设置True对应的值。
        /// </summary>
        private TParameter FalseValue { get; }

        /// <summary>
        /// Gets a value indicating whether IsInverted
        /// 获取或设置是否反向转换。
        /// </summary>
        private bool IsInverted { get; }

        /// <summary>
        /// Gets the TrueValue
        /// 获取或设置False对应的值。
        /// </summary>
        private TParameter TrueValue { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToValueConverter{T, TParameter}"/> class.
        /// </summary>
        /// <param name="trueValue">The <see cref="TParameter" /></param>
        /// <param name="falseValue">The <see cref="TParameter" /></param>
        /// <param name="isInverted">The <see cref="bool" /></param>
        protected BoolToValueConverter(TParameter trueValue, TParameter falseValue, bool isInverted = false)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
            IsInverted = isInverted;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var returnValue = FalseValue;

            if (value is bool)
                if (IsInverted)
                    returnValue = (bool) value ? FalseValue : TrueValue;
                else
                    returnValue = (bool) value ? TrueValue : FalseValue;

            return returnValue;
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="targetType">The <see cref="Type" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="culture">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="object" /></returns>
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var returnValue = false;

            if (value != null)
                if (IsInverted)
                    returnValue = value.Equals(FalseValue);
                else
                    returnValue = value.Equals(TrueValue);

            return returnValue;
        }

        #endregion
    }
}