using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị null hoặc chuỗi rỗng sang Visibility
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi giá trị null hoặc chuỗi rỗng sang Visibility
		/// </summary>
		/// <param name="value">Giá trị cần chuyển đổi</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

		/// <summary>
		/// Chuyển đổi giá trị Visibility sang null hoặc chuỗi rỗng
		/// </summary>
		/// <param name="value">Giá trị Visibility</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị null hoặc chuỗi rỗng</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 