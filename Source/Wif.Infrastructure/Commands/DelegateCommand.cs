using System;
using System.Windows.Input;
using Frontier.Wif.Utils.Extensions;

namespace Frontier.Wif.Infrastructure.Commands
{
    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。手动调用<see cref="RaiseCanExecuteChanged" />刷新执行状态。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region Properties

        /// <summary>
        /// Gets the CanExecuteCommand
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Predicate<T> CanExecuteCommand { get; }

        /// <summary>
        /// Gets the ExecuteCommand
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action<T> ExecuteCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        public DelegateCommand() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        #endregion

        #region Events

        /// <summary>
        /// 调用 RaiseCanExecuteChanged 时引发。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Methods

        /// <summary>
        /// 确定此 <see cref="DelegateCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="DelegateCommand" />。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 用于引发 <see cref="CanExecuteChanged" /> 事件的方法
        ///     执行 <see cref="CanExecute" /> 的返回值
        ///     方法已更改。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。手动调用<see cref="RaiseCanExecuteChanged" />刷新执行状态。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Properties

        /// <summary>
        /// Gets the CanExecuteCommand
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Func<bool> CanExecuteCommand { get; }

        /// <summary>
        /// Gets the ExecuteCommand
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action ExecuteCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        public DelegateCommand() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action execute, bool canExecute)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = () => canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        #endregion

        #region Events

        /// <summary>
        /// 调用 RaiseCanExecuteChanged 时引发。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Methods

        /// <summary>
        /// 确定此 <see cref="DelegateCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand();
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="DelegateCommand" />。
        /// </summary>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke();
        }

        /// <summary>
        /// 用于引发 <see cref="CanExecuteChanged" /> 事件的方法
        ///     执行 <see cref="CanExecute" /> 的返回值
        ///     方法已更改。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}