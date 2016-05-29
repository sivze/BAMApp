using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BAMApp.ViewModels
{
    public class BaseCommand : ICommand
    {
        #region Fields

        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        #endregion

        #region Event

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        public BaseCommand(Action<object> execute) : this(execute, null)
        {
        }

        public BaseCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }
        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void RaiseCanExecuteChanged(EventArgs e)
        {
            var handler = CanExecuteChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
        #endregion
    }
}
