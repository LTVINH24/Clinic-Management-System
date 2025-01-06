using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị trạng thái thăm khám sang màu sắc
    /// </summary>
    public class ExaminationStatusColorConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị trạng thái thăm khám sang màu sắc
        /// </summary>
        /// <param name="value">Giá trị trạng thái thăm khám</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị màu sắc</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;
            return status?.ToLower() == "true" 
                ? new SolidColorBrush(Colors.Green) 
                : new SolidColorBrush(Colors.Orange);
        }

		/// <summary>
		/// Chuyển đổi giá trị màu sắc sang trạng thái thăm khám
		/// </summary>
		/// <param name="value">Giá trị màu sắc</param>
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