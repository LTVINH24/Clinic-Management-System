using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ClinicManagementSystem.Helper
{
	/// <summary>
	/// Kiểm tra dữ liệu nhập vào
	/// </summary>
	public class IsValidData
	{
		/// <summary>
		/// Kiểm tra tên
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool IsValidName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}

			return Regex.IsMatch(name, @"^[\p{L}\s]+$");
		}

		/// <summary>
		/// Kiểm tra số điện thoại
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            string pattern = @"^\+?[0-9\s\-\(\)]{10,15}$";
            return Regex.IsMatch(phone, pattern);
        }

		/// <summary>
		/// Kiểm tra tên đăng nhập
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

          
            string pattern = @"^[a-zA-Z0-9_]{3,}$";
            return Regex.IsMatch(username, pattern);
        }

		/// <summary>
		/// Kiểm tra mật khẩu
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Password phải có ít nhất 8 ký tự, chứa ít nhất một chữ cái viết hoa, một chữ cái thường, một số và một ký tự đặc biệt
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

		/// <summary>
		/// Kiểm tra email
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}

			try
			{
				var mailAddress = new MailAddress(email);

				// length
				if (email.Length > 254) 
					return false;

				// valid domain
				var domain = email.Split('@')[1];
				if (!domain.Contains(".") || domain.EndsWith("."))
					return false;

				// character
				string invalidChars = "()<>[]\\,;: ";
				if (email.Any(c => invalidChars.Contains(c)))
					return false;

				// local part, domain
				var regex = new Regex(
					@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
					RegexOptions.IgnoreCase);

				return regex.IsMatch(email);
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// Kiểm tra số CCCD
		/// </summary>
		/// <param name="residentID"></param>
		/// <returns></returns>
		public bool IsValidResedentID(string residentID)
		{
			if (string.IsNullOrWhiteSpace(residentID))
			{
				return false;
			}
			return Regex.IsMatch(residentID, @"^\d{12}$");
		}

		/// <summary>
		/// Kiểm tra địa chỉ
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public bool IsValidAddress(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
			{
				return false;
			}

			return Regex.IsMatch(address, @"^[\p{L}\d\s\.,/]+$");
		}

		/// <summary>
		/// Kiểm tra ngày tháng
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public bool IsValidDatePicker(DateTimeOffset? date)
		{
			return date != null && date.Value < DateTimeOffset.Now;
		}

		/// <summary>
		/// Kiểm tra ngày tháng
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool IsValidTwoDatePicker(DateTimeOffset? a , DateTimeOffset? b)
        {
            return a != null &&b!=null && a <= b;
        }

		/// <summary>
		/// Kiểm tra giới tính
		/// </summary>
		/// <param name="gender"></param>
		/// <returns></returns>
		public bool IsValidGender(string gender)
		{
			return !string.IsNullOrWhiteSpace(gender);
		}

		/// <summary>
		/// Kiểm tra mô tả
		/// </summary>
		/// <param name="description"></param>
		/// <returns></returns>
		public bool IsValidDescription(string description)
		{
			return !string.IsNullOrWhiteSpace(description);
		}
        public bool IsValidEmpty(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
