using System;

/// <summary>
/// Lớp Prescription chứa thông tin của đơn thuốc gồm các thuộc tính Id, Time, MedicineId, Quantity, Dosage, MedicalExaminationFormId
/// </summary>
public class Prescription
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int MedicineId { get; set; }
    public int Quantity { get; set; }
    public string Dosage { get; set; }
    public int MedicalExaminationFormId { get; set; }
}