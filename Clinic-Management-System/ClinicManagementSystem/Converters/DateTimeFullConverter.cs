using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public class DateTimeFullConverter : IValueConverter
    {
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