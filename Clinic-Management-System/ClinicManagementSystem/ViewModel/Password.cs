using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClinicManagementSystem.ViewModel
{
    public class Password
    {
		/// <summary>
		/// Mã hóa mật khẩu
		/// </summary>
		/// <param name="password"></param>
		/// <returns>Mật khẩu đã được mã hóa</returns>
		public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


		/// <summary>
		/// Xác thực mật khẩu
		/// </summary>
		/// <param name="password"></param>
		/// <param name="hashedPassword"></param>
		/// <returns>True nếu xác thực thành công, False nếu xác thực thất bại</returns>
		public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
