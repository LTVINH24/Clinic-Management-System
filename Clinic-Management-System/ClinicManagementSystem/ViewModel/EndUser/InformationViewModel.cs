using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Model;
namespace ClinicManagementSystem.ViewModel.EndUser
{
    public class InformationViewModel
    {
        IDao _dao;
        private int userId;
        public User user {  get; set; }
        public InformationViewModel()
        {
            userId = UserSessionService.Instance.LoggedInUserId;
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
		/// <summary>
		/// Lấy thông tin người dùng
		/// </summary>
		public void LoadInformationUser()
        {
            user = _dao.GetUserById(userId);
        }
		/// <summary>
		/// Cập nhật thông tin người dùng
		/// </summary>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool UpdateInformationUser()
        {
            if(user.password != null && user.password != "")
            {
                var password = new Password();
                user.password  = password.HashPassword(user.password);
            } 
            bool success = _dao.UpdateUser(user);
            user.password = "";
                
            return success;
        }
    }

}
