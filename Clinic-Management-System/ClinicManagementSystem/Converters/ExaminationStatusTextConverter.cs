using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị trạng thái thăm khám sang chuỗi
    /// </summary>
    public class ExaminationStatusTextConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị trạng thái thăm khám sang chuỗi
        /// </summary>
        /// <param name="value">Giá trị trạng thái thăm khám</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị chuỗi</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;
            return status?.ToLower() == "true" ? "Examined" : "Not examined";
        }

		/// <summary>
		/// Chuyển đổi giá trị chuỗi sang trạng thái thăm khám
		/// </summary>
		/// <param name="value">Giá trị chuỗi</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị trạng thái thăm khám</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 
