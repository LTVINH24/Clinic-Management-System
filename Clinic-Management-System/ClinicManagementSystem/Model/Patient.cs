﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp Patient chứa thông tin của bệnh nhân gồm các thuộc tính Id, Name, Email, Resident, Address, DoB, Gender
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

		//public ICollection<MedicalExaminationForm> MedicalExaminationForms { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
