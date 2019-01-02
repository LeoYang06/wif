using System;
using System.Threading.Tasks;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    /// Defines the <see cref="AsyncPropertyUtils" />
    /// </summary>
    public static class AsyncPropertyUtils
    {
        #region Methods

        /// <summary>
        /// Creates an <seealso cref="AsyncProperty{T}"/> from the given task. When the task competes it will
        /// apply <paramref name="func"/> and that will be the final value of the property.
        /// </summary>
        /// <typeparam name="TIn">The type that the task will produce.</typeparam>
        /// <typeparam name="T">The type that the property will produce.</typeparam>
        /// <param name="valueSource">The task where the value comes from.</param>
        /// <param name="func">The function to apply to the result of the task.</param>
        /// <param name="defaultValue">The value to use while the task is executing.</param>
        /// <returns>The <see cref="AsyncProperty{T}"/></returns>
        public static AsyncProperty<T> CreateAsyncProperty<TIn, T>(
                Task<TIn> valueSource, Func<TIn, T> func, T defaultValue = default)
        {
            return new AsyncProperty<T>(
                    valueSource.ContinueWith(t => func(GetTaskResultSafe(t))),
                    defaultValue);
        }

        /// <summary>
        /// The CreateAsyncProperty
        /// </summary>
        /// <param name="sourceTask">The sourceTask<see cref="Task"/></param>
        /// <returns>The <see cref="AsyncProperty"/></returns>
        public static AsyncProperty CreateAsyncProperty(Task sourceTask)
        {
            return new AsyncProperty(sourceTask);
        }

        /// <summary>
        /// The CreateAsyncProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueSource">The valueSource<see cref="Task{T}"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="T"/></param>
        /// <returns>The <see cref="AsyncProperty{T}"/></returns>
        public static AsyncProperty<T> CreateAsyncProperty<T>(Task<T> valueSource, T defaultValue = default)
        {
            return new AsyncProperty<T>(valueSource, defaultValue);
        }

        /// <summary>
        /// The GetTaskResultSafe
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="task">The task<see cref="Task{TIn}"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="TIn"/></param>
        /// <returns>The <see cref="TIn"/></returns>
        public static TIn GetTaskResultSafe<TIn>(Task<TIn> task, TIn defaultValue = default)
        {
            try
            {
                return task.Result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion
    }
}