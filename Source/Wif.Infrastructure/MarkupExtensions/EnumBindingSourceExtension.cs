/**************************************************************************
*      File Name：EnumBindingSourceExtension.cs
*    Description：EnumBindingSourceExtension.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Markup extension for Enum values.
    /// https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
    /// </summary>
    public class EnumBindingSourceExtension
            : MarkupExtension
    {
        #region Fields

        /// <summary>
        /// Defines the _enumType
        /// </summary>
        private Type _enumType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of the Enum.
        /// </summary>
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != EnumType)
                {
                    if (!Equals(value, null) && !(Nullable.GetUnderlyingType(value) ?? value).IsEnum)
                        throw new ArgumentException("Type must be an Enum.");

                    _enumType = value;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        public EnumBindingSourceExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        /// <param name="enumType">The type of the Enum.</param>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>The values of the Enum.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Equals(EnumType, null))
                throw new InvalidOperationException("The type of the Enum is undefined.");

            var underlyingEnumType = Nullable.GetUnderlyingType(EnumType) ?? EnumType;
            var enumValues = Enum.GetValues(underlyingEnumType);
            if (underlyingEnumType == EnumType)
                return enumValues;

            var nullableEnumValues = Array.CreateInstance(underlyingEnumType, enumValues.Length);
            enumValues.CopyTo(nullableEnumValues, 1);
            return nullableEnumValues;
        }

        #endregion
    }
}