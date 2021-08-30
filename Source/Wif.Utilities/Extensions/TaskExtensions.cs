using System;
using System.Threading.Tasks;

namespace Frontier.Wif.Utilities.Extensions
{
    /// <summary>
    /// Defines the <see cref="TaskExtensions" />
    /// </summary>
    public static class TaskExtensions
    {
        #region Methods

        /// <summary>
        /// 延时异步执行方法。
        /// </summary>
        /// <param name="action">待执行方法</param>
        /// <param name="delayTime">延时时间（毫秒）</param>
        public static void DelayRun(this Action action, int delayTime)
        {
            Task.Delay(delayTime).ContinueWith(unusedTask => action());
        }

        /// <summary>
        /// 支持异步执行工作委托和同步执行完成委托的扩展。
        /// </summary>
        /// <param name="workAction">The action<see cref="Action"/></param>
        /// <param name="completedAction">The completedAction<see cref="Action"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static Task RunAsync(this Action workAction, Action completedAction,
                bool runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;

            return Task.Run(workAction).ContinueWith(unusedTask => completedAction(), taskScheduler);
        }

        /// <summary>
        /// 支持异步执行工作委托和同步执行完成委托的扩展。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="workAction">The action<see cref="Action"/></param>
        /// <param name="completedFunction">The completedFunction<see cref="Func{TResult}"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        public static Task<TResult> RunAsync<TResult>(this Action workAction, Func<TResult> completedFunction,
                bool runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;

            return Task.Run(workAction).ContinueWith(unusedTask => completedFunction(), taskScheduler);
        }

        /// <summary>
        /// 支持异步执行工作委托和同步执行完成委托的扩展。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="workFunction">The function<see cref="Func{TResult}"/></param>
        /// <param name="completedAction">The completedAction<see cref="Action"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        public static Task<TResult> RunAsync<TResult>(this Func<TResult> workFunction, Action<TResult> completedAction,
                bool runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;

            var task = Task.Run(workFunction);
            task.ContinueWith(unusedTask => completedAction(task.Result), taskScheduler);
            return task;
        }

        /// <summary>
        /// 支持异步执行工作委托和同步执行完成委托的扩展。
        /// </summary>
        /// <typeparam name="TWorkResult"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="workFunction">The workFunction<see cref="Func{TWorkResult}"/></param>
        /// <param name="completedAction">The completedAction<see cref="Func{TWorkResult, TResult}"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        public static Task<TResult> RunAsync<TWorkResult, TResult>(this Func<TWorkResult> workFunction,
                Func<TWorkResult, TResult> completedAction,
                bool runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;

            return Task.Run(workFunction).ContinueWith(workTask => completedAction(workTask.Result), taskScheduler);
        }

        /// <summary>
        /// 支持异步执行工作委托和同步执行完成委托的扩展。
        /// </summary>
        /// <param name="workTask">The workTask<see cref="Task"/></param>
        /// <param name="completedAction">The completedAction<see cref="Action"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static Task RunAsync(this Task workTask, Action completedAction,
                bool runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;
            if (workTask.Status == TaskStatus.Created)
                workTask.Start();

            return workTask.ContinueWith(unusedTask => completedAction(), taskScheduler);
        }

        /// <summary>
        /// 支持工作任务和同步执行完成委托的扩展。
        /// </summary>
        /// <param name="workTask">The action<see cref="Task"/></param>
        /// <param name="completedAction">The completedAction<see cref="Action"/></param>
        /// <param name="runCompletedActionInUIThread">The runCompletedActionInUIThread<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static Task ContinueWithRun(this Task workTask, Action completedAction,
            bool                                     runCompletedActionInUIThread = true)
        {
            var taskScheduler = runCompletedActionInUIThread
                ? TaskScheduler.FromCurrentSynchronizationContext()
                : TaskScheduler.Current;

            return workTask.ContinueWith(unusedTask => completedAction(), taskScheduler);
        }

        #endregion
    }
}