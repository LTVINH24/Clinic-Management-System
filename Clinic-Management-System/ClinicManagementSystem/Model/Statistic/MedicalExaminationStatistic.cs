using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model.Statistic
{
	/// <summary>
	/// Thống kê khám bệnh
	/// </summary>
	public class MedicalExaminationStatistic:INotifyPropertyChanged
    {
        public DateTime date { get; set; }
        public int amount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
