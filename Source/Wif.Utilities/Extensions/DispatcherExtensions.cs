using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// 在UI线程上执行调用的扩展。
    /// </summary>
    public static class DispatcherExtensions
    {
        #region Methods

        /// <summary>
        /// 延时执行方法。
        /// </summary>
        /// <param name="dispatcher">线程调度器</param>
        /// <param name="action">待执行方法</param>
        /// <param name="delayTime">延时时间（毫秒）</param>
        public static void DelayInvoke(this Dispatcher dispatcher, Action action, int delayTime)
        {
            dispatcher.DelayInvoke(TimeSpan.FromMilliseconds(delayTime), action);
        }

        /// <summary>
        /// UI线程非阻塞延时
        /// </summary>
        /// <param name="dispatcher">线程调度器</param>
        /// <param name="delayTime">延时时间（毫秒）</param>
        /// <returns></returns>
        public static bool DelayInvoke(this Dispatcher dispatcher, int delayTime)
        {
            var now = DateTime.Now;
            double ms;
            do
            {
                var timeSpan = DateTime.Now - now;
                ms = timeSpan.TotalMilliseconds;
                dispatcher.ProcessMessages(DispatcherPriority.Background);
            } while (ms < delayTime);

            return true;
        }

        /// <summary>
        /// 延时执行方法。
        /// </summary>
        /// <param name="dispatcher">线程调度器</param>
        /// <param name="delayTime">延时时间（时间间隔）</param>
        /// <param name="action">待执行方法</param>
        public static void DelayInvoke(this Dispatcher dispatcher, TimeSpan delayTime,
                Action action)
        {
            Task.Delay(delayTime).ContinueWith(t => { dispatcher.Invoke(action); });
        }

        /// <summary>
        /// Waits until all pending messages up to the <see cref="DispatcherPriority.Background" /> priority are processed.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to wait on.</param>
        public static void ProcessMessages(this Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            ProcessMessages(dispatcher, DispatcherPriority.Background);
        }

        /// <summary>
        /// Waits until all pending messages up to the specified priority are processed.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to wait on.</param>
        /// <param name="priority">The priority up to which all messages should be processed.</param>
        public static void ProcessMessages(this Dispatcher dispatcher, DispatcherPriority priority)
        {
            Contract.Requires(dispatcher != null);

            var frame = new DispatcherFrame();
            dispatcher.BeginInvoke(priority, new Action(() => frame.Continue = false));
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Waits until all pending messages up to the <see cref="DispatcherPriority.Background" /> priority are processed.
        /// </summary>
        /// <param name="visual">The dispatcher object to wait on.</param>
        public static void ProcessMessages(this Visual visual)
        {
            Contract.Requires(visual != null);

            ProcessMessages(visual, DispatcherPriority.Background);
        }

        /// <summary>
        /// Waits until all pending messages up to the specified priority are processed.
        /// </summary>
        /// <param name="visual">The dispatcher object to wait on.</param>
        /// <param name="priority">The priority up to which all messages should be processed.</param>
        public static void ProcessMessages(this Visual visual, DispatcherPriority priority)
        {
            Contract.Requires(visual != null);

            var dispatcher = visual.Dispatcher;
            Contract.Assume(dispatcher != null);

            ProcessMessages(dispatcher, priority);
        }

        #endregion
    }
}