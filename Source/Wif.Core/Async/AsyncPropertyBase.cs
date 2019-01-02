using System.Linq;
using System.Threading.Tasks;
using Frontier.Wif.Core.ComponentModel;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    /// Base class for modeling a task. Implementations should NotifyAllPropertyChanged when the task completes.
    /// </summary>
    public abstract class AsyncPropertyBase : PropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Gets the relevant exception, if it exists.
        /// </summary>
        public string ErrorMessage =>
                Task.Exception?.InnerException?.Message ??
                Task.Exception?.InnerExceptions?.FirstOrDefault()?.Message ??
                Task.Exception?.Message;

        /// <summary>
        /// Gets a value indicating whether IsCanceled
        /// True if the task ended execution due to being canceled.
        /// </summary>
        public bool IsCanceled => Task?.IsCanceled ?? false;

        /// <summary>
        /// Gets a value indicating whether IsCompleted
        /// Returns true if the wrapped task is completed.
        /// </summary>
        public bool IsCompleted => Task?.IsCompleted ?? true;

        /// <summary>
        /// Gets a value indicating whether IsError
        /// Returns true if the wrapped task is in error.
        /// </summary>
        public bool IsError => Task?.IsFaulted ?? false;

        /// <summary>
        /// Gets a value indicating whether IsPending
        /// Returns whether the wrapped task is still pending.
        /// </summary>
        public bool IsPending => !Task?.IsCompleted ?? false;

        /// <summary>
        /// Gets a value indicating whether IsSuccess
        /// True if the task completed without cancelation or exception.
        /// </summary>
        public bool IsSuccess => IsCompleted && !IsCanceled && !IsError;

        /// <summary>
        /// Gets the Task
        /// The task to trigger a nodify on completion.
        /// </summary>
        protected abstract Task Task { get; }

        #endregion
    }
}