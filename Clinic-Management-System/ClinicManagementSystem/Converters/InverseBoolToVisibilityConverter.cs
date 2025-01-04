using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace ClinicManagementSystem.Converters
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string isGetMedicine)
            {
                return isGetMedicine.ToLower() == "true" ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 