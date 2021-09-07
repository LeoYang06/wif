using System.Windows;
using System.Windows.Controls;
using Wif.Demo.Common.Interfaces;

namespace Wif.Demo.Common
{
    public class UserControlBase<TViewModel> : UserControl, IViewFor<TViewModel> where TViewModel : class
    {
        /// <summary>
        /// The view model dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(UserControlBase<TViewModel>), new PropertyMetadata(null));

        /// <summary>
        /// Gets the binding root view model.
        /// </summary>
        public TViewModel BindingRoot => ViewModel;

        #region IViewFor<TViewModel> Members

        /// <summary>
        /// 
        /// </summary>
        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }

        #endregion
    }
}