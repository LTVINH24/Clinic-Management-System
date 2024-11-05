using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Model
{
	public class Doctor : User, INotifyCollectionChanged
	{
		public int Id { get; set; }
		public int SpecialtyId { get; set; }
		public string SpecialtyName { get; set; }
		public string Room { get; set; }

		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
