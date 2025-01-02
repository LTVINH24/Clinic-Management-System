using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model.Statistic
{
	/// <summary>
	/// Thống kê hóa đơn
	/// </summary>
	public class BillStatistic : INotifyPropertyChanged
    {
        public DateTime CreateDate { get; set; }
        public int TotalAmount { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
