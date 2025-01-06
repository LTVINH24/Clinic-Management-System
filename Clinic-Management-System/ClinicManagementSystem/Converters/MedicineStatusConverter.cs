using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị trạng thái thuốc sang chuỗi
    /// </summary>
    public class MedicineStatusConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị trạng thái thuốc sang chuỗi
        /// </summary>
        /// <param name="value">Giá trị trạng thái thuốc</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị chuỗi</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status.ToLower() == "true" ? "Got Medicine" : "Not Yet";
            }
            return "Not Yet";
        }

		/// <summary>
		/// Chuyển đổi giá trị chuỗi sang trạng thái thuốc
		/// </summary>
		/// <param name="value">Giá trị chuỗi</param>
		/// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
		/// <param name="parameter">Tham số</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị trạng thái thuốc</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}