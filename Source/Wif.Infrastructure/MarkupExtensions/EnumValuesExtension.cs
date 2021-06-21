using System;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="EnumValuesExtension" />
    /// http://tlevesque.developpez.com/dotnet/wpf-markup-extensions/
    /// </summary>
    [MarkupExtensionReturnType(typeof(Array))]
    public class EnumValuesExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the EnumType
        /// Gets or sets the EnumType
        /// </summary>
        [ConstructorArgument("enumType")]
        public Type EnumType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValuesExtension"/> class.
        /// </summary>
        public EnumValuesExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValuesExtension"/> class.
        /// </summary>
        /// <param name="enumType">The <see cref="Type" /></param>
        public EnumValuesExtension(Type enumType)
        {
            EnumType = enumType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ProvideValue
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /></param>
        /// <returns>The <see cref="object" /></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }

        #endregion
    }
}