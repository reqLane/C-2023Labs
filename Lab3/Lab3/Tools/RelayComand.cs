using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab3.Tools
{
    internal class RelayComand<T> : ICommand
    {
        # region Fields
        readonly Action<T> _execute;
        readonly Predicate<T>? _canExecute;
        #endregion

        #region Constructors
        public RelayComand(Action<T> execute, Predicate<T>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public RelayComand(Action<T> execute) :
            this(execute, null)
        { }
        #endregion

        #region ICommand members
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        #endregion
    }
}
