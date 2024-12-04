using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
    public class Specialty : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
