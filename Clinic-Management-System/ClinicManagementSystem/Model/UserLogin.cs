using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp UserLogin chứa thông tin của người dùng khi đăng nhập gồm các thuộc tính Username, Password
	/// </summary>
	public class UserLogin:INotifyPropertyChanged
    {
		public string Username { get; set; }
        public string Password { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
