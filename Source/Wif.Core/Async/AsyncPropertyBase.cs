/**************************************************************************
*      File Name£ºAsyncPropertyBase.cs
*    Description£ºAsyncPropertyBase.cs class description...
*      Copyright£ºCopyright ? 2020 LeoYang-Chuese. All rights reserved.
*        Creator£ºLeo Yang
*    Create Time£º2020/12/15
*Project Address£ºhttps://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Frontier.Wif.Core.ComponentModel;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    ///     Base class for modeling a task. Implementations should NotifyAllPropertyChanged when the task completes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AsyncPropertyBase<T> : PropertyChangedBase, IAsyncProperty<T> where T : Task
    {
        #region Properties

        /// <summary>
        ///     Gets the ActualTask
        ///     The actual task sent to the property.
        /// </summary>
        public T ActualTask { get; }

        /// <summary>
        ///     Gets the relevant exception, if it exists.
        /// </summary>
        public string ErrorMessage =>
                ActualTask?.Exception?.InnerException?.Message ??
                ActualTask?.Exception?.InnerExceptions?.Select(e => e?.Message)
                        .FirstOrDefault(m => !string.IsNullOrEmpty(m)) ??
                ActualTask?.Exception?.Message;

        /// <summary>
        ///     Gets a value indicating whether IsCanceled
        ///     True if the task ended execution due to being canceled.
        /// </summary>
        public bool IsCanceled => ActualTask?.IsCanceled ?? false;

        /// <summary>
        ///     Gets a value indicating whether IsCompleted
        ///     Returns true if the wrapped task is completed.
        /// </summary>
        public bool IsCompleted => ActualTask?.IsCompleted ?? false;

        /// <summary>
        ///     Gets a value indicating whether IsError
        ///     Returns true if the wrapped task is in error.
        /// </summary>
        public bool IsError => ActualTask?.IsFaulted ?? false;

        /// <summary>
        ///     Gets a value indicating whether IsPending
        ///     Returns whether the wrapped task is still pending.
        /// </summary>
        public bool IsPending => !ActualTask?.IsCompleted ?? false;

        /// <summary>
        ///     Gets a value indicating whether IsSuccess
        ///     True if the task completed without cancellation or exception.
        /// </summary>
        public bool IsSuccess => IsCompleted && !IsCanceled && !IsError;

        /// <summary>
        ///     Gets the SafeTask
        ///     A task that succeeds when the actual task completes. This task will never throw.
        /// </summary>
        public Task SafeTask { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncPropertyBase{T}" /> class.
        /// </summary>
        /// <param name="task">The task<see cref="T" /></param>
        protected AsyncPropertyBase(T task)
        {
            ActualTask = task;
            SafeTask = WaitTask();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Allows <code>await AsyncPropertyBase</code> rather than requiring <code>await AsyncPropertyBase.SafeTask</code>.
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter" /> from <see cref="IAsyncProperty{T}.SafeTask" /></returns>
        public TaskAwaiter GetAwaiter()
        {
            return SafeTask.GetAwaiter();
        }

        /// <summary>
        ///     Gets the result of the task, or a default value if the task is not successful.
        /// </summary>
        /// <typeparam name="TIn">They type of the task result.</typeparam>
        /// <param name="task">The task to get the result from.</param>
        /// <param name="defaultValue">The default value to return on a task error.</param>
        /// <returns>The value of the task, or the default value.</returns>
        protected static TIn GetTaskResultSafe<TIn>(Task<TIn> task, TIn defaultValue = default)
        {
            try
            {
                return task.Result;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     Called when the actual task completes.
        /// </summary>
        protected virtual void OnTaskComplete()
        {
        }

        /// <summary>
        ///     The RaiseTaskCompletedPropertiesChanged
        /// </summary>
        private void RaiseTaskCompletedPropertiesChanged()
        {
            SafeRaisePropertyChanged(nameof(IsPending));
            SafeRaisePropertyChanged(nameof(IsCompleted));
            if (IsSuccess) SafeRaisePropertyChanged(nameof(IsSuccess));

            if (IsCanceled) SafeRaisePropertyChanged(nameof(IsCanceled));

            if (IsError) SafeRaisePropertyChanged(nameof(IsError));

            if (ErrorMessage != null) SafeRaisePropertyChanged(nameof(ErrorMessage));
        }

        /// <summary>
        ///     The SafeRaisePropertyChanged
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string" /></param>
        private void SafeRaisePropertyChanged(string propertyName)
        {
            try
            {
                RaisePropertyChanged(propertyName);
            }
            catch
            {
                // Keep SafeTask safe. Ignore event handler exceptions.
            }
        }

        /// <summary>
        ///     The WaitTask
        /// </summary>
        /// <returns>The <see cref="Task" /></returns>
        private async Task WaitTask()
        {
            if (ActualTask != null)
            {
                try
                {
                    await ActualTask;
                }
                catch
                {
                    // Check exceptions using task.
                }

                OnTaskComplete();
                RaiseTaskCompletedPropertiesChanged();
            }
        }

        #endregion
    }
}