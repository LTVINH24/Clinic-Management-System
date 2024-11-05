using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Model
{
	public class PageInfo : INotifyPropertyChanged
	{
		public int Page { get; set; }
		public int Total { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}

