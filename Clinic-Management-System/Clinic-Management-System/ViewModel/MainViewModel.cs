using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls;
using System.Security.Cryptography;

namespace Clinic_Management_System.ViewModel
{
    public class MainViewModel
    {
        IDao _dao;
        public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public UserLogin UserLogin { get; set; } =new UserLogin();
        public event Action<string> LoginCompleted;
        public bool Authentication (UserLogin userLogin)
        {
            if (userLogin.Password == null || userLogin.Username == null)
            {
                LoginCompleted?.Invoke("Password or username is empty");
                return false;
            }
            string role = _dao.Authentication(userLogin.Username, userLogin.Password);
            if (role != "")
            {
                LoginCompleted?.Invoke(role);
                return true;
            }
            else
            {
                LoginCompleted?.Invoke("");
                return false;
            }
        }
        private UserViewModel userViewModfel { get; set; } = new UserViewModel();
        public void SavePassWord(UserLogin userLogin)
        {
            var checkLogin=Authentication(UserLogin);
            if(checkLogin)
            {
                var (encryptedPasswordInBase64, entropyInBase64) = userViewModfel.EncryptPassword(userLogin.Password);
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["username"] = userLogin.Username;
                localSettings.Values["password"] = encryptedPasswordInBase64;
                localSettings.Values["entropy"] = entropyInBase64;
            }    
        }
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
