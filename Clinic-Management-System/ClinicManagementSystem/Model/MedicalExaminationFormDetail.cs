using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	public class MedicalExaminationFormDetail : MedicalExaminationForm
	{
		public string PatientEmail { get; set; }
		public string Diagnosis { get; set; }
		public List<PrescriptionMedicine> Medicines { get; set; }
		public DateTime? NextExaminationDate { get; set; }
	}

	public class PrescriptionMedicine
	{
		public string MedicineName { get; set; }
		public int Dosage { get; set; }
		public int Quantity { get; set; }
	}
}
