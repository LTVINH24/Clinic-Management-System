using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị số lượng phần tử trong collection sang Visibility
    /// </summary>
    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi giá trị số lượng phần tử trong collection sang Visibility
		/// </summary>
		/// <param name="value">Giá trị số lượng phần tử trong collection</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

		/// <summary>
		/// Chuyển đổi giá trị Visibility sang số lượng phần tử trong collection
		/// </summary>
		/// <param name="value">Giá trị Visibility</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị số lượng phần tử trong collection</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 