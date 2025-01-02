
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp Patient chứa thông tin của bệnh nhân gồm các thuộc tính Id, Name, Email, Resident, Address, DoB, Gender, NextExaminationDate
	/// </summary>
	public class Patient :INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string ResidentId { get; set; }
		public string Address { get; set; }
		public DateTimeOffset? DoB { get; set; } = null;
		public string Gender { get; set; }
		public DateTime? NextExaminationDate { get; set; }

		public Visibility ShowMailButton()  
		{
			return (NextExaminationDate != null && NextExaminationDate > DateTime.Now)
				   ? Visibility.Visible
				   : Visibility.Collapsed;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
