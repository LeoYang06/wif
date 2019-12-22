using System;
using System.Windows.Input;
using Frontier.Wif.Utils.Extensions;

namespace Frontier.Wif.Infrastructure.Commands
{
    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。自动刷新执行状态。
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// 创建可始终执行的新命令。
        /// </summary>
        public RelayCommand()
        {
        }

        /// <summary>
        /// 创建新命令。
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        /// <summary>
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action<T> ExecuteCommand { get; }

        /// <summary>
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Predicate<T> CanExecuteCommand { get; }

        #region ICommand Members

        /// <summary>
        /// 确定此 <see cref="RelayCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">
        /// 命令使用的数据。如果不需要向命令传递数据，则可将此对象设置为 null。
        /// </param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="RelayCommand" />。
        /// </summary>
        /// <param name="parameter">
        /// 命令使用的数据。如果不需要向命令传递数据，则可将此对象设置为 null。
        /// </param>
        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke(parameter.SafeCast<T>());
        }

        /// <summary>
        /// 在发生影响命令是否应执行的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion
    }



    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。自动刷新执行状态。
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 创建可始终执行的新命令。
        /// </summary>
        public RelayCommand()
        {
        }

        /// <summary>
        /// 创建新命令。
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public RelayCommand(Action<object> execute, bool canExecute)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = () => canExecute;
        }

        /// <summary>
        /// 创建新命令。
        /// </summary>
        /// <param name="execute">执行逻辑。</param>
        /// <param name="canExecute">执行状态逻辑。</param>
        public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            ExecuteCommand = execute ?? throw new ArgumentNullException(nameof(execute));
            CanExecuteCommand = canExecute;
        }

        /// <summary>
        /// 获取或设置处理ICommand.Execute方法的操作。 如果未设置，ICommand.CanExecute将始终返回false。
        /// </summary>
        public Action<object> ExecuteCommand { get; }

        /// <summary>
        /// 获取或设置谓词以处理ICommand.CanExecute方法。如果未设置，则如果设置了ExecuteCallback，则ICommand.CanExecute将始终返回true。
        /// </summary>
        public Func<bool> CanExecuteCommand { get; }

        #region ICommand Members

        /// <summary>
        /// 确定此 <see cref="RelayCommand" /> 是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">
        /// 命令使用的数据。如果不需要向命令传递数据，则可将此对象设置为 null。
        /// </param>
        /// <returns>如果可执行此命令，则返回 true；否则返回 false。</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand();
        }

        /// <summary>
        /// 对当前命令目标执行 <see cref="RelayCommand" />。
        /// </summary>
        /// <param name="parameter">
        /// 命令使用的数据。如果不需要向命令传递数据，则可将此对象设置为 null。
        /// </param>
        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke(parameter);
        }

        /// <summary>
        /// 在发生影响命令是否应执行的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion
    }
}