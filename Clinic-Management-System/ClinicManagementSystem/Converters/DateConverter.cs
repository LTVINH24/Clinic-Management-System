using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 