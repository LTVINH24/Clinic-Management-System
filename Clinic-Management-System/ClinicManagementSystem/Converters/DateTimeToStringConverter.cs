using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public sealed class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;

            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy HH:mm");
            }
            
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime.ToString("dd/MM/yyyy");
            }

            var nullableDateTime = value as DateTime?;
            if (nullableDateTime.HasValue)
            {
                return nullableDateTime.Value.ToString("dd/MM/yyyy HH:mm");
            }

            var nullableDateTimeOffset = value as DateTimeOffset?;
            if (nullableDateTimeOffset.HasValue)
            {
                return nullableDateTimeOffset.Value.DateTime.ToString("dd/MM/yyyy");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 