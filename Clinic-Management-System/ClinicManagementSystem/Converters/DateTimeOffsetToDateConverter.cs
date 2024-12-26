using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
	public class DateTimeOffsetToDateConverters : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is DateTimeOffset dateTimeOffset)
			{
				return dateTimeOffset.DateTime.ToString("dd/MM/yyyy");
			}
			else if (value is DateTime dateTime)
			{
				return dateTime.ToString("dd/MM/yyyy");
			}
			else if (value == null)
			{
				return "";
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is string dateString)
			{
				if (DateTime.TryParse(dateString, out DateTime dateTime))
				{
					return new DateTimeOffset(dateTime);
				}
			}
			return null;
		}
	}
}
