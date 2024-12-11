using System;

public class Prescription
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int MedicineId { get; set; }
    public int Quantity { get; set; }
    public string Dosage { get; set; }
    public int MedicalExaminationFormId { get; set; }
}