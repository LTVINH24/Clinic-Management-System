using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
	/// <summary>
	/// Class chuyển đổi ngày tháng sang định dạng dd/MM/yyyy
	/// </summary>
	public class DateConverter : IValueConverter
    {
        /// <summary>
        /// C
        /// </summary>
        /// <param name="value">Giá trị ngày tháng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Định dạng ngày tháng</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }

        /// <summary>
        /// Chuyển đổi ngày tháng từ định dạng dd/MM/yyyy sang DateTimeOffset
        /// </summary>
        /// <param name="value">Giá trị ngày tháng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị DateTimeOffset</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 