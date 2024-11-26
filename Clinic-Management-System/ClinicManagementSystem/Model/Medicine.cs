namespace ClinicManagementSystem.Model
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }       // Số lượng thuốc còn lại trong kho
        public int QuantityUsed { get; set; }   // Số lượng thuốc được kê đơn
        public string Dosage { get; set; }      // Liều dùng (số viên/ngày hoặc ghi chú khác)
    }
}
