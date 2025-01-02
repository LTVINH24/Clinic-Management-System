using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp Medicine chứa thông tin của thuốc gồm các thuộc tính Id, Name, Manufacturer, Price, Quantity, QuantityImport, Dosage, DateImport, ExpDate, MfgDate
	/// </summary>
	public class Medicine : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public int Price { get; set; }
		public int Quantity { get; set; }       
		public int QuantityImport { get; set; }  
		public string Dosage { get; set; }     
		public DateTimeOffset DateImport { get; set; } = DateTimeOffset.Now;
		public DateTimeOffset? ExpDate { get; set; } = null;
		public DateTimeOffset? MfgDate { get; set; } = null;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
