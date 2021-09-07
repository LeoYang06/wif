/**************************************************************************
*      File Name：ListViewScrollToEndBehaviour.cs
*    Description：ListViewScrollToEndBehaviour.cs class description...
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
    /// Defines the <see cref="ListViewScrollToEndBehaviour" />
    /// </summary>
    public class ListViewScrollToEndBehaviour : Behavior<ListView>
    {
        #region Fields

        /// <summary>
        /// Defines the AutoScrollProperty
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
                DependencyProperty.Register("AutoScroll", typeof(bool), typeof(ListViewScrollToEndBehaviour),
                        new PropertyMetadata(false
                                , PropertyChangedCallback));

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
            var listView = AssociatedObject;
            ((INotifyCollectionChanged) listView.Items).CollectionChanged += OnListViewCollectionChanged;
        }

        /// <summary>
        /// The OnDetaching
        /// </summary>
        protected override void OnDetaching()
        {
            var listView = AssociatedObject;
            ((INotifyCollectionChanged) listView.Items).CollectionChanged -= OnListViewCollectionChanged;
        }

        /// <summary>
        /// The PropertyChangedCallback
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Convert.ToBoolean(e.NewValue))
                ((ListViewScrollToEndBehaviour) d).OnAttached();
            else
                ((ListViewScrollToEndBehaviour) d).OnDetaching();
        }

        /// <summary>
        /// The OnListViewCollectionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/></param>
        private void OnListViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var listView = AssociatedObject;

            listView.SelectedIndex = listView.Items.Count - 1;
            if (e.Action == NotifyCollectionChangedAction.Add) listView.ScrollIntoView(listView.SelectedItem);
            listView.ScrollIntoView(listView.SelectedItem);
        }

        #endregion
    }
}