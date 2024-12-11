using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp Doctor kế thừa từ lớp User, thêm các thuộc tính chuyên khoa, phòng khám
	/// </summary>
	public class Doctor : User, INotifyCollectionChanged
	{
		public int Id { get; set; }
		public int SpecialtyId { get; set; }
		public string SpecialtyName { get; set; }
		public string Room { get; set; }

		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
