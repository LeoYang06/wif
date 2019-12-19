using System;
using System.Collections.Generic;
using System.Windows.Input;
using Frontier.Wif.Core.Generic;

namespace Frontier.Wif.Infrastructure.Commands
{
    /// <summary>
    /// 专门用于中继自身功能的命令，通过调用委托分配给其他对象。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private List<WeakReference> _canExecuteChangedHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute action.</param>
        /// <param name="canExecute">The can execute predicate.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        #region ICommand Members

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value);
            remove => WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// True if this command can be executed, otherwise - false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void InvalidateCanExecute()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }
    }
}