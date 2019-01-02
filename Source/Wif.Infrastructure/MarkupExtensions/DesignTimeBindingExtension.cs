using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.MarkupExtensions
{
    /// <summary>
    /// Defines the <see cref="DesignTimeBindingExtension" />
    ///     http://www.singulink.com/CodeIndex/post/wpf-visibility-binding-with-design-time-control
    /// </summary>
    public class DesignTimeBindingExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the DesignValue
        /// Gets or sets the DesignValue
        /// </summary>
        public object DesignValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// Gets or sets the Value
        /// Gets or sets the Value
        /// </summary>
        [ConstructorArgument("value")]
        public object Value { get; set; } = DependencyProperty.UnsetValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeBindingExtension"/> class.
        /// </summary>
        public DesignTimeBindingExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeBindingExtension"/> class.
        /// </summary>
        /// <param name="value">The <see cref="object" /></param>
        public DesignTimeBindingExtension(object value)
        {
            Value = value;
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
            var provideValueTarget = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            var property = provideValueTarget.TargetProperty as DependencyProperty;
            var target = provideValueTarget.TargetObject as DependencyObject;

            if (target == null || property == null)
                return this;

            var value = DesignerProperties.GetIsInDesignMode(target) && DesignValue != DependencyProperty.UnsetValue
                    ? DesignValue
                    : Value;

            if (value == DependencyProperty.UnsetValue || value == null)
                return value;

            if (value is MarkupExtension)
                return ((MarkupExtension) value).ProvideValue(serviceProvider);

            if (property.PropertyType.IsInstanceOfType(value))
                return value;

            return TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value);
        }

        #endregion
    }
}