using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
	public class EditDateTimeOffsetConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is DateTimeOffset dateOffset)
			{
				return dateOffset;
			}
			else if (value is string dateString && !string.IsNullOrEmpty(dateString))
			{
				if (DateTimeOffset.TryParse(dateString, out DateTimeOffset result))
				{
					return result;
				}
			}
			return DateTimeOffset.Now;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is DateTimeOffset date)
			{
				return date;
			}
			return null;
		}
	}
}
