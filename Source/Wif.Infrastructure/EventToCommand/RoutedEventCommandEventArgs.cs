/**************************************************************************
*      File Name：RoutedEventCommandEventArgs.cs
*    Description：RoutedEventCommandEventArgs.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Windows;

namespace Frontier.Wif.Infrastructure.EventToCommand
{
    /// <summary>
    /// Defines the <see cref="RoutedEventCommandEventArgs" />
    /// </summary>
    internal sealed class RoutedEventCommandEventArgs : RoutedEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether CloseAll
        /// Gets a value indicating whether CloseAll
        /// </summary>
        public bool CloseAll { get; }

        /// <summary>
        /// Gets the CommandParameter
        /// Gets the CommandParameter
        /// </summary>
        public object CommandParameter { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventCommandEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The <see cref="RoutedEvent" /></param>
        /// <param name="parameter">The <see cref="object" /></param>
        /// <param name="closeAllEvent">The <see cref="bool" /></param>
        internal RoutedEventCommandEventArgs(RoutedEvent routedEvent, object parameter, bool closeAllEvent)
                : base(routedEvent)
        {
            CommandParameter = parameter;
            CloseAll = closeAllEvent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The InvokeEventHandler
        /// </summary>
        /// <param name="genericHandler">The <see cref="Delegate" /></param>
        /// <param name="target">The <see cref="object" /></param>
        protected override void InvokeEventHandler(Delegate genericHandler, object target)
        {
            var handler = (EventHandler<RoutedEventCommandEventArgs>) genericHandler;
            handler(target as DependencyObject, this);
        }

        #endregion
    }
}