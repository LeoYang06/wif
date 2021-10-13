/**************************************************************************
*      File Name：ConcurrentObservableCollection’T.cs
*    Description：ConcurrentObservableCollection’T.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Data;
using Frontier.Wif.Core.Generic;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    /// 表示一个动态数据集合，在添加项、移除项或刷新整个列表时，此集合将提供通知。支持跨线程和并发更新。.Net 4.5后可用。
    /// </summary>
    /// <typeparam name="T">集合中的元素类型。</typeparam>
    [Serializable]
    public class ConcurrentObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// 定义线程同步上下文。
        /// </summary>
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        /// <summary>
        /// 定义同步锁对象。
        /// </summary>
        private readonly UsingLock<object> _syncLock = new UsingLock<object>();

        /// <summary>
        /// 在集合发生改变时发生。
        /// </summary>
        private NotifyCollectionChangedEventHandler _collectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}" /> class.
        /// </summary>
        public ConcurrentObservableCollection()
        {
            using (_syncLock.Read())
            {
                enableCollectionSynchronization(this, _syncLock);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="list">集合。</param>
        public ConcurrentObservableCollection(IEnumerable<T> list) : base(list)
        {
            using (_syncLock.Read())
            {
                enableCollectionSynchronization(this, _syncLock);
            }
        }

        /// <summary>
        /// 获取集合中实际包含的元素数。
        /// </summary>
        public new int Count
        {
            get
            {
                using (_syncLock.Read())
                {
                    var count = base.Count;
                    return count;
                }
            }
        }

        /// <summary>
        /// 根据索引获取项。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns></returns>
        public new T this[int index]
        {
            get
            {
                using (_syncLock.Read())
                {
                    var t = base[index];
                    return t;
                }
            }
            set
            {
                using (_syncLock.Write())
                {
                    base[index] = value;
                }
            }
        }

        /// <summary>
        /// 重写集合发生变化事件处理函数。
        /// </summary>
        /// <param name="e">集合变化事件参数。</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // 不允许可重入的更改此集合的尝试。
            using (BlockReentrancy())
            {
                if (_collectionChanged != null)
                {
                    var delegates = _collectionChanged.GetInvocationList();

                    // 遍历调用列表。
                    foreach (var @delegate in delegates)
                    {
                        var handler = (NotifyCollectionChangedEventHandler) @delegate;
                        // 如果订阅者是DispatcherObject和不同的线程。
                        if (handler != null)
                            // 目标调度程序线程中的调用处理程序。
                            handler.Invoke(handler, e);
                        else // 按原样执行处理程序。
                            _collectionChanged(this, e);
                    }
                }
            }
        }

        /// <summary>
        /// 在集合发生改变时发生。
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => _collectionChanged += value;
            remove => _collectionChanged -= value;
        }

        /// <summary>
        /// 使集合同步。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="lockObject"></param>
        private static void enableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            var method = typeof(BindingOperations).GetMethod("EnableCollectionSynchronization",
                new[] {typeof(IEnumerable), typeof(object)});
            if (method != null)
                // It's .NET 4.5
                method.Invoke(null, new[] {collection, lockObject});
        }

        /// <summary>
        /// 将对象添加到集合的结尾处。
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        public new void Add(T item)
        {
            using (_syncLock.Write())
            {
                base.Add(item);
            }
        }

        /// <summary>
        /// 将对象添加到集合的结尾处。
        /// </summary>
        public new void Clear()
        {
            using (_syncLock.Write())
            {
                base.Clear();
            }
        }

        /// <summary>
        /// 确定某元素是否在集合中。
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        /// <returns>The <see cref="bool " /></returns>
        public new bool Contains(T item)
        {
            using (_syncLock.Read())
            {
                var result = base.Contains(item);
                return result;
            }
        }

        /// <summary>
        /// 从目标数组的指定索引处开始将整个集合复制到兼容的一维数组中。
        /// </summary>
        /// <param name="array">The <see cref="T [ ]" /></param>
        /// <param name="arrayIndex">The <see cref="int " /></param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            using (_syncLock.Write())
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}" /></returns>
        public new IEnumerator<T> GetEnumerator()
        {
            using (_syncLock.Read())
            {
                var enumerator = base.GetEnumerator();
                return enumerator;
            }
        }

        /// <summary>
        /// 搜索指定的对象，并返回整个集合中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        /// <returns>The <see cref="int " /></returns>
        public new int IndexOf(T item)
        {
            using (_syncLock.Read())
            {
                var index = base.IndexOf(item);
                return index;
            }
        }

        /// <summary>
        /// 将元素插入集合的指定索引处。
        /// </summary>
        /// <param name="index">The <see cref="int " /></param>
        /// <param name="item">The <see cref="T" /></param>
        public new void Insert(int index, T item)
        {
            using (_syncLock.Write())
            {
                base.Insert(index, item);
            }
        }

        /// <summary>
        /// 从集合中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        public new void Remove(T item)
        {
            using (_syncLock.Write())
            {
                base.Remove(item);
            }
        }

        /// <summary>
        /// 移除集合的指定索引处的元素。
        /// </summary>
        /// <param name="index">The <see cref="int " /></param>
        public new void RemoveAt(int index)
        {
            using (_syncLock.Write())
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        /// 重写属性发生改变事件处理函数。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
                //如果操作发生在UI线程中，不需要进行跨线程执行
                RaisePropertyChanged(e);
            else
                // 如果操作发生在非UI线程中
                _synchronizationContext?.Post(RaisePropertyChanged, e);
        }

        /// <summary>
        /// 触发集合改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void OnCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param);
        }

        /// <summary>
        /// 触发属性改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs) param);
        }

        /// <summary>
        /// 将list1与list2组合，并返回修改后的list1。
        /// </summary>
        /// <param name="list1">第一个集合</param>
        /// <param name="list2">第二个集合</param>
        /// <returns>修改后的list1实例</returns>
        public static ConcurrentObservableCollection<T> operator +(ConcurrentObservableCollection<T> list1,
            IList<T> list2)
        {
            foreach (var item in list2)
                list1.Add(item);
            return list1;
        }
    }
}