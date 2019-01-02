using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    /// 表示对象的线程安全的动态数据集合，它可在添加、删除项目或刷新整个列表时提供通知。<remarks>.Net 4.5后可用</remarks>
    /// </summary>
    /// <typeparam name="T">集合中的元素类型。</typeparam>
    [Serializable]
    public class ConcurrentObservableCollection<T> : ObservableCollection<T>
    {
        #region Fields

        /// <summary>
        /// 线程安全锁对象。
        /// </summary>
        private static readonly object _threadSafeLock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}"/> class.
        /// </summary>
        public ConcurrentObservableCollection()
        {
            BindingOperations.EnableCollectionSynchronization(this, _threadSafeLock);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="list">集合。</param>
        public ConcurrentObservableCollection(IEnumerable<T> list) : base(list)
        {
            BindingOperations.EnableCollectionSynchronization(this, _threadSafeLock);
        }

        #endregion
    }
}