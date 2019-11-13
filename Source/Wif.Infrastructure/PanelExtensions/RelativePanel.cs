using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Frontier.Wif.Infrastructure.PanelExtensions
{
    /// <summary>
    /// Defines an area within which you can position and align child objects in relation
    /// to each other or the parent panel.
    /// </summary>
    public partial class RelativePanel : Panel
    {
        #region Fields

        // Dependency property for storing intermediate arrange state on the children
        /// <summary>
        /// Defines the ArrangeStateProperty
        /// </summary>
        private static readonly DependencyProperty ArrangeStateProperty =
            DependencyProperty.Register("ArrangeState", typeof(double[]), typeof(RelativePanel),
                new PropertyMetadata(null));

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a
        /// size for a System.Windows.FrameworkElement derived class.
        /// </summary>
        /// <param name="finalSize">The finalSize<see cref="Size" /></param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var item in CalculateLocations(finalSize))
                item.Item1.Arrange(item.Item2);
            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for
        /// child elements and determines a size for the System.Windows.FrameworkElement-derived
        /// </summary>
        /// <param name="availableSize">The availableSize<see cref="Size" /></param>
        /// <returns>The <see cref="Size" /></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in Children.OfType<FrameworkElement>())
                child.Measure(availableSize);
            foreach (var item in CalculateLocations(availableSize))
                if (item.Item2.Size.Width < item.Item1.DesiredSize.Width ||
                    item.Item2.Size.Height < item.Item1.DesiredSize.Height)
                    item.Item1.Measure(item.Item2.Size);
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// The CalculateLocations
        /// </summary>
        /// <param name="finalSize">The finalSize<see cref="Size" /></param>
        /// <returns>The <see cref="IEnumerable{Tuple{UIElement, Rect}}" /></returns>
        private IEnumerable<Tuple<UIElement, Rect>> CalculateLocations(Size finalSize)
        {
            //List of margins for each element between the element and panel (left, top, right, bottom)
            var arranges = new List<double[]>(Children.Count);
            //First pass aligns all sides that aren't constrained by other elements
            var arrangedCount = 0;
            foreach (var child in Children.OfType<UIElement>())
            {
                //NaN means the arrange value is not constrained yet for that side
                double[] rect = {double.NaN, double.NaN, double.NaN, double.NaN};
                arranges.Add(rect);
                child.SetValue(ArrangeStateProperty, rect);

                //Align with panels always wins, so do these first, or if no constraints are set at all set margin to 0

                //Left side
                if (GetAlignLeftWithPanel(child))
                {
                    rect[0] = 0;
                }
                else if (
                    child.GetValue(AlignLeftWithProperty) == null &&
                    child.GetValue(RightOfProperty) == null &&
                    child.GetValue(AlignHorizontalCenterWithProperty) == null &&
                    !GetAlignHorizontalCenterWithPanel(child))
                {
                    if (GetAlignRightWithPanel(child))
                        rect[0] = finalSize.Width - child.DesiredSize.Width;
                    else if (child.GetValue(AlignRightWithProperty) == null &&
                        child.GetValue(AlignHorizontalCenterWithProperty) == null &&
                        child.GetValue(LeftOfProperty) == null)
                        rect[0] = 0; //default fallback to 0
                }

                //Top side
                if (GetAlignTopWithPanel(child))
                {
                    rect[1] = 0;
                }
                else if (
                    child.GetValue(AlignTopWithProperty) == null &&
                    child.GetValue(BelowProperty) == null &&
                    child.GetValue(AlignVerticalCenterWithProperty) == null &&
                    !GetAlignVerticalCenterWithPanel(child))
                {
                    if (GetAlignBottomWithPanel(child))
                        rect[1] = finalSize.Height - child.DesiredSize.Height;
                    else if (child.GetValue(AlignBottomWithProperty) == null &&
                        child.GetValue(AlignVerticalCenterWithProperty) == null &&
                        child.GetValue(AboveProperty) == null)
                        rect[1] = 0; //default fallback to 0
                }

                //Right side
                if (GetAlignRightWithPanel(child))
                    rect[2] = 0;
                else if (!double.IsNaN(rect[0]) &&
                    child.GetValue(AlignRightWithProperty) == null &&
                    child.GetValue(LeftOfProperty) == null &&
                    child.GetValue(AlignHorizontalCenterWithProperty) == null &&
                    !GetAlignHorizontalCenterWithPanel(child))
                    rect[2] = finalSize.Width - rect[0] - child.DesiredSize.Width;

                //Bottom side
                if (GetAlignBottomWithPanel(child))
                    rect[3] = 0;
                else if (!double.IsNaN(rect[1]) && child.GetValue(AlignBottomWithProperty) == null &&
                    child.GetValue(AboveProperty) == null &&
                    child.GetValue(AlignVerticalCenterWithProperty) == null &&
                    !GetAlignVerticalCenterWithPanel(child))
                    rect[3] = finalSize.Height - rect[1] - child.DesiredSize.Height;

                if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                    !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                    arrangedCount++;
            }

            var i = 0;
            //Run iterative layout passes
            while (arrangedCount < Children.Count)
            {
                var valueChanged = false;
                i = 0;
                foreach (var child in Children.OfType<UIElement>())
                {
                    var rect = arranges[i++];

                    if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                        !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                        continue; //Control is fully arranged

                    //Calculate left side
                    if (double.IsNaN(rect[0]))
                    {
                        var alignLeftWith = GetDependencyElement(AlignLeftWithProperty, child);
                        if (alignLeftWith != null)
                        {
                            var r = (double[]) alignLeftWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[0]))
                            {
                                rect[0]      = r[0];
                                valueChanged = true;
                            }
                        }
                        else
                        {
                            var rightOf = GetDependencyElement(RightOfProperty, child);
                            if (rightOf != null)
                            {
                                var r = (double[]) rightOf.GetValue(ArrangeStateProperty);
                                if (!double.IsNaN(r[2]))
                                {
                                    rect[0]      = finalSize.Width - r[2];
                                    valueChanged = true;
                                }
                            }
                            else if (!double.IsNaN(rect[2]))
                            {
                                rect[0]      = finalSize.Width - rect[2] - child.DesiredSize.Width;
                                valueChanged = true;
                            }
                        }
                    }

                    //Calculate top side
                    if (double.IsNaN(rect[1]))
                    {
                        var alignTopWith = GetDependencyElement(AlignTopWithProperty, child);
                        if (alignTopWith != null)
                        {
                            var r = (double[]) alignTopWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[1]))
                            {
                                rect[1]      = r[1];
                                valueChanged = true;
                            }
                        }
                        else
                        {
                            var below = GetDependencyElement(BelowProperty, child);
                            if (below != null)
                            {
                                var r = (double[]) below.GetValue(ArrangeStateProperty);
                                if (!double.IsNaN(r[3]))
                                {
                                    rect[1]      = finalSize.Height - r[3];
                                    valueChanged = true;
                                }
                            }
                            else if (!double.IsNaN(rect[3]))
                            {
                                rect[1]      = finalSize.Height - rect[3] - child.DesiredSize.Height;
                                valueChanged = true;
                            }
                        }
                    }

                    //Calculate right side
                    if (double.IsNaN(rect[2]))
                    {
                        var alignRightWith = GetDependencyElement(AlignRightWithProperty, child);
                        if (alignRightWith != null)
                        {
                            var r = (double[]) alignRightWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[2]))
                            {
                                rect[2] = r[2];
                                if (double.IsNaN(rect[0]))
                                    if (child.GetValue(AlignLeftWithProperty) == null)
                                    {
                                        rect[0]      = rect[2] + child.DesiredSize.Width;
                                        valueChanged = true;
                                    }
                            }
                        }
                        else
                        {
                            var leftOf = GetDependencyElement(LeftOfProperty, child);
                            if (leftOf != null)
                            {
                                var r = (double[]) leftOf.GetValue(ArrangeStateProperty);
                                if (!double.IsNaN(r[0]))
                                {
                                    rect[2]      = finalSize.Width - r[0];
                                    valueChanged = true;
                                }
                            }
                            else if (!double.IsNaN(rect[0]))
                            {
                                rect[2]      = finalSize.Width - rect[0] - child.DesiredSize.Width;
                                valueChanged = true;
                            }
                        }
                    }

                    //Calculate bottom side
                    if (double.IsNaN(rect[3]))
                    {
                        var alignBottomWith = GetDependencyElement(AlignBottomWithProperty, child);
                        if (alignBottomWith != null)
                        {
                            var r = (double[]) alignBottomWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[3]))
                            {
                                rect[3]      = r[3];
                                valueChanged = true;
                                if (double.IsNaN(rect[1]))
                                    if (child.GetValue(AlignTopWithProperty) == null)
                                        rect[1] = finalSize.Height - rect[3] - child.DesiredSize.Height;
                            }
                        }
                        else
                        {
                            var above = GetDependencyElement(AboveProperty, child);
                            if (above != null)
                            {
                                var r = (double[]) above.GetValue(ArrangeStateProperty);
                                if (!double.IsNaN(r[1]))
                                {
                                    rect[3]      = finalSize.Height - r[1];
                                    valueChanged = true;
                                }
                            }
                            else if (!double.IsNaN(rect[1]))
                            {
                                rect[3]      = finalSize.Height - rect[1] - child.DesiredSize.Height;
                                valueChanged = true;
                            }
                        }
                    }

                    //Calculate horizontal alignment
                    if (double.IsNaN(rect[0]) && double.IsNaN(rect[2]))
                    {
                        var alignHorizontalCenterWith = GetDependencyElement(AlignHorizontalCenterWithProperty, child);
                        if (alignHorizontalCenterWith != null)
                        {
                            var r = (double[]) alignHorizontalCenterWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[0]) && !double.IsNaN(r[2]))
                            {
                                rect[0]      = r[0] + (finalSize.Width - r[2] - r[0]) * .5 - child.DesiredSize.Width * .5;
                                rect[2]      = finalSize.Width - rect[0] - child.DesiredSize.Width;
                                valueChanged = true;
                            }
                        }
                        else
                        {
                            if (GetAlignHorizontalCenterWithPanel(child))
                            {
                                var roomToSpare = finalSize.Width - child.DesiredSize.Width;
                                rect[0]      = roomToSpare * .5;
                                rect[2]      = roomToSpare * .5;
                                valueChanged = true;
                            }
                        }
                    }

                    //Calculate vertical alignment
                    if (double.IsNaN(rect[1]) && double.IsNaN(rect[3]))
                    {
                        var alignVerticalCenterWith = GetDependencyElement(AlignVerticalCenterWithProperty, child);
                        if (alignVerticalCenterWith != null)
                        {
                            var r = (double[]) alignVerticalCenterWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[1]) && !double.IsNaN(r[3]))
                            {
                                rect[1]      = r[1] + (finalSize.Height - r[3] - r[1]) * .5 - child.DesiredSize.Height * .5;
                                rect[3]      = finalSize.Height - rect[1] - child.DesiredSize.Height;
                                valueChanged = true;
                            }
                        }
                        else
                        {
                            if (GetAlignVerticalCenterWithPanel(child))
                            {
                                var roomToSpare = finalSize.Height - child.DesiredSize.Height;
                                rect[1]      = roomToSpare * .5;
                                rect[3]      = roomToSpare * .5;
                                valueChanged = true;
                            }
                        }
                    }


                    //if panel is now fully arranged, increase the counter
                    if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                        !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                        arrangedCount++;
                }

                if (!valueChanged)
                    //If a layout pass didn't increase number of arranged elements,
                    //there must be a circular dependency
                    throw new ArgumentException(
                        "RelativePanel error: Circular dependency detected. Layout could not complete");
            }

            i = 0;
            //Arrange iterations complete - Apply the results to the child elements
            foreach (var child in Children.OfType<UIElement>())
            {
                var rect = arranges[i++];
                //Measure child again with the new calculated available size
                //this helps for instance textblocks to reflow the text wrapping
                //We should probably have done this during the measure step but it would cause a more expensive
                //measure+arrange layout cycle
                //if(child is TextBlock)
                //    child.Measure(new Size(Math.Max(0, finalSize.Width - rect[2] - rect[0]), Math.Max(0, finalSize.Height - rect[3] - rect[1])));

                //if(child is TextBlock tb)
                //{
                //    tb.ArrangeOverride(new Rect(rect[0], rect[1], Math.Max(0, finalSize.Width - rect[2] - rect[0]), Math.Max(0, finalSize.Height - rect[3] - rect[1])));
                //}
                //else 
                yield return new Tuple<UIElement, Rect>(child,
                    new Rect(rect[0], rect[1], Math.Max(0, finalSize.Width - rect[2] - rect[0]),
                        Math.Max(0, finalSize.Height - rect[3] - rect[1])));
            }
        }

        //Gets the element that's referred to in the alignment attached properties
        /// <summary>
        /// The GetDependencyElement
        /// </summary>
        /// <param name="property">The property<see cref="DependencyProperty" /></param>
        /// <param name="child">The child<see cref="DependencyObject" /></param>
        /// <returns>The <see cref="UIElement" /></returns>
        private UIElement GetDependencyElement(DependencyProperty property, DependencyObject child)
        {
            var dependency = child.GetValue(property);
            if (dependency == null)
                return null;
            if (dependency is UIElement)
            {
                if (Children.Contains((UIElement) dependency))
                    return (UIElement) dependency;
                throw new ArgumentException(
                    string.Format("RelativePanel error: Element does not exist in the current context",
                        property.Name));
            }

            throw new ArgumentException("RelativePanel error: Value must be of type UIElement");
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="RelativePanel" />
    /// </summary>
    public partial class RelativePanel
    {
        #region Fields

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AboveProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AboveProperty =
            DependencyProperty.RegisterAttached("Above", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignBottomWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignBottomWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignBottomWithPanel", typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignBottomWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignBottomWithProperty =
            DependencyProperty.RegisterAttached("AlignBottomWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignHorizontalCenterWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignHorizontalCenterWithPanel", typeof(bool),
                typeof(RelativePanel), new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignHorizontalCenterWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignHorizontalCenterWithProperty =
            DependencyProperty.RegisterAttached("AlignHorizontalCenterWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignLeftWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignLeftWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignLeftWithPanel", typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignLeftWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignLeftWithProperty =
            DependencyProperty.RegisterAttached("AlignLeftWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignRightWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignRightWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignRightWithPanel", typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignRightWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignRightWithProperty =
            DependencyProperty.RegisterAttached("AlignRightWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignTopWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignTopWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignTopWithPanel", typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignTopWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignTopWithProperty =
            DependencyProperty.RegisterAttached("AlignTopWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignVerticalCenterWithPanelProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.AlignVerticalCenterWithProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty AlignVerticalCenterWithProperty =
            DependencyProperty.RegisterAttached("AlignVerticalCenterWith", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.BelowProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty BelowProperty =
            DependencyProperty.RegisterAttached("Below", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.LeftOfProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty LeftOfProperty =
            DependencyProperty.RegisterAttached("LeftOf", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="RelativePanel.RightOfProperty" /> XAML attached property.
        /// </summary>
        public static readonly DependencyProperty RightOfProperty =
            DependencyProperty.RegisterAttached("RightOf", typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of the RelativePanel.Above XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAbove(DependencyObject obj)
        {
            return obj.GetValue(AboveProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignBottomWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignBottomWith(DependencyObject obj)
        {
            return obj.GetValue(AlignBottomWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignBottomWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignBottomWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignBottomWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignHorizontalCenterWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignHorizontalCenterWith(DependencyObject obj)
        {
            return obj.GetValue(AlignHorizontalCenterWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignHorizontalCenterWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignHorizontalCenterWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignLeftWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignLeftWith(DependencyObject obj)
        {
            return obj.GetValue(AlignLeftWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignLeftWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignLeftWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignLeftWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignRightWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignRightWith(DependencyObject obj)
        {
            return obj.GetValue(AlignRightWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignRightWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignRightWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignRightWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignTopWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The value to set. (The element to align this element's top edge with.)</returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignTopWith(DependencyObject obj)
        {
            return obj.GetValue(AlignTopWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignTopWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignTopWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignTopWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignVerticalCenterWith XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The value to set. (The element to align this element's vertical center with.)</returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetAlignVerticalCenterWith(DependencyObject obj)
        {
            return obj.GetValue(AlignVerticalCenterWithProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.AlignVerticalCenterWithPanel XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="bool" /></returns>
        public static bool GetAlignVerticalCenterWithPanel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AlignVerticalCenterWithPanelProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.Below XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetBelow(DependencyObject obj)
        {
            return obj.GetValue(BelowProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.LeftOf XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetLeftOf(DependencyObject obj)
        {
            return obj.GetValue(LeftOfProperty);
        }

        /// <summary>
        /// Gets the value of the RelativePanel.RightOf XAML attached property for the target element.
        /// </summary>
        /// <param name="obj">The object from which the property value is read.</param>
        /// <returns>The <see cref="object" /></returns>
        [TypeConverter(typeof(NameReferenceConverter))]
        public static object GetRightOf(DependencyObject obj)
        {
            return obj.GetValue(RightOfProperty);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to position this element above.)</param>
        public static void SetAbove(DependencyObject obj, object value)
        {
            obj.SetValue(AboveProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's bottom edge with.)</param>
        public static void SetAlignBottomWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignBottomWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignBottomWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignBottomWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's horizontal center with.)</param>
        public static void SetAlignHorizontalCenterWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignHorizontalCenterWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignHorizontalCenterWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignHorizontalCenterWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's left edge with.)</param>
        public static void SetAlignLeftWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignLeftWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignLeftWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignLeftWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.AlignRightWith XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's right edge with.)</param>
        public static void SetAlignRightWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignRightWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignRightWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignRightWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.AlignTopWith XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's top edge with.)</param>
        public static void SetAlignTopWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignTopWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.AlignTopWithPanel XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignTopWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignTopWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.AlignVerticalCenterWith XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to align this element's horizontal center with.)</param>
        public static void SetAlignVerticalCenterWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignVerticalCenterWithProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.AlignVerticalCenterWithPanel XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value<see cref="bool" /></param>
        public static void SetAlignVerticalCenterWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignVerticalCenterWithPanelProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.Above XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to position this element below.)</param>
        public static void SetBelow(DependencyObject obj, object value)
        {
            obj.SetValue(BelowProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.LeftOf XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to position this element to the left of.)</param>
        public static void SetLeftOf(DependencyObject obj, object value)
        {
            obj.SetValue(LeftOfProperty, value);
        }

        /// <summary>
        /// Sets the value of the RelativePanel.RightOf XAML attached property for a target element.
        /// </summary>
        /// <param name="obj">The object to which the property value is written.</param>
        /// <param name="value">The value to set. (The element to position this element to the right of.)</param>
        public static void SetRightOf(DependencyObject obj, object value)
        {
            obj.SetValue(RightOfProperty, value);
        }

        /// <summary>
        /// The OnAlignPropertiesChanged
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject" /></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs" /></param>
        private static void OnAlignPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var elm = d as FrameworkElement;
            if (elm.Parent is FrameworkElement)
                ((FrameworkElement) elm.Parent).InvalidateArrange();
        }

        #endregion
    }
}