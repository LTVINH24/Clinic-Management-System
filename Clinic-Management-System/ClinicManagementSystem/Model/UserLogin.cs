using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
    public class UserLogin:INotifyPropertyChanged
    {
		public string Username { get; set; }
        public string Password { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
