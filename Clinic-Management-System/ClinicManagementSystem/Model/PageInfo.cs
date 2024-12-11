using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp PageInfo chứa thông tin của trang gồm các thuộc tính Page, Total
	/// </summary>
	public class PageInfo : INotifyPropertyChanged
	{
		public int Page { get; set; }
		public int Total { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}

