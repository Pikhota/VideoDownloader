using System;
using System.Windows.Input;

namespace VDownLoader.Commands
{
	public class DelegateCommand : ICommand
	{
		#region private members
		private Action<object> _execute;
		private Func<object, bool> _canExecute;
		#endregion

		#region events
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested += value; }
		}
		#endregion

		#region constructor
		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion

		#region public methods
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
		#endregion
	}
}
