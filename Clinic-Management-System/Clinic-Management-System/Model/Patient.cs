using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Model
{
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
