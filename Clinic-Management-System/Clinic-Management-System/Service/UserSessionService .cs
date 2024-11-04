using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Service
{
	public class UserSessionService
	{
		private static UserSessionService _instance;
		public static UserSessionService Instance => _instance ??= new UserSessionService();

		public int LoggedInUserId { get; private set; }


		public void SetLoggedInUserId(int userId)
		{
			LoggedInUserId = userId;
		}
	}
}
