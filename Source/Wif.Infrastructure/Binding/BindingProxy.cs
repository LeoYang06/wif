using System.Windows;

namespace Frontier.Wif.Infrastructure.Binding
{
    /// <summary>
    /// 绑定代理类。
    /// http://www.thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
    /// </summary>
    public class BindingProxy : Freezable
    {
        #region Fields

        /// <summary>
        /// Defines the DataProperty
        /// </summary>
        public static readonly DependencyProperty DataProperty =
                DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CreateInstanceCore
        /// </summary>
        /// <returns>The <see cref="Freezable"/></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion
    }
}