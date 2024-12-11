//using Microsoft.UI.Xaml.Data;
//using System;
//using System.Globalization;

//public class DateTimeToStringConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is DateTime dateTime)
//        {
//            return dateTime.ToString("dd/mm/yyyy");
//        }
//        return value;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (DateTime.TryParse(value as string, out DateTime dateTime))
//        {
//            return dateTime;
//        }
//        return value;
//    }
//}
