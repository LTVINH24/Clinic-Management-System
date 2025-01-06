using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Converters
{
    public class DateTimeOffsetToDateForCanlendarConverters:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return new DateTimeOffset(dateTimeOffset.Date);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset date)
            {
                return new DateTimeOffset(date.Date);
            }
            return null;
        }
    }
}
