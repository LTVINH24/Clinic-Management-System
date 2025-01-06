using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
	/// <summary>
	/// Class chuyển đổi ngày tháng từ DateTimeOffset sang Date cho Canlendar
	/// </summary>
	public class DateTimeOffsetToDateForCanlendarConverters:IValueConverter
    {
		/// <summary>
		/// Chuyển đổi ngày tháng từ DateTimeOffset sang Date cho Canlendar
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns>Object theo định dạng</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return new DateTimeOffset(dateTimeOffset.Date);
            }
            return null;
        }
		/// <summary>
		/// Chuyển đổi ngày tháng từ Date sang DateTimeOffset cho Canlendar
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns>Object theo định dạnh</returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset date)
            {
                return new DateTimeOffset(date.Date);
            }
            return null;
        }
    }
}
