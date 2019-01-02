using System.Windows;
using System.Windows.Data;

namespace Frontier.Wif.Infrastructure.Converters
{
    /// <summary>
    /// Defines the <see cref="BoolToVisibilityConverter" />
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : BoolToValueConverter<BoolToVisibilityConverter, Visibility>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToVisibilityConverter"/> class.
        /// </summary>
        public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToVisibilityConverter"/> class.
        /// </summary>
        /// <param name="isInverted">是否反向转换。</param>
        public BoolToVisibilityConverter(bool isInverted) : base(Visibility.Visible, Visibility.Collapsed, isInverted)
        {
        }

        #endregion
    }
}