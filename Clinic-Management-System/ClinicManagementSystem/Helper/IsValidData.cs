using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ClinicManagementSystem.Helper
{
	public class IsValidData
	{
		// Name
		public bool IsValidName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}

			return Regex.IsMatch(name, @"^[\p{L}\s]+$");
		}

		// Email
		public bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}

			try
			{
				var mailAddress = new MailAddress(email);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		// ResidentID
		public bool IsValidResedentID(string residentID)
		{
			if (string.IsNullOrWhiteSpace(residentID))
			{
				return false;
			}
			return Regex.IsMatch(residentID, @"^\d{12}$");
		}

		// Address
		public bool IsValidAddress(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
			{
				return false;
			}

			return Regex.IsMatch(address, @"^[\p{L}\d\s\.,/]+$");
		}

		// DatePicker
		public bool IsValidDatePicker(DateTimeOffset? date)
		{
			return date != null && date.Value < DateTimeOffset.Now;
		}

		// Gender
		public bool IsValidGender(string gender)
		{
			return !string.IsNullOrWhiteSpace(gender);
		}

		// Text
		public bool IsValidDescription(string description)
		{
			return !string.IsNullOrWhiteSpace(description);
		}
	}
}
