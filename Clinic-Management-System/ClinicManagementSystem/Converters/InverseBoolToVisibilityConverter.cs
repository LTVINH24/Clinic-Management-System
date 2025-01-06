using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị boolean sang Visibility
    /// </summary>
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị boolean sang Visibility
        /// </summary>
        /// <param name="value">Giá trị boolean</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string isGetMedicine)
            {
                return isGetMedicine.ToLower() == "true" ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

		/// <summary>
		/// Chuyển đổi giá trị Visibility sang boolean
		/// </summary>
		/// <param name="value">Giá trị Visibility</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị boolean</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 