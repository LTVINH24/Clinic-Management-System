using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ClinicManagementSystem.Converters
{
	/// <summary>
	/// Chuyển đổi giá trị Boolean sang Visibility
	/// </summary>
	public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

		/// <summary>
		/// Chuyển đổi giá trị Visibility sang Boolean
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
