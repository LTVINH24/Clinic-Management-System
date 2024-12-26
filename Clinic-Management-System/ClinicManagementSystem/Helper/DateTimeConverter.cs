using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Helper
{
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

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
