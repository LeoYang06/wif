/**************************************************************************
*      File Name：DelegateCommand.cs
*    Description：DelegateCommand.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Collections.Generic;
using System.Windows.Input;
using Frontier.Wif.Core.Generic;
using Frontier.Wif.Utilities.Extensions;

namespace Frontier.Wif.Infrastructure.Commands
{
    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。手动调用<see cref="RaiseCanExecuteChanged" />刷新执行状态。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region ICommand Members

        #region Events

        /// <summary>
        /// 调用 RaiseCanExecuteChanged 时引发。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value);
            remove => WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Predicate<T> CanExecuteCommand { get; }

        /// <summary>
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action<T> ExecuteCommand { get; }

        /// <summary>
        /// 定义是否能执行命令状态改变事件处理函数。
        /// </summary>
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}" /> class.
        /// </summary>
        public DelegateCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 确定此 <see cref="DelegateCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object" /></param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="DelegateCommand" />。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object" /></param>
        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 用于引发 <see cref="CanExecuteChanged" /> 事件的方法
        /// 执行 <see cref="CanExecute" /> 的返回值
        /// 方法已更改。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }

        #endregion
    }



    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。手动调用<see cref="RaiseCanExecuteChanged" />刷新执行状态。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region ICommand Members

        #region Events

        /// <summary>
        /// 调用 RaiseCanExecuteChanged 时引发。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value);
            remove => WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Func<bool> CanExecuteCommand { get; }

        /// <summary>
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action<object> ExecuteCommand { get; }

        /// <summary>
        /// 定义是否能执行命令状态改变事件处理函数。
        /// </summary>
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        public DelegateCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action<object> execute, bool canExecute)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = () => canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 确定此 <see cref="DelegateCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object" /></param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand();
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="DelegateCommand" />。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object" /></param>
        public void Execute(object parameter = null)
        {
            ExecuteCommand?.Invoke(parameter);
        }

        /// <summary>
        /// 用于引发 <see cref="CanExecuteChanged" /> 事件的方法
        /// 执行 <see cref="CanExecute" /> 的返回值
        /// 方法已更改。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }

        #endregion
    }
}