using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Frontier.Wif.Core.Collections;
using Frontier.Wif.Infrastructure.MarkupExtensions;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// Defines the <see cref="EnumToVisibilityConverter" />
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    [MarkupExtensionReturnType(typeof(IList))]
    [ValueConversion(typeof(Enum), typeof(Visibility))]
    public class EnumToVisibilityConverter : MarkupExtensionConverter<EnumToVisibilityConverter>
    {
        #region Fields

        /// <summary>
        /// Defines the _items
        /// </summary>
        private EnumCollection _items;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Items
        /// Gets the Items
        /// </summary>
        public EnumCollection Items
        {
            get => _items ?? (_items = new EnumCollection());
            set => _items = value;
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
            return Items.Contains(value)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
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
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}