using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Service
{
	public class UserSessionService
	{
		private static UserSessionService _instance;
		private int _loggedInUserId;
		private bool _isRemembered;
		
		private UserSessionService() { }
		public static UserSessionService Instance => _instance ??= new UserSessionService();
		
		
		
		public int LoggedInUserId { get; private set; }


		public void SetLoggedInUserId(int userId)
		{
			LoggedInUserId = userId;
		}
		public int GetLoggedInUserId()
		{
			return LoggedInUserId;
		}

		public void ClearSession(bool isRemember = false)
		{
			LoggedInUserId = 0;
			
			var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			bool rememberMe = localSettings.Values.ContainsKey("RememberMe") && 
							(bool)localSettings.Values["RememberMe"];
			
			if (!rememberMe)
			{
				localSettings.Values.Remove("username");
				localSettings.Values.Remove("password");
				localSettings.Values.Remove("RememberMe");
			}
		}
	}
}
