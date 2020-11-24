using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Windows.Media;

namespace Frontier.Wif.Infrastructure.Behaviors
{
    /// <summary>
    /// Defines the <see cref="DragInCanvasBehavior" />
    /// </summary>
    public class DragInCanvasBehavior : Behavior<UIElement>
    {
        #region Fields

        /// <summary>
        /// Keep track of the Canvas where this element is placed.
        /// </summary>
        private Canvas _canvas;

        /// <summary>
        /// Keep track of when the element is being dragged.
        /// </summary>
        private bool _isDragging;

        /// <summary>
        /// When the element is clicked, record the exact position where the click is made.
        /// </summary>
        private Point _mouseOffset;

        #endregion

        #region Methods

        /// <summary>
        /// The OnAttached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            // Hook up event handlers.
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        /// <summary>
        /// The OnDetaching
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            // Detach event handlers.
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        /// <summary>
        /// The AssociatedObject_MouseLeftButtonDown
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /></param>
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the Canvas.
            if (_canvas == null)
                _canvas = (Canvas) VisualTreeHelper.GetParent(AssociatedObject);
            // Dragging mode begins.
            _isDragging = true;
            // Get the position of the click relative to the element
            // (so the top-left corner of the element is (0,0).
            _mouseOffset = e.GetPosition(AssociatedObject);
            // Capture the mouse. This way you'll keep receiving
            // the MouseMove event even if the user jerks the mouse
            // off the element.
            AssociatedObject.CaptureMouse();
        }

        /// <summary>
        /// The AssociatedObject_MouseLeftButtonUp
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /></param>
        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                _isDragging = false;
            }
        }

        /// <summary>
        /// The AssociatedObject_MouseMove
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="MouseEventArgs" /></param>
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Get the position of the element relative to the Canvas.
                var point = e.GetPosition(_canvas);
                // Move the element.
                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - _mouseOffset.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - _mouseOffset.X);
            }
        }

        #endregion
    }
}