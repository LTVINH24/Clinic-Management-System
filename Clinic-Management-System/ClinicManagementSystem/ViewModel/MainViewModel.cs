﻿using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls;
using System.Security.Cryptography;
using ClinicManagementSystem.ViewModel.EndUser;

namespace ClinicManagementSystem.ViewModel
{
    /// <summary>
    /// ViewModel cho Main
    /// </summary>
	public class MainViewModel
    {
        IDao _dao;

		public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public UserLogin UserLogin { get; set; } = new UserLogin();
        public event Action<string> LoginCompleted;

		/// <summary>
		/// Xác thực người dùng
		/// </summary>
		/// <param name="userLogin"></param>
		/// <param name="isSavePassword"></param>
		/// <returns>True nếu xác thực thành công, False nếu xác thực thất bại</returns>
		public bool Authentication (UserLogin userLogin, bool isSavePassword)
        {
            if (userLogin.Password == null || userLogin.Username == null)
            {
                LoginCompleted?.Invoke("Password or username is empty");
                return false;
            }
            var(id, name, role, phone, gender, address) = _dao.Authentication(userLogin.Username, userLogin.Password);
            if (role != "")
            {
                if (isSavePassword) {
                    SavePassWord(userLogin);
                    var localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["RememberMe"] = true;
                }
                else {
                    var localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["RememberMe"] = false;
                }
                LoginCompleted?.Invoke(role);
                UserSessionService.Instance.SetLoggedInUserId(id);
                return true;
            }
            else
            {
                LoginCompleted?.Invoke("");
                return false;
            }
        }
        private UserViewModel userViewModfel { get; set; } = new UserViewModel();

		/// <summary>
		/// Lưu thông tin đăng nhập
		/// </summary>
		/// <param name="password"></param>
		/// <returns>Mật khẩu và mật khẩu đã được mã hóa</returns>
		private (string, string) EncryptPassword(string password)
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropyInBytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropyInBytes);
            }
            var encryptedPassword = ProtectedData.Protect(passwordInBytes,
                        entropyInBytes, DataProtectionScope.CurrentUser);
            var encryptedPasswordInBase64 = Convert.ToBase64String(encryptedPassword);
            var entropyInBase64 = Convert.ToBase64String(entropyInBytes);
            return (encryptedPasswordInBase64, entropyInBase64);
        }

		/// <summary>
		/// Lưu mật khẩu
		/// </summary>
		/// <param name="userLogin"></param>
		public void SavePassWord(UserLogin userLogin)
        {
                var (encryptedPasswordInBase64, entropyInBase64) = EncryptPassword(userLogin.Password);
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["username"] = userLogin.Username;
                localSettings.Values["password"] = encryptedPasswordInBase64;
                localSettings.Values["entropy"] = entropyInBase64;  
        }

		/// <summary>
		/// Load mật khẩu
		/// </summary>
		/// <param name="usernameTextbox"></param>
		/// <param name="passwordBox"></param>
		public void LoadPassword(TextBox usernameTextbox, PasswordBox passwordBox)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("username"))
            {
               UserLogin.Username = localSettings.Values["username"].ToString();
                var encryptedPasswordInBase64 = localSettings.Values["password"].ToString();
                var entropyInBase64 = localSettings.Values["entropy"].ToString();
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordInBase64);
                var entropyInBytes = Convert.FromBase64String(entropyInBase64);
                var passwordInBytes = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                var password = Encoding.UTF8.GetString(passwordInBytes);
                UserLogin.Password= password;
            }
        }
    }
}
