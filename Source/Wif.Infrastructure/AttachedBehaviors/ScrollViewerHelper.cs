using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Frontier.Wif.Infrastructure.AttachedBehaviors
{
    /// <summary>
    /// Defines the <see cref="ScrollViewerHelper" />
    /// </summary>
    public static class ScrollViewerHelper
    {
        #region Fields

        /// <summary>
        /// Identifies the IsHorizontalScrollWheelEnabled attached property.
        /// </summary>
        public static readonly DependencyProperty IsHorizontalScrollWheelEnabledProperty =
                DependencyProperty.RegisterAttached("IsHorizontalScrollWheelEnabled",
                        typeof(bool),
                        typeof(ScrollViewerHelper),
                        new PropertyMetadata(false, OnIsHorizontalScrollWheelEnabledPropertyChangedCallback));

        /// <summary>
        /// Identifies the VerticalScrollBarOnLeftSide attached property.
        /// This property can be used to set vertical scrollbar left side from the tabpanel (look at MetroAnimatedSingleRowTabControl)
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty =
                DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide",
                        typeof(bool),
                        typeof(ScrollViewerHelper),
                        new FrameworkPropertyMetadata(false,
                                FrameworkPropertyMetadataOptions.AffectsArrange |
                                FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region Methods

        /// <summary>
        /// Gets whether the ScrollViewer is scrolling horizontal by using the mouse wheel.
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <returns>The <see cref="bool"/></returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsHorizontalScrollWheelEnabled(UIElement element)
        {
            return (bool) element.GetValue(IsHorizontalScrollWheelEnabledProperty);
        }

        /// <summary>
        /// Gets whether the vertical ScrollBar is on the left side or not.
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <returns>The <see cref="bool"/></returns>
        [AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
        public static bool GetVerticalScrollBarOnLeftSide(UIElement element)
        {
            return (bool) element.GetValue(VerticalScrollBarOnLeftSideProperty);
        }

        /// <summary>
        /// Sets whether the ScrollViewer should be scroll horizontal by using the mouse wheel.
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <param name="value">The value<see cref="bool"/></param>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static void SetIsHorizontalScrollWheelEnabled(UIElement element, bool value)
        {
            element.SetValue(IsHorizontalScrollWheelEnabledProperty, value);
        }

        /// <summary>
        /// Sets whether the vertical ScrollBar should be on the left side or not.
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <param name="value">The value<see cref="bool"/></param>
        [AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
        public static void SetVerticalScrollBarOnLeftSide(UIElement element, bool value)
        {
            element.SetValue(VerticalScrollBarOnLeftSideProperty, value);
        }

        /// <summary>
        /// The OnIsHorizontalScrollWheelEnabledPropertyChangedCallback
        /// </summary>
        /// <param name="o">The o<see cref="DependencyObject"/></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void OnIsHorizontalScrollWheelEnabledPropertyChangedCallback(DependencyObject o,
                DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = o as ScrollViewer;
            if (scrollViewer != null && e.NewValue != e.OldValue && e.NewValue is bool)
            {
                scrollViewer.PreviewMouseWheel -= ScrollViewerOnPreviewMouseWheel;
                if ((bool) e.NewValue) scrollViewer.PreviewMouseWheel += ScrollViewerOnPreviewMouseWheel;
            }
        }

        /// <summary>
        /// The ScrollViewerOnPreviewMouseWheel
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="System.Windows.Input.MouseWheelEventArgs"/></param>
        private static void ScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null && scrollViewer.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled)
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
        }

        #endregion
    }
}