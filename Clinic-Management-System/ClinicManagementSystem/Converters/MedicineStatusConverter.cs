using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
    public class MedicineStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status.ToLower() == "true" ? "Got Medicine" : "Not Yet";
            }
            return "Not Yet";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}