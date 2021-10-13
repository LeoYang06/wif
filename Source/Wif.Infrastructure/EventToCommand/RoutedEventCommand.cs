/**************************************************************************
*      File Name：RoutedEventCommand.cs
*    Description：RoutedEventCommand.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows;
using System.Windows.Input;

namespace Frontier.Wif.Infrastructure.EventToCommand
{
    internal class RoutedEventCommand : ICommand
    {
        private readonly bool _closeAllEvent;
        private readonly RoutedEvent _routedEvent;

        internal RoutedEventCommand(RoutedEvent targetRoutedEvent, bool closeAll)
        {
            _routedEvent = targetRoutedEvent;
            _closeAllEvent = closeAll;
        }


        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Execute(parameter, FilterInputElement(GetFocusedElement()));
        }


        public void Execute(object parameter, IInputElement target)
        {
            if (target != null && !IsValid(target)) throw new InvalidOperationException("Invalid target.");

            if (target == null) target = FilterInputElement(GetFocusedElement());

            if (target != null)
                target.RaiseEvent(new RoutedEventCommandEventArgs(_routedEvent, parameter, _closeAllEvent));
        }

        private static IInputElement FilterInputElement(IInputElement element)
        {
            return element != null && IsValid(element) ? element : null;
        }

        private static IInputElement GetFocusedElement()
        {
            return Keyboard.FocusedElement;
        }

        private static bool IsValid(IInputElement e)
        {
            return e is UIElement || e is ContentElement || e is UIElement3D;
        }
    }
}