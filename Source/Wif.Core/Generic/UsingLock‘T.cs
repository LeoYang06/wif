using System;
using System.Threading;

namespace Frontier.Wif.Core.Generic
{
    /// <summary> 可释放资源的基类或组件,可定义托管对象释放事件和非托管对象释放事件
    /// </summary>
    public class Disposable : IDisposable
    {
        public Disposable(object obj)
        {
            _obj = obj;
            _disposeMark = 0;
        }

        protected Disposable()
        {
            _obj = this;
            _auto = true;
            _disposeMark = 0;
        }

        ~Disposable()
        {
            if (_auto) Destructor();
        }

        public void Destructor()
        {
            //如果尚未释放对象(标记为0),则将标记改为2,否则标记不变
            Interlocked.CompareExchange(ref _disposeMark, 2, 0);
            Dispose();
        }

        private readonly object _obj;
        private readonly bool _auto;

        /// <summary> 释放标记,0未释放,1已释放,2执行了析构函数
        /// </summary>
        private int _disposeMark;

        /// <summary> 释放非托管资源
        /// </summary>
        public void Dispose()
        {
            //如果已释放(标记为1)则不执行任何操作
            if (_disposeMark == 1) return;
            //将标记改为1,并返回修改之前的值
            var mark = Interlocked.Exchange(ref _disposeMark, 1);
            //如果当前方法被多个线程同时执行,确保仅执行其中的一个
            if (mark == 1) return;
            try
            {
                ((IDisposable) this).Dispose();
            }
            finally
            {
                //释放非托管资源
                OnEvent(_disposeUnmanaged);
                _disposeUnmanaged = null;
                if (mark == 0)
                {
                    //释放托管资源
                    OnEvent(_disposeManaged);
                    _disposeManaged = null;
                    GC.SuppressFinalize(_obj);
                }
            }
        }

        public bool IsDisposed => _disposeMark != 0;

        public void Assert()
        {
            if (_disposeMark != 0)
            {
                if (_obj is string)
                    throw new ObjectDisposedException(_obj + "");
                throw new ObjectDisposedException(_obj.GetType().FullName);
            }
        }

        private void OnEvent(ThreadStart start)
        {
            var eve = start;
            if (eve != null)
            {
                var dele = eve.GetInvocationList();
                var length = dele.Length;
                for (var i = 0; i < length; i++)
                    try
                    {
                        ((ThreadStart) dele[i])();
                    }
                    catch
                    {
                    }
            }
        }

        private event ThreadStart _disposeManaged;

        private event ThreadStart _disposeUnmanaged;

        public event ThreadStart DisposeManaged
        {
            add
            {
                _disposeManaged -= value;
                _disposeManaged += value;
            }
            remove => _disposeManaged += value;
        }

        public event ThreadStart DisposeUnmanaged
        {
            add
            {
                _disposeUnmanaged -= value;
                _disposeUnmanaged += value;
            }
            remove => _disposeUnmanaged += value;
        }
    }

    /// <summary>
    /// Defines the <see cref="UsingLock" />
    /// </summary>
    public class UsingLock : Disposable, IDisposable
    {
        #region Fields

        /// <summary>
        /// Defines the _lockSlim
        /// </summary>
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsReadLocked
        /// </summary>
        public bool IsReadLocked => _lockSlim.IsReadLockHeld;

        /// <summary>
        /// Gets a value indicating whether IsWriteLocked
        /// </summary>
        public bool IsWriteLocked => _lockSlim.IsWriteLockHeld;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingLock"/> class.
        /// </summary>
        public UsingLock()
        {
            Enabled = true;
            DisposeManaged += _lockSlim.Dispose;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在读取或写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <returns>The <see cref="IDisposable"/></returns>
        public IDisposable Read()
        {
            if (Enabled == false || _lockSlim.IsReadLockHeld || _lockSlim.IsWriteLockHeld) return Disposable.Empty;

            _lockSlim.EnterReadLock();
            return new Lock(_lockSlim, false);
        }

        /// <summary>
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <returns>The <see cref="IDisposable"/></returns>
        public IDisposable Write()
        {
            if (Enabled == false || _lockSlim.IsWriteLockHeld) return Disposable.Empty;

            if (_lockSlim.IsReadLockHeld)
            {
                throw new NotImplementedException("读取模式下不能进入写入锁定状态");
            }

            _lockSlim.EnterWriteLock();
            return new Lock(_lockSlim, true);
        }

        #endregion

        /// <summary>
        /// <para>内部类</para>
        /// </summary>
        private struct Lock : IDisposable
        {
            #region Fields

            /// <summary>
            /// Defines the _IsWrite
            /// </summary>
            private readonly bool _IsWrite;

            /// <summary>
            /// Defines the _Lock
            /// </summary>
            private readonly ReaderWriterLockSlim _Lock;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref=""/> class.
            /// </summary>
            /// <param name="rwl">读写锁</param>
            /// <param name="isWrite">写入模式为true,读取模式为false</param>
            public Lock(ReaderWriterLockSlim rwl, bool isWrite)
            {
                _Lock = rwl;
                _IsWrite = isWrite;
            }

            #endregion

            #region Methods

            /// <summary>
            /// The Dispose
            /// </summary>
            public void Dispose()
            {
                if (_IsWrite)
                {
                    if (_Lock.IsWriteLockHeld) _Lock.ExitWriteLock();
                }
                else
                {
                    if (_Lock.IsReadLockHeld) _Lock.ExitReadLock();
                }
            }

            #endregion
        }

        /// <summary>
        /// <para>内部类</para>
        /// </summary>
        private class Disposable : IDisposable
        {
            #region Fields

            /// <summary>
            /// Defines the Empty
            /// </summary>
            public static readonly Disposable Empty = new Disposable();

            #endregion

            #region Methods

            /// <summary>
            /// The Dispose
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }
    }

    /// <summary>
    /// 定义 <see cref="UsingLock{T}" /> 自动释放的读写锁。
    /// </summary>
    /// <example>
    /// This sample shows how to use the UsingLock class.
    /// <code>
    /// UsingLock<object> _Lock = new UsingLock<object>();
    /// using(_Lock.Write())
    /// {
    /// }
    /// using(_Lock.Read())
    /// {
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public sealed class UsingLock<T> : UsingLock
    {
        #region Fields

        /// <summary>
        /// 定义锁对象变量。
        /// </summary>
        private T _lockObject;

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置锁对象实例。
        /// </summary>
        public T LockObject
        {
            get
            {
                if (IsReadLocked || IsWriteLocked) return _lockObject;
                throw new MemberAccessException("请先进入读取或写入锁定模式再进行操作");
            }
            set
            {
                if (IsWriteLocked == false) throw new MemberAccessException("只有写入锁定模式中才能改变LockObject的值");
                _lockObject = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingLock{T}"/> class.
        /// </summary>
        public UsingLock()
        {
            Enabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingLock{T}"/> class.
        /// </summary>
        /// <param name="lockObject">为 LockObject 属性设置初始值</param>
        public UsingLock(T lockObject)
        {
            Enabled = true;
            _lockObject = lockObject;
        }

        #endregion
    }
}