using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Service
{
	public class UserSessionService
	{
		// Singleton để đảm bảo chỉ có một instance của UserSessionService
		private static UserSessionService _instance;
		public static UserSessionService Instance => _instance ??= new UserSessionService();

		// ID người dùng đã đăng nhập
		public int LoggedInUserId { get; private set; }

		// Hàm thiết lập ID sau khi đăng nhập thành công
		public void SetLoggedInUserId(int userId)
		{
			LoggedInUserId = userId;
		}
	}
}
