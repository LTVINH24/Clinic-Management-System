using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections;
using Microsoft.IdentityModel.Protocols;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Service;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.StaffView
{


	// Dữ liệu trống (x)
	// Dữ liệu không đúng định dạng (x)
	// Dữ liệu không đúng miền giá trị (x)


	public sealed partial class AddMedicalExaminationForm : Page
    {
		public AddMedicalExaminationFormViewModel viewModel { get; set; } = new AddMedicalExaminationFormViewModel();
		public AddMedicalExaminationForm()
        {
            this.InitializeComponent();
			this.DataContext = viewModel;

			viewModel.AddCompleted += ViewModel_AddCompleted;
		}

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Add
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Add_Button(object sender, RoutedEventArgs e)
		{
            viewModel.AddMedicalExaminationForm();
		}

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút Cancel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Cancel_Button(object sender, RoutedEventArgs e)
		{
			viewModel.Reset();
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn giới tính
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Set_Gender(object sender, RoutedEventArgs e)
		{
			if (sender is MenuFlyoutItem menuItem)
			{
				GenderDropdown.Content = menuItem.Text;
			}
		}

		/// <summary>
		/// Xử lí sự kiện khi thêm thành công
		/// </summary>
		/// <param name="isSuccess"></param>
		/// <param name="statusCode"></param>
		/// <param name="message"></param>
		private async void ViewModel_AddCompleted(bool isSuccess, int statusCode, string message)
		{
			string displayMessage;
			var currentTheme = ThemeService.Instance.GetCurrentTheme();
			ElementTheme dialogTheme;

			switch (currentTheme)
			{
				case "Light":
					dialogTheme = ElementTheme.Light;
					break;
				case "Dark":
					dialogTheme = ElementTheme.Dark;
					break;
				default:
					dialogTheme = ElementTheme.Default;
					break;
			}

			if (statusCode == 200 || statusCode == 201)
			{
				displayMessage = message;
			}
			else if (statusCode >= 301 && statusCode <= 308)
			{
				displayMessage = $"Failed to add medical examination form. {message}";
			}
			else
			{
				displayMessage = message;
			}

			ContentDialog dialog = new ContentDialog
			{
				Title = "Notification",
				Content = message,
				CloseButtonText = "OK",
				XamlRoot = this.XamlRoot,
				RequestedTheme = dialogTheme
			};

			if(isSuccess)
			{
				viewModel.Reset();
			}

			await dialog.ShowAsync();
		}

		/// <summary>
		/// Xử lí sự kiện khi chọn loại khám
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Set_VisitType(object sender, RoutedEventArgs e)
		{
			if(sender is MenuFlyoutItem menuItem)
			{
				VisitTypeDropDown.Content = menuItem.Text;
			}
        }

		private void DoctorFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			string filterText = DoctorFilter.Text.Trim();
			viewModel.LoadDoctors(filterText);
		}
    }
}
