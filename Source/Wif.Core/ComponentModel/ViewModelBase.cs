using System;
using System.Windows;
using System.Windows.Threading;
using Frontier.Wif.Utils.Helpers;

namespace Frontier.Wif.Core.ComponentModel
{
    /// <summary>
    ///     视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : PropertyChangedBase, IDisposable
    {
        /// <summary>
        /// 是否为设计模式。
        /// </summary>
        public static bool IsInDesignModeStatic => DesignerPropertiesHelper.IsInDesignMode;

        /// <summary>
        /// 是否为设计模式。
        /// </summary>
        public bool IsInDesignMode => IsInDesignModeStatic;


        /// <summary>
        /// 在UI线程上调用指定的操作。
        /// </summary>
        /// <param name="action">在UI线程上调用的操作。</param>
        public static void InvokeOnUIThread(Action action)
        {
            var dispatcher = Application.Current != null && Application.Current.Dispatcher != null
                    ? Application.Current.Dispatcher
                    : Dispatcher.CurrentDispatcher;

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// 析构函数。
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        /// <param name="disposing">释放托管和非托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO 释放托管资源。
            }

            // TODO 释放非托管资源。
        }
    }
}