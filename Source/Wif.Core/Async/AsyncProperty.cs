using System;
using System.Threading.Tasks;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    /// This class is an INotifyPropertyChanged for an async task.
    /// </summary>
    public class AsyncProperty : AsyncPropertyBase
    {
        #region Properties

        /// <summary>
        /// Gets the Task
        /// </summary>
        protected override Task Task { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncProperty"/> class.
        /// </summary>
        /// <param name="task">The task<see cref="Task"/></param>
        public AsyncProperty(Task task)
        {
            Task = task;
            WaitTask(task);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The WaitTask
        /// </summary>
        /// <param name="task">The task<see cref="Task"/></param>
        private async void WaitTask(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // Check exceptions using task.
            }

            RaiseAllPropertyChanged();
        }

        #endregion
    }

    /// <summary>
    /// This class is an async model for a single async property, the Value property will
    /// be set to the result of the Task once it is completed.
    /// </summary>
    /// <typeparam name="T">The type of the property</typeparam>
    public class AsyncProperty<T> : AsyncPropertyBase
    {
        #region Fields

        /// <summary>
        /// Defines the _completionSource
        /// </summary>
        private readonly Lazy<TaskCompletionSource<bool>> _completionSource = new Lazy<TaskCompletionSource<bool>>();

        /// <summary>
        /// Defines the _valueSource
        /// </summary>
        private readonly Task<T> _valueSource;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Value
        /// The value of the property, which will be set once Task where the value comes from
        /// is completed.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the ValueTask
        /// Returns a task that will be completed once the wrapped task is completed. This task is
        /// not directly connected to the wrapped task and will never throw and error.
        /// </summary>
        public Task ValueTask => _completionSource.Value.Task;

        /// <summary>
        /// Gets the Task
        /// </summary>
        protected override Task Task => _valueSource;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncProperty{T}"/> class.
        /// </summary>
        /// <param name="value">The value<see cref="T"/></param>
        public AsyncProperty(T value)
        {
            Value = value;
            _completionSource.Value.SetResult(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncProperty{T}"/> class.
        /// </summary>
        /// <param name="valueSource">The valueSource<see cref="Task{T}"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="T"/></param>
        public AsyncProperty(Task<T> valueSource, T defaultValue = default)
        {
            _valueSource = valueSource;
            Value = defaultValue;
            AwaitForValue();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The AwaitForValue
        /// </summary>
        private void AwaitForValue()
        {
            _valueSource.ContinueWith(t =>
            {
                // Value is initiated with defaultValue at constructor.
                Value = AsyncPropertyUtils.GetTaskResultSafe(t, Value);
                _completionSource.Value.SetResult(true);
                RaiseAllPropertyChanged();
            });
        }

        #endregion
    }
}