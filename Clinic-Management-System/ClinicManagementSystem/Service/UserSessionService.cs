using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Service
{
	/// <summary>
	/// Lớp UserSessionService chứa thông tin về phiên đăng nhập của người dùng
	/// </summary>
	public class UserSessionService
	{
		private static UserSessionService _instance;
		private int _loggedInUserId;
		private bool _isRemembered;
		
		private UserSessionService() { }
		public static UserSessionService Instance => _instance ??= new UserSessionService();
		
		
		
		public int LoggedInUserId { get; private set; }

		/// <summary>
		/// Hàm SetLoggedInUserId dùng để lưu id của người dùng đăng nhập vào hệ thống
		/// </summary>
		/// <param name="userId"></param>
		public void SetLoggedInUserId(int userId)
		{
			LoggedInUserId = userId;
		}
		/// <summary>
		/// Hàm GetLoggedInUserId dùng để lấy id của người dùng đăng nhập vào hệ thống
		/// </summary>
		/// <returns></returns>
		public int GetLoggedInUserId()
		{
			return LoggedInUserId;
		}

		/// <summary>
		/// Hàm ClearSession dùng để xóa thông tin phiên đăng nhập của người dùng
		/// </summary>
		/// <param name="isRemember"></param>
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
