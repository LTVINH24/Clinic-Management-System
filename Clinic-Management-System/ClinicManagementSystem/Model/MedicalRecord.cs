using System;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp MedicalRecord chứa thông tin của bệnh án gồm các thuộc tính Id, DoctorId, MedicalExaminationFormID, Time, Diagnosis
	/// </summary>
	public class MedicalRecord
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int MedicalExaminationFormID { get; set; }
        public DateTime Time { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? NextExaminationDate { get; set; }
    }
}
