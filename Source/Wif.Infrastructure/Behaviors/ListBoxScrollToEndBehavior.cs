/**************************************************************************
*      File Name：ListBoxScrollToEndBehavior.cs
*    Description：ListBoxScrollToEndBehavior.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Frontier.Wif.Infrastructure.Behaviors
{
    /// <summary>
    /// ListBox自动滚动到最后一行行为。
    /// </summary>
    public class ListBoxScrollToEndBehavior : Behavior<ListBox>
    {
        #region Fields

        /// <summary>
        /// Defines the AutoScrollProperty
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
                DependencyProperty.Register("AutoScroll", typeof(bool), typeof(ListBoxScrollToEndBehavior),
                        new PropertyMetadata(false, PropertyChangedCallback));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AutoScroll
        /// </summary>
        public bool AutoScroll
        {
            get => (bool) GetValue(AutoScrollProperty);
            set => SetValue(AutoScrollProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The OnAttached
        /// </summary>
        protected override void OnAttached()
        {
            if (!AutoScroll) return;
            var listBox = AssociatedObject;
            ((INotifyCollectionChanged) listBox.Items).CollectionChanged += OnListBoxCollectionChanged;
        }

        /// <summary>
        /// The OnDetaching
        /// </summary>
        protected override void OnDetaching()
        {
            var listBox = AssociatedObject;
            ((INotifyCollectionChanged) listBox.Items).CollectionChanged -= OnListBoxCollectionChanged;
        }

        /// <summary>
        /// The PropertyChangedCallback
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Convert.ToBoolean(e.NewValue))
                ((ListBoxScrollToEndBehavior) d).OnAttached();
            else
                ((ListBoxScrollToEndBehavior) d).OnDetaching();
        }

        /// <summary>
        /// The OnListBoxCollectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/></param>
        private void OnListBoxCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var listBox = AssociatedObject;
            if (e.Action == NotifyCollectionChangedAction.Add) listBox.ScrollIntoView(e.NewItems[0]);
        }

        #endregion
    }
}