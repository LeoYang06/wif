/**************************************************************************
*      File Name：UsingLock‘T.cs
*    Description：UsingLock‘T.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Threading;

namespace Frontier.Wif.Core.Generic
{
    /// <summary>
    /// Defines the <see cref="UsingLock{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UsingLock<T> : IDisposable
    {
        /// <summary>
        /// 定义读写锁。
        /// </summary>
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// 定义保存的数据。
        /// <para>构造函数</para>
        /// </summary>
        private T _data;

        /// <summary>
        /// 使用using代替lock操作的对象,可指定写入和读取锁定模式。
        /// <para>构造函数</para>
        /// </summary>
        public UsingLock()
        {
            Enabled = true;
        }

        /// <summary>
        /// 使用using代替lock操作的对象,可指定写入和读取锁定模式。
        /// </summary>
        /// <param name="data">为Data属性设置初始值</param>
        public UsingLock(T data)
        {
            Enabled = true;
            _data = data;
        }

        /// <summary>
        /// 获取或设置当前对象中保存数据的值。
        /// </summary>
        /// <exception cref="MemberAccessException">获取数据时未进入读取或写入锁定模式</exception>
        /// <exception cref="MemberAccessException">设置数据时未进入写入锁定模式</exception>
        public T Data
        {
            get
            {
                if (_lockSlim.IsReadLockHeld || _lockSlim.IsWriteLockHeld)
                    return _data;
                throw new MemberAccessException("请先进入读取或写入锁定模式再进行操作");
            }
            set
            {
                if (_lockSlim.IsWriteLockHeld == false)
                    throw new MemberAccessException("只有写入模式中才能改变Data的值");
                _data = value;
            }
        }

        /// <summary>
        /// 获取或设置是否启用,当该值为false时,Read()和Write()方法将返回 Disposable.Empty。
        /// </summary>
        public bool Enabled { get; set; }

        #region IDisposable Members

        /// <summary>
        /// 释放锁资源。
        /// </summary>
        public void Dispose()
        {
            _lockSlim.Dispose();
        }

        #endregion

        /// <summary>
        ///     <para>退出读锁请将返回对象释放,建议使用using语块</para>
        ///     <para>Enabled为false时,返回Disposable.Empty;</para>
        ///     <para>在读取或写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <returns>The <see cref="IDisposable" /></returns>
        public IDisposable Read()
        {
            if (Enabled == false || _lockSlim.IsReadLockHeld || _lockSlim.IsWriteLockHeld)
                return Disposable.Empty;

            _lockSlim.EnterReadLock();
            return new Lock(_lockSlim, false);
        }

        /// <summary>
        ///     <para>退出读锁请将返回对象释放,建议使用using语块</para>
        ///     <para>Enabled为false时,返回Disposable.Empty;</para>
        ///     <para>在写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <returns>The <see cref="IDisposable" /></returns>
        public IDisposable Write()
        {
            if (Enabled == false || _lockSlim.IsWriteLockHeld)
                return Disposable.Empty;

            if (_lockSlim.IsReadLockHeld)
                throw new NotImplementedException("读取模式下不能进入写入锁定状态");

            _lockSlim.EnterWriteLock();
            return new Lock(_lockSlim, true);
        }

        #region Nested type: Disposable

        /// <summary>
        /// 空的可释放对象,免去了调用时需要判断是否为null的问题。
        /// <para>内部类</para>
        /// </summary>
        private class Disposable : IDisposable
        {
            /// <summary>
            /// 空的可释放对象。
            /// </summary>
            public static readonly Disposable Empty = new Disposable();

            #region IDisposable Members

            /// <summary>
            /// 空的释放方法。
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: Lock

        /// <summary>
        /// 利用IDisposable的using语法糖方便的释放锁定操作。
        /// <para>内部类</para>
        /// </summary>
        private struct Lock : IDisposable
        {
            /// <summary>
            /// 定义读写锁对象。
            /// </summary>
            private readonly ReaderWriterLockSlim _Lock;

            /// <summary>
            /// 是否为写入模式。
            /// </summary>
            private readonly bool _IsWrite;

            /// <summary>
            /// 利用IDisposable的using语法糖方便的释放锁定操作。
            /// </summary>
            /// <param name="rwl">读写锁</param>
            /// <param name="isWrite">写入模式为true,读取模式为false</param>
            public Lock(ReaderWriterLockSlim rwl, bool isWrite)
            {
                _Lock = rwl;
                _IsWrite = isWrite;
            }

            /// <summary>
            /// 释放对象时退出指定锁定模式。
            /// </summary>
            public void Dispose()
            {
                if (_IsWrite)
                {
                    if (_Lock.IsWriteLockHeld)
                        _Lock.ExitWriteLock();
                }
                else
                {
                    if (_Lock.IsReadLockHeld)
                        _Lock.ExitReadLock();
                }
            }
        }

        #endregion
    }
}