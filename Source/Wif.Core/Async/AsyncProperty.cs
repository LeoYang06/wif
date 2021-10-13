/**************************************************************************
*      File Name£ºAsyncProperty.cs
*    Description£ºAsyncProperty.cs class description...
*      Copyright£ºCopyright ? 2020 LeoYang-Chuese. All rights reserved.
*        Creator£ºLeo Yang
*    Create Time£º2020/12/15
*Project Address£ºhttps://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Threading.Tasks;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    ///     This class is an INotifyPropertyChanged for an async task.
    /// </summary>
    public class AsyncProperty : AsyncPropertyBase<Task>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncProperty" /> class.
        /// </summary>
        public AsyncProperty() : base(Task.CompletedTask)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncProperty" /> class.
        /// </summary>
        /// <param name="task">The task<see cref="Task" /></param>
        public AsyncProperty(Task task) : base(task)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates an <seealso cref="AsyncProperty{T}" /> from the given task. When the task competes it will
        ///     apply <paramref name="func" /> and that will be the final value of the property.
        /// </summary>
        /// <typeparam name="TIn">The type of the input task result.</typeparam>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="valueSource">The task where the initial value comes from.</param>
        /// <param name="func">The function to apply to the result of the task.</param>
        /// <param name="defaultValue">The value to use while the task is executing.</param>
        /// <returns>The <see cref="AsyncProperty{T}" /></returns>
        public static AsyncProperty<T> Create<TIn, T>(
                Task<TIn> valueSource,
                Func<TIn, T> func,
                T defaultValue = default)
        {
            var continuationTask = valueSource.ContinueWith(
                    t => func(GetTaskResultSafe(t)));
            return new AsyncProperty<T>(continuationTask, defaultValue);
        }

        #endregion
    }

    /// <summary>
    ///     This class is an async model for a single async property, the Value property will
    ///     be set to the result of the Task once it is completed.
    /// </summary>
    /// <typeparam name="T">The type of the property</typeparam>
    public class AsyncProperty<T> : AsyncPropertyBase<Task<T>>
    {
        #region Fields

        /// <summary>
        ///     Defines the _value
        /// </summary>
        private T _value;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the Value
        ///     The value of the property, which will be set once Task where the value comes from
        ///     is completed.
        /// </summary>
        public T Value
        {
            get => _value;
            private set => RaiseAndSetIfChanged(ref _value, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncProperty{T}" /> class.
        /// </summary>
        /// <param name="value">The value<see cref="T" /></param>
        public AsyncProperty(T value) : this(Task.FromResult(value), value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncProperty{T}" /> class.
        /// </summary>
        /// <param name="valueSource">The valueSource<see cref="Task{T}" /></param>
        /// <param name="defaultValue">The defaultValue<see cref="T" /></param>
        public AsyncProperty(Task<T> valueSource, T defaultValue = default) : base(valueSource)
        {
            Value = ActualTask.IsCompleted ? GetTaskResultSafe(ActualTask, defaultValue) : defaultValue;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The OnTaskComplete
        /// </summary>
        protected override void OnTaskComplete()
        {
            Value = GetTaskResultSafe(ActualTask, Value);
        }

        #endregion
    }
}