using System.Collections.Generic;
using System;

namespace Clinic_Management_System.Model
{
    public class Prescription
    {
        public int Id { get; set; } // Mã đơn thuốc
        public int MedicalRecordId { get; set; } // Mã hồ sơ bệnh nhân
        public DateTime DateIssued { get; set; } // Ngày kê đơn
        public string Diagnosis { get; set; } // Chẩn đoán đi kèm

        // Danh sách thuốc trong đơn
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();

        // Phương thức để thêm thuốc vào đơn
        public void AddMedicine(Medicine medicine)
        {
            Medicines.Add(medicine);
        }
    }
}
