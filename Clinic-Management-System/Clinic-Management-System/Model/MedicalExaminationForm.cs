using System;

namespace Clinic_Management_System.Model
{
    public class MedicalExaminationForm
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int StaffId { get; set; }
        public DateTime Time { get; set; }
        public string Symptom { get; set; }
        public int DoctorId { get; set; }
    }
}