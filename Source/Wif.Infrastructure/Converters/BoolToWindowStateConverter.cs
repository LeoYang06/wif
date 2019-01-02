using System;
using System.Windows;
using System.Windows.Data;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// Defines the <see cref="BoolToWindowStateConverter" />
    /// </summary>
    [ValueConversion(typeof(bool), typeof(WindowState))]
    public class BoolToWindowStateConverter : BoolToValueConverter<BoolToWindowStateConverter, WindowState>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToWindowStateConverter"/> class.
        /// </summary>
        public BoolToWindowStateConverter() : base(WindowState.Maximized, WindowState.Minimized)
        {
        }

        #endregion

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

        #endregion
    }
}