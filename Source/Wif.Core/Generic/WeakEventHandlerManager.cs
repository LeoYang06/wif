/**************************************************************************
*      File Name：WeakEventHandlerManager.cs
*    Description：WeakEventHandlerManager.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Frontier.Wif.Core.Generic
{
    /// <summary>
    /// Defines the <see cref="WeakEventHandlerManager" />
    /// </summary>
    public static class WeakEventHandlerManager
    {
        /// <summary>
        /// The AddWeakReferenceHandler
        /// </summary>
        /// <param name="handlers">The handlers<see cref="List{WeakReference}" /></param>
        /// <param name="handler">The handler<see cref="EventHandler" /></param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers == null)
                handlers = new List<WeakReference>();
            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// The RemoveWeakReferenceHandler
        /// </summary>
        /// <param name="handlers">The handlers<see cref="List{WeakReference}" /></param>
        /// <param name="handler">The handler<see cref="EventHandler" /></param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
                for (var i = handlers.Count - 1; i >= 0; i--)
                {
                    var reference = handlers[i];
                    if (!(reference.Target is EventHandler target) || target == handler)
                        handlers.RemoveAt(i);
                }
        }

        /// <summary>
        /// The CallWeakReferenceHandlers
        /// </summary>
        /// <param name="sender">The sender<see cref="object" /></param>
        /// <param name="handlers">The handlers<see cref="List{WeakReference}" /></param>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                var callees = new EventHandler[handlers.Count];
                var count = 0;
                count = CleanupOldHandlers(handlers, callees, count);
                for (var i = 0; i < count; i++)
                    CallHandler(sender, callees[i]);
            }
        }

        /// <summary>
        /// The CallHandler
        /// </summary>
        /// <param name="sender">The sender<see cref="object" /></param>
        /// <param name="eventHandler">The eventHandler<see cref="EventHandler" /></param>
        private static void CallHandler(object sender, EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                var dispatcher = Dispatcher.CurrentDispatcher;
                if (!dispatcher.CheckAccess())
                    dispatcher.BeginInvoke(new Action<object, EventHandler>(CallHandler), sender, eventHandler);
                else
                    eventHandler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The CleanupOldHandlers
        /// </summary>
        /// <param name="handlers">The handlers<see cref="List{WeakReference}" /></param>
        /// <param name="callees">The callees<see cref="EventHandler[]" /></param>
        /// <param name="count">The count<see cref="int" /></param>
        /// <returns>The <see cref="int" /></returns>
        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count)
        {
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var reference = handlers[i];
                var handler = reference.Target as EventHandler;
                if (handler == null || !reference.IsAlive)
                {
                    handlers.RemoveAt(i);
                }
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }

            return count;
        }
    }
}