using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Threading;

namespace Frontier.Wif.Core.Collections
{
    /// <summary>
    /// 表示一个动态数据集合，在添加项、移除项或刷新整个列表时，此集合将提供通知。支持跨线程更新，但不支持并发。.Net 4.5后可用。
    /// <remarks>
    /// 对比常规的 ObservableCollection 集合，此集合允许在非集合所有者线程上操作它，同时应用了Framework4.5版本中的 EnableCollectionSynchronization 接口。
    /// 更多细节 http://stackoverflow.com/questions/14336750/upgrading-to-net-4-5-an-itemscontrol-is-inconsistent-with-its-items-source.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">集合中的元素类型。</typeparam>
    [Serializable]
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// 定义同步锁对象。
        /// </summary>
        private readonly object _syncLock = new object();

        /// <summary>
        /// 定义线程同步上下文。
        /// </summary>
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncObservableCollection{T}" /> class.
        /// </summary>
        /// 
        public AsyncObservableCollection()
        {
            enableCollectionSynchronization(this, _syncLock);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="list">集合。</param>
        public AsyncObservableCollection(IEnumerable<T> list) : base(list)
        {
            enableCollectionSynchronization(this, _syncLock);
        }

        /// <summary>
        /// 触发属性改变事件。
        /// </summary>
        /// <param name="param"></param>
        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
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
        /// 当集合发生变化时触发。
        /// </summary>
        /// <param name="e">集合变化事件参数。</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var notifyCollectionChanged = CollectionChanged;
                if (notifyCollectionChanged == null) return;

                var dispatcher =
                        (from NotifyCollectionChangedEventHandler ncc in notifyCollectionChanged.GetInvocationList()
                                let dpo = ncc.Target as DispatcherObject
                                where dpo != null
                                select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => OnCollectionChanged(e)));
                else
                    foreach (var @delegate in notifyCollectionChanged.GetInvocationList())
                    {
                        var ncc = (NotifyCollectionChangedEventHandler) @delegate;
                        ncc.Invoke(this, e);
                    }
            }
        }

        /// <summary>
        /// 在集合发生改变时发生。
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

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
    }
}