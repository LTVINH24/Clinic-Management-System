using System;
using System.Windows.Input;

/// <summary>
/// Lớp RelayCommand để thực hiện các lệnh trong MVVM
/// </summary>
namespace ClinicManagementSystem.Command
{
    /// <summary>
    /// Lớp RelayCommand để thực hiện các lệnh trong MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Khởi tạo RelayCommand
        /// </summary>
        /// <param name="execute">Hành động cần thực hiện</param>
        /// <param name="canExecute">Hàm kiểm tra xem có thể thực hiện hành động hay không</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Kiểm tra xem có thể thực hiện hành động hay không
        /// </summary>
        /// <param name="parameter">Tham số</param>
        /// <returns>true nếu có thể thực hiện hành động, false nếu không</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Thực hiện hành động
        /// </summary>
        /// <param name="parameter">Tham số</param>
        public void Execute(object parameter)
        {
            _execute();
        }

        /// <summary>
        /// Kích hoạt sự kiện CanExecuteChanged
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
