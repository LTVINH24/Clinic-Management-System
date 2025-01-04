using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
    public class Bill
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IsGetMedicine { get; set; }

        // Navigation properties
        public Prescription Prescription { get; set; }
        public Patient Patient { get; set; }
        public int ExaminationFee { get; set; }
        public List<MedicineSelection> Medicines { get; set; }
    }
}
