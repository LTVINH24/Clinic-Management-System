using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
    public class User : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password {  get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string role { get; set; }
        public DateTimeOffset? birthday { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
