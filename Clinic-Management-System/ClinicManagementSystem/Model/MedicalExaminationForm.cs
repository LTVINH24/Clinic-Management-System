using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp MedicalExaminationForm chứa thông tin của phiếu khám bệnh gồm các thuộc tính Id, PatientId, StaffId, DoctorId, Time, Symptoms, VisitType
	/// </summary>
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

