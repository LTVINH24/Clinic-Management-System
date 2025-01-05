using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
	/// <summary>
	/// Chuyển đổi giá trị DateTimeOffset sang DateTimeOffset
	/// </summary>
	public class EditDateTimeOffsetConverter : IValueConverter
	{
		/// <summary>
		/// Chuyển đổi giá trị DateTimeOffset sang DateTimeOffset
		/// </summary>
		/// <param name="value">Giá trị DateTimeOffset</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị DateTimeOffset</returns>
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

		/// <summary>
		/// Chuyển đổi giá trị DateTimeOffset sang DateTimeOffset
		/// </summary>
		/// <param name="value">Giá trị DateTimeOffset</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị DateTimeOffset</returns>
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
