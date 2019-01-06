using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Frontier.Wif.Core.Async
{
    #region Interfaces

    /// <summary>
    ///     Defines the <see cref="IAsyncProperty{out T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncProperty<out T> where T : Task
    {
        #region Properties

        /// <summary>
        ///     Gets the ActualTask
        ///     The actual task sent to the property.
        /// </summary>
        T ActualTask { get; }

        /// <summary>
        ///     Gets the relevant exception, if it exists.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        ///     Gets the IsCanceled
        ///     True if the task ended execution due to being canceled.
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        ///     Gets the IsCompleted
        ///     Returns true if the wrapped task is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        ///     Gets the IsError
        ///     Returns true if the wrapped task is in error.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        ///     Gets the IsPending
        ///     Returns whether the wrapped task is still pending.
        /// </summary>
        bool IsPending { get; }

        /// <summary>
        ///     Gets the IsSuccess
        ///     True if the task completed without cancelation or exception.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        ///     Gets the SafeTask
        ///     A task that succeeds when the actual task completes. This task will never throw.
        /// </summary>
        Task SafeTask { get; }

        #endregion

        #region Events

        /// <summary>
        ///     Defines the PropertyChanged
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        ///     Allows <code>await IAsyncProperty</code> rather than requiring <code>await IAsyncProperty.SafeTask</code>.
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter" /> from <see cref="SafeTask" /></returns>
        TaskAwaiter GetAwaiter();

        #endregion
    }

    #endregion
}