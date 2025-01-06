using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    /// <summary>
    /// Chuyển đổi giá trị DateTime sang chuỗi ngày tháng năm, giờ phút giây
    /// </summary>
    public class DateTimeFullConverter : IValueConverter
    {
        /// <summary>
        /// Chuyển đổi giá trị DateTime sang chuỗi ngày tháng năm, giờ phút giây
        /// </summary>
        /// <param name="value">Giá trị DateTime</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Định dạng ngày tháng năm, giờ phút giây</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;
            
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime.ToString("dd/MM/yyyy HH:mm:ss");
            }

            return string.Empty;
        }

        /// <summary>
        /// Chuyển đổi ngày tháng từ định dạng dd/MM/yyyy HH:mm:ss sang DateTime
        /// </summary>
        /// <param name="value">Giá trị ngày tháng</param>
        /// <param name="targetType">Kiểu dữ liệu mục tiêu</param>
        /// <param name="parameter">Tham số</param>
        /// <param name="language">Ngôn ngữ</param>
        /// <returns>Giá trị DateTime</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateTimeString)
            {
                if (DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy HH:mm:ss", null, 
                    System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            return null;
        }
    }
} 