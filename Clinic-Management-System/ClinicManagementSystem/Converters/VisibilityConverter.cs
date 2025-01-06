using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị số lượng thành Visibility
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị số lượng thành Visibility
        /// </summary>
        /// <param name="value">Giá trị số lượng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int count)
            {
                bool isZero = parameter?.ToString() == "Zero";
                return isZero ? 
                    (count == 0 ? Visibility.Visible : Visibility.Collapsed) : 
                    (count > 0 ? Visibility.Visible : Visibility.Collapsed);
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Chuyển đổi giá trị Visibility sang số lượng
        /// </summary>
        /// <param name="value">Giá trị Visibility</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị số lượng</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 