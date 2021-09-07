/**************************************************************************
*      File Name：SimpleDateValidationRule.cs
*    Description：SimpleDateValidationRule.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Globalization;
using System.Windows.Controls;

namespace Frontier.Wif.Infrastructure.ValidationRules
{
    /// <summary>
    /// Defines the <see cref="SimpleDateValidationRule" />
    /// </summary>
    public class SimpleDateValidationRule : ValidationRule
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
            return DateTime.TryParse((value ?? "").ToString(),
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                    out var time)
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, "Invalid date");
        }

        #endregion
    }
}