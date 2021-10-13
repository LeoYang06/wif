/**************************************************************************
*      File Name：IPAddressValidationRule.cs
*    Description：IPAddressValidationRule.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace Frontier.Wif.Infrastructure.ValidationRules
{
    /// <summary>
    /// Defines the <see cref="IPAddressValidationRule" />
    /// </summary>
    public class IPAddressValidationRule : ValidationRule
    {
        #region Methods

        /// <summary>
        /// The Validate
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        /// <param name="cultureInfo">The <see cref="CultureInfo" /></param>
        /// <returns>The <see cref="ValidationResult" /></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return IPAddress.TryParse(Convert.ToString(value), out var dummy)
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, "Invalid IP address");
        }

        #endregion
    }
}