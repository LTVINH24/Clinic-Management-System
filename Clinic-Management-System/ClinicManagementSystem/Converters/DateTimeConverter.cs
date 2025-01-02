using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Helper
{
	/// <summary>
	/// Chuyển đổi giá trị DateTime sang chuỗi ngày tháng năm, giờ phút
	/// </summary>
	public class DateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is DateTime dateTime)
			{
				return dateTime.ToString("dd/MM/yyyy HH:mm");
			}
			else if (value is DateTimeOffset dateTimeOffset)
			{
				return dateTimeOffset.LocalDateTime.ToString("dd/MM/yyyy HH:mm");
			}
			return value;
		}

		/// <summary>
		/// Chuyển đổi giá trị chuỗi ngày tháng năm, giờ phút sang DateTime
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
