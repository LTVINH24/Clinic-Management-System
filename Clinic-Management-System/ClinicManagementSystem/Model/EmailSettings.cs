using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Class chứa thông tin cấu hình email
	/// </summary>
	public class EmailSettings
	{
		public string FromEmail { get; set; }
		public string Password { get; set; }
		public string SmtpServer { get; set; }
		public int Port { get; set; }
		public string DisplayName { get; set; }
	}
}
