using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Class chứa thông tin phiếu khám bệnh gồm thông tin bệnh nhân, bác sĩ, thời gian, triệu chứng, loại khám, trạng thái
	/// </summary>
	public class MedicalExaminationForm : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public int PatientId { get; set; }  
		public int StaffId { get; set; }    
		public int DoctorId { get; set; }  
		public string PatientName { get; set; }
		public string DoctorName { get; set; }
		public DateTimeOffset? Time { get; set; }
		public string Symptoms { get; set; }
		public string VisitType { get; set; }
		public string Status { get; set; }
	
		public Visibility ShowSendMailButton()
		{
			return Status == "Examined" ? Visibility.Visible : Visibility.Collapsed;
		}

		public string IsExaminated { get; set; }

		public Patient Patient { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

