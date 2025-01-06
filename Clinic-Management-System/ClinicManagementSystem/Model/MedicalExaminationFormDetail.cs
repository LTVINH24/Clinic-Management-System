using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Class chứa thông tin phiếu khám bệnh gồm email bệnh nhân, chuẩn đoán, danh sách thuốc, ngày tái khám
	/// </summary>
	public class MedicalExaminationFormDetail : MedicalExaminationForm
	{
		public string PatientEmail { get; set; }
		public string Diagnosis { get; set; }
		public List<PrescriptionMedicine> Medicines { get; set; }
		public DateTime? NextExaminationDate { get; set; }
	}
	/// <summary>
	/// Class chứa thông tin thuốc gồm tên thuốc, liều lượng, số lượng
	/// </summary>
	public class PrescriptionMedicine
	{
		public string MedicineName { get; set; }
		public string Dosage { get; set; }
		public int Quantity { get; set; }
	}
}
