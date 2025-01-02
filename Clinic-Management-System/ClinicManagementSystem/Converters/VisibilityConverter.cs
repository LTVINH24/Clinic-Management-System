using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public class VisibilityConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 