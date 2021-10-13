/**************************************************************************
*      File Name：EventToCommandBehavior.cs
*    Description：EventToCommandBehavior.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.Windows;

namespace Frontier.Wif.Infrastructure.EventToCommand
{
    /// <summary>
    /// Behavior that execute command when given event is raised.
    /// <remarks>https://docs.telerik.com/devtools/wpf/common-information/mvvm-helpers/common-event-to-command-behavior#getting-started</remarks>
    /// <code>
    /// <Wif.Demo:ListBox x:Name="ListBox" ItemsSource="{Binding ListBoxItems}"> 
    /// <wif:EventToCommandBehavior.EventBindings> 
    /// <Wif:EventBinding Command = "{Binding CustomCommand}" EventName="MouseEnter" /> 
    /// </Wif:EventToCommandBehavior.EventBindings> 
    /// </Wif.Demo:ListBox> 
    /// </code>
    /// </summary>
    public static class EventToCommandBehavior
    {
        #region Fields

        /// <summary>
        /// Defines the EventBindingsProperty
        /// </summary>
        private static readonly DependencyProperty EventBindingsProperty =
                DependencyProperty.RegisterAttached("EventBindingsInternal", typeof(EventBindingCollection),
                        typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventBindingsPropertyChanged));

        #endregion

        #region Methods

        /// <summary>
        /// Gets the collection of <see cref="EventBinding" />s associated with this element.
        /// </summary>
        /// <param name="obj">The object that <see cref="EventBindingCollection" /> is returned.</param>
        /// <returns>Returns the <see cref="EventBindingCollection" /> associated with this object.</returns>
        public static EventBindingCollection GetEventBindings(DependencyObject obj)
        {
            var collection = (EventBindingCollection)obj.GetValue(EventBindingsProperty);
            if (collection == null)
            {
                collection = new EventBindingCollection();
                obj.SetValue(EventBindingsProperty, collection);
            }

            return collection;
        }

        /// <summary>
        /// The OnEventBindingsPropertyChanged
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" /></param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /></param>
        private static void OnEventBindingsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This is possible in WPF because Freezable raise property change for same values.
            if (e.OldValue == e.NewValue) return;

            var collection = e.OldValue as EventBindingCollection;
            if (collection != null) collection.SetOwner(null);

            collection = e.NewValue as EventBindingCollection;
            if (collection != null) collection.SetOwner(d as UIElement);
        }

        #endregion
    }
}
