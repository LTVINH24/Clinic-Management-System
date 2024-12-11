using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp Medicine chứa thông tin của thuốc gồm các thuộc tính Id, Name, Manufacturer, Price, Quantity, QuantityUsed, Dosage, ExpDate, MfgDate
	/// </summary>
	public class Medicine : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public int Price { get; set; }
		public int Quantity { get; set; }       // Số lượng thuốc còn lại trong kho
		public int QuantityUsed { get; set; }   // Số lượng thuốc được kê đơn
		public string Dosage { get; set; }      // Liều dùng (số viên/ngày hoặc ghi chú khác)
		public DateTimeOffset ExpDate { get; set; } = DateTimeOffset.Now;
		public DateTimeOffset MfgDate { get; set; } = DateTimeOffset.Now;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
