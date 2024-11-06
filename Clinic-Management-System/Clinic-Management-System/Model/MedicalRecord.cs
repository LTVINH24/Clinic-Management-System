using System;

namespace Clinic_Management_System.Model
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int MedicalExaminationFormID { get; set; }
        public DateTime Time { get; set; }
        public string Diagnosis { get; set; }
    }
}
