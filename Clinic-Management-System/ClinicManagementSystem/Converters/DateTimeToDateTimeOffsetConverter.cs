using System;
using Microsoft.UI.Xaml.Data;

namespace ClinicManagementSystem.Converters
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi giá trị DateTime sang DateTimeOffset 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return new DateTimeOffset(dateTime);
            }
            return null;
        }

		/// <summary>
		/// Chuyển đổi giá trị DateTimeOffset sang DateTime
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }
            return null;
        }
    }
}
