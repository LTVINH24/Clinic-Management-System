using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public class DateOnlyConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi ngày tháng từ DateTime sang định dạng dd/MM/yyyy
        /// </summary>
        /// <param name="value">Giá trị ngày tháng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Định dạng ngày tháng</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;
            
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy");
            }

            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime.ToString("dd/MM/yyyy");
            }

            return string.Empty;
        }

        /// <summary>
        /// Chuyển đổi ngày tháng từ định dạng dd/MM/yyyy sang DateTime
        /// </summary>
        /// <param name="value">Giá trị ngày tháng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị DateTime</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, 
                    System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            return null;
        }
    }
} 