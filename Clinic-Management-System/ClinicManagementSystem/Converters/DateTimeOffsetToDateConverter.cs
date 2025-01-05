using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
	/// <summary>
	/// Chuyển đổi giá trị DateTimeOffset sang chuỗi ngày tháng năm
	/// </summary>
	public class DateTimeOffsetToDateConverters : IValueConverter
	{
		/// <summary>
		/// Chuyển đổi giá trị DateTimeOffset sang chuỗi ngày tháng năm
		/// </summary>
		/// <param name="value">Giá trị DateTimeOffset</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Định dạng ngày tháng năm</returns>
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
		/// <summary>
		/// Chuyển đổi giá trị chuỗi ngày tháng năm sang DateTimeOffset
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
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
