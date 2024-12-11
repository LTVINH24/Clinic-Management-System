using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	public class MedicalExaminationForm : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public int PatientId { get; set; }  
		public int StaffId { get; set; }    
		public int DoctorId { get; set; }  
		public DateTimeOffset? Time { get; set; }
		public string Symptoms { get; set; }
		public string VisitType { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

