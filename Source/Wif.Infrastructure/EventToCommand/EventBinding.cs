/**************************************************************************
*      File Name：EventBinding.cs
*    Description：EventBinding.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Frontier.Wif.Infrastructure.EventToCommand
{
    /// <summary>
    /// Represents a binding between an event and a command. The command is potentially a <see cref="RoutedCommand" />.
    /// </summary>
    public class EventBinding : Freezable, ICommandSource
    {
        #region Fields

        /// <summary>
        /// Identifies the CommandParameter dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
                InputBinding.CommandParameterProperty.AddOwner(typeof(EventBinding));

        /// <summary>
        /// Identifies the Command dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
                InputBinding.CommandProperty.AddOwner(typeof(EventBinding));

        /// <summary>
        /// Identifies the CommandTarget dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty =
                InputBinding.CommandTargetProperty.AddOwner(typeof(EventBinding));

        /// <summary>
        /// Identifies the EventName dependency property.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty =
                DependencyProperty.Register("EventName", typeof(string), typeof(EventBinding),
                        new PropertyMetadata(string.Empty, OnEventNameChanged));

        /// <summary>
        /// Identifies the PassEventArgsToCommand property.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsToCommandProperty =
                DependencyProperty.Register("PassEventArgsToCommand", typeof(bool), typeof(EventBinding),
                        new PropertyMetadata(false));

        /// <summary>
        /// Identifies the RaiseOnHandledEvents dependency property.
        /// </summary>
        public static readonly DependencyProperty RaiseOnHandledEventsProperty =
                DependencyProperty.Register("RaiseOnHandledEvents", typeof(bool), typeof(EventBinding),
                        new PropertyMetadata(false, OnRaiseOnHandledEventsChanged));

        /// <summary>
        /// Defines the methodInfo
        /// </summary>
        private static readonly MethodInfo _methodInfo =
                typeof(EventBinding).GetMethod("OnEventTriggered", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Defines the eventHandler
        /// </summary>
        private Delegate _eventHandler;

        /// <summary>
        /// Defines the eventInfo
        /// </summary>
        private EventInfo _eventInfo;

        /// <summary>
        /// Defines the routedEvent
        /// </summary>
        private RoutedEvent _routedEvent;

        /// <summary>
        /// Defines the visual
        /// </summary>
        private UIElement _visual;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Command
        /// Gets or sets the <see cref="System.Windows.Input.ICommand" /> associated with this input binding.
        /// </summary>
        [Localizability(LocalizationCategory.NeverLocalize)]
        [TypeConverter(typeof(CommandConverter))]
        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the CommandParameter
        /// Gets or sets the command-specific data for a particular command.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the CommandTarget
        /// Gets or sets the target element of the command.
        /// </summary>
        public IInputElement CommandTarget
        {
            get => (IInputElement) GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        /// <summary>
        /// Gets or sets the EventName
        /// Gets or sets the name of the event that will open the context menu.
        /// </summary>
        public string EventName
        {
            get => (string) GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether PassEventArgsToCommand
        /// Gets or sets the value indicating if the event arguments will be passed to the command. If you specify
        ///     CommandParameter this value is ignored.
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get => (bool) GetValue(PassEventArgsToCommandProperty);
            set => SetValue(PassEventArgsToCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether RaiseOnHandledEvents
        /// Gets or sets whether <see cref="EventBinding" /> will raise the <see cref="Command" /> on handled routed events.
        ///     The default value is false. This is a dependency property.
        /// </summary>
        public bool RaiseOnHandledEvents
        {
            get => (bool) GetValue(RaiseOnHandledEventsProperty);
            set => SetValue(RaiseOnHandledEventsProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SetSource
        /// </summary>
        /// <param name="source">The <see cref="UIElement" /></param>
        internal void SetSource(UIElement source)
        {
            RemoveHandler(_visual, EventName);
            _visual = source;
            AttachHandler(source);
        }

        /// <summary>
        /// Creates an instance of an <see cref="EventBinding" />.
        /// </summary>
        /// <returns>A new instance of an <see cref="EventBinding" />.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new EventBinding();
        }

        /// <summary>
        /// The OnEventNameChanged
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" /></param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /></param>
        private static void OnEventNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventBinding = d as EventBinding;
            eventBinding.RemoveHandler(eventBinding._visual, e.OldValue as string);
            eventBinding.AttachHandler(eventBinding._visual);
        }

        /// <summary>
        /// The OnRaiseOnHandledEventsChanged
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject" /></param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /></param>
        private static void OnRaiseOnHandledEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventBinding = d as EventBinding;
            eventBinding.RemoveHandler(eventBinding._visual, eventBinding.EventName);
            eventBinding.AttachHandler(eventBinding._visual);
        }

        /// <summary>
        /// The AttachDynamicEventHandler
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /></param>
        private void AttachDynamicEventHandler(Type elementType)
        {
            _eventInfo = elementType.GetEvent(EventName);
            if (_eventInfo == null) throw new InvalidOperationException(EventName + " event cannot be found.");

            _eventHandler = Delegate.CreateDelegate(_eventInfo.EventHandlerType, this, _methodInfo);
            _eventInfo.AddEventHandler(_visual, _eventHandler);
        }

        /// <summary>
        /// The AttachHandler
        /// </summary>
        /// <param name="element">The <see cref="UIElement" /></param>
        private void AttachHandler(UIElement element)
        {
            if (element != null)
            {
                var eventName = EventName;
                var isNullOrEmpty = string.IsNullOrWhiteSpace(eventName);
                if (!isNullOrEmpty)
                {
                    var elementType = element.GetType();

                    _routedEvent = GetRoutedEvent(elementType);
                    if (_routedEvent != null)
                        element.AddHandler(_routedEvent, new RoutedEventHandler(RoutedEventFired),
                                RaiseOnHandledEvents);
                    else
                        AttachDynamicEventHandler(elementType);
                }
            }
        }

        /// <summary>
        /// The ExecuteCommand
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /></param>
        private void ExecuteCommand(EventArgs e)
        {
            var command = Command;
            if (command != null)
            {
                var commandParameter = CommandParameter;

                if (commandParameter == null && PassEventArgsToCommand) commandParameter = e;

                var commandTarget = CommandTarget ?? _visual;

                var routedCommand = command as RoutedCommand;
                if (routedCommand != null)
                {
                    if (routedCommand.CanExecute(commandParameter, commandTarget))
                        routedCommand.Execute(commandParameter, commandTarget);
                }
                else
                {
                    var eventCommand = command as RoutedEventCommand;
                    if (eventCommand != null)
                    {
                        if (eventCommand.CanExecute(commandParameter))
                            eventCommand.Execute(commandParameter, commandTarget);
                    }
                    else if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                }
            }
        }

        /// <summary>
        /// The GetRoutedEvent
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /></param>
        /// <returns>The <see cref="RoutedEvent" /></returns>
        private RoutedEvent GetRoutedEvent(Type elementType)
        {
            while (elementType != typeof(DependencyObject))
            {
                var routedEvents = EventManager.GetRoutedEventsForOwner(elementType);
                if (routedEvents != null)
                    foreach (var routEvent in routedEvents)
                        if (routEvent.Name == EventName)
                            return routEvent;

                elementType = elementType.BaseType;
            }

            return null;
        }

        /// <summary>
        /// The OnEventTriggered
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="EventArgs" /></param>
        private void OnEventTriggered(object sender, EventArgs e)
        {
            ExecuteCommand(e);
        }

        /// <summary>
        /// The RemoveHandler
        /// </summary>
        /// <param name="element">The <see cref="UIElement" /></param>
        /// <param name="eventName">The <see cref="string" /></param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventName")]
        private void RemoveHandler(UIElement element, string eventName)
        {
            if (element != null)
                if (_routedEvent != null)
                {
                    element.RemoveHandler(_routedEvent, new RoutedEventHandler(RoutedEventFired));
                }
                else if (_eventInfo != null && _eventHandler != null)
                {
                    _eventInfo.RemoveEventHandler(element, _eventHandler);
                    _eventHandler = null;
                    _eventInfo = null;
                }
        }

        /// <summary>
        /// The RoutedEventFired
        /// </summary>
        /// <param name="sender">The <see cref="object" /></param>
        /// <param name="e">The <see cref="RoutedEventArgs" /></param>
        private void RoutedEventFired(object sender, RoutedEventArgs e)
        {
            ExecuteCommand(e);
        }

        #endregion
    }
}