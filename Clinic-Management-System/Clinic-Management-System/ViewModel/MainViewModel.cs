using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.ViewModel
{
    public class MainViewModel
    {
        IDao _dao;

		public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public UserLogin UserLogin { get; set; } = new UserLogin();
        public event Action<string> LoginCompleted;
		public bool Authentication(UserLogin userLogin)
		{
			if (userLogin.Password == null || userLogin.Username == null)
			{
				LoginCompleted?.Invoke("Password or username is empty");
				return false;
			}
			var (id, role) = _dao.Authentication(userLogin.Username, userLogin.Password);

			if (role != "")
			{
				UserSessionService.Instance.SetLoggedInUserId(id);
				LoginCompleted?.Invoke(role);
				return true;
			}
			else
			{
				LoginCompleted?.Invoke("");
				return false;
			}
		}

	}
}
