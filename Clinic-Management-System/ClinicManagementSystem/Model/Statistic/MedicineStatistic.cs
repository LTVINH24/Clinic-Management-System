using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Class chứa thông tin thống kê thuốc
	/// </summary>
	public class MedicineStatistic : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public string  MedicineName{ get; set; }
        public int QuantitySold {  get; set; }
        public int QuantityRemain { get; set; }
        public int Money { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
