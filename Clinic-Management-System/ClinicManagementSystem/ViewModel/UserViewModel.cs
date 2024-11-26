using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using Windows.Storage;

namespace ClinicManagementSystem.ViewModel
{
    
    public class UserViewModel
    {
        IDao _dao;
        public UserViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public User user { get; set; } =new User();
        private ValidData valid {  get; set; } =new ValidData();
        public string CreateUser(User user)
        {
            string error = valid.IsNotValid(user);
            
            if (error == "")
            {
                if (_dao.CheckUserExists(user.username))
                {
                    return "Username already exists";
                }
                else
                {
                //var (encryptedPasswordInBase64, entropyInBase64) = EncryptPassword(user.password); 
                var success=_dao.CreateUser(user);
                return success?"":"Create false";
                }    
            }
            else
            {
                return $"{error} is invalid or empty";
                
            }
        }
       
        public (string,string) EncryptPassword(string password) 
        {
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropyInBytes = new Byte[20];
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
    }
    
}
