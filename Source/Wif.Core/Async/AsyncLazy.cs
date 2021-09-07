/**************************************************************************
*      File Name£ºAsyncLazy.cs
*    Description£ºAsyncLazy.cs class description...
*      Copyright£ºCopyright ? 2020 LeoYang-Chuese. All rights reserved.
*        Creator£ºLeo Yang
*    Create Time£º2020/12/15
*Project Address£ºhttps://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Frontier.Wif.Core.Async
{
    /// <summary>
    /// Defines the <see cref="AsyncLazy{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AsyncLazy<T> : Lazy<Task<T>>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="valueFactory">The <see cref="Func{T}" /></param>
        public AsyncLazy(Func<T> valueFactory) :
                base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="taskFactory">The <see cref="Func{Task{T}}" /></param>
        public AsyncLazy(Func<Task<T>> taskFactory) :
                base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The GetAwaiter
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter{T}" /></returns>
        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }

        #endregion
    }
}