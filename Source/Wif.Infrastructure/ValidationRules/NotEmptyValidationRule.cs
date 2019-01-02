using System.Globalization;
using System.Windows.Controls;

namespace Frontier.Wif.Infrastructure.ValidationRules
{
    /// <summary>
    /// Defines the <see cref="NotEmptyValidationRule" />
    /// </summary>
    public class NotEmptyValidationRule : ValidationRule
    {
        #region Methods

        /// <summary>
        /// The Validate
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="cultureInfo">The cultureInfo<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="ValidationResult"/></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                    ? new ValidationResult(false, "Field is required.")
                    : ValidationResult.ValidResult;
        }

        #endregion
    }
}