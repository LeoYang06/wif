using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Frontier.Wif.Core.Generic;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    ///     表示一个动态数据集合，支持跨线程更新，它可在添加、删除项目或刷新整个列表时提供通知。
    ///     <remarks>确保集合在UI线程上实例化，后续修改集合可以从任何线程来完成。</remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("该集合类有 “某个ItemsControl与它的项源不一致” 异常风险，建议使用 ConcurrentObservableCollection 类")]
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Defines the _locker
        /// </summary>
        private readonly UsingLock<object> _locker = new UsingLock<object>();

        /// <summary>
        ///     获取或设置当前线程的SynchronizationContext对象。
        /// </summary>
        private SynchronizationContext _synchronizationContext;


        /// <summary>
        ///     无参构造函数。
        /// </summary>
        public AsyncObservableCollection()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        ///     有参构造函数。
        /// </summary>
        /// <param name="list">集合。</param>
        public AsyncObservableCollection(IEnumerable<T> list) : base(list)
        {
            SynchronizationContext = SynchronizationContext.Current;
        }


        /// <summary>
        ///     Gets the Count
        /// </summary>
        public new int Count
        {
            get
            {
                using (_locker.Read())
                {
                    var count = base.Count;
                    return count;
                }
            }
        }

        /// <summary>
        ///     获取或设置当前线程的SynchronizationContext对象。
        /// </summary>
        /// <summary>
        ///     Gets or sets the SynchronizationContext
        /// </summary>
        private SynchronizationContext SynchronizationContext
        {
            get => _synchronizationContext ?? SynchronizationContext.Current;
            set
            {
                if (value != null)
                    _synchronizationContext = value;
            }
        }


        /// <summary>
        ///     The Add
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        public new void Add(T item)
        {
            using (_locker.Write())
            {
                base.Add(item);
            }
        }

        /// <summary>
        ///     The Clear
        /// </summary>
        public new void Clear()
        {
            using (_locker.Write())
            {
                base.Clear();
            }
        }

        /// <summary>
        ///     The Contains
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        /// <returns>The <see cref="bool " /></returns>
        public new bool Contains(T item)
        {
            using (_locker.Read())
            {
                var result = base.Contains(item);
                return result;
            }
        }

        /// <summary>
        ///     The CopyTo
        /// </summary>
        /// <param name="array">The <see cref="T [ ]" /></param>
        /// <param name="arrayIndex">The <see cref="int " /></param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            using (_locker.Write())
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        ///     The GetEnumerator
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}" /></returns>
        public new IEnumerator<T> GetEnumerator()
        {
            using (_locker.Read())
            {
                var enumerator = base.GetEnumerator();
                return enumerator;
            }
        }

        /// <summary>
        ///     The IndexOf
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        /// <returns>The <see cref="int " /></returns>
        public new int IndexOf(T item)
        {
            using (_locker.Read())
            {
                var index = base.IndexOf(item);
                return index;
            }
        }

        /// <summary>
        ///     The Insert
        /// </summary>
        /// <param name="index">The <see cref="int " /></param>
        /// <param name="item">The <see cref="T" /></param>
        public new void Insert(int index, T item)
        {
            using (_locker.Write())
            {
                base.Insert(index, item);
            }
        }

        /// <summary>
        ///     The Remove
        /// </summary>
        /// <param name="item">The <see cref="T" /></param>
        public new void Remove(T item)
        {
            using (_locker.Write())
            {
                base.Remove(item);
            }
        }

        /// <summary>
        ///     The RemoveAt
        /// </summary>
        /// <param name="index">The <see cref="int " /></param>
        public new void RemoveAt(int index)
        {
            using (_locker.Write())
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        ///     重写集合改变事件处理函数。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == SynchronizationContext)
                    // 如果操作发生在UI线程中，不需要进行跨线程执行
                RaiseCollectionChanged(e);
            else
                    // 如果操作发生在非UI线程中。注：使用Send代替Post。使用Post导致在UI线程上异步引发事件，如果在前一个事件处理之前再次修改集合，可能会导致竞争条件。
                SynchronizationContext.Send(RaiseCollectionChanged, e);
        }

        /// <summary>
        ///     重写属性改变事件处理函数。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == SynchronizationContext)
                    // 如果操作发生在UI线程中，不需要进行跨线程执行
                OnPropertyChanged(e);
            else
                    // 如果操作发生在非UI线程中
                SynchronizationContext.Send(OnPropertyChanged, e);
        }

        /// <summary>
        ///     触发集合改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void OnCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param);
        }

        /// <summary>
        ///     触发属性改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void OnPropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs) param);
        }

        /// <summary>
        ///     触发集合改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void RaiseCollectionChanged(object param)
        {
            try
            {
                OnCollectionChanged(param);
            }
            catch (NotSupportedException)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    try
                    {
                        OnCollectionChanged(param);
                    }
                    catch
                    {
                        // Ignored
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                // Ignored
            }
        }


        /// <summary>
        ///     将list1与list2组合，并返回修改后的list1。
        /// </summary>
        /// <param name="list1">第一个集合</param>
        /// <param name="list2">第二个集合</param>
        /// <returns>修改后的list1实例</returns>
        public static AsyncObservableCollection<T> operator +(AsyncObservableCollection<T> list1, IList<T> list2)
        {
            foreach (var item in list2)
                list1.Add(item);
            return list1;
        }

        public new T this[int index]
        {
            get
            {
                using (_locker.Read())
                {
                    var t = base[index];
                    return t;
                }
            }

            set
            {
                using (_locker.Write())
                {
                    base[index] = value;
                }
            }
        }
    }
}