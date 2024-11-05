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
using Clinic_Management_System.ViewModel;
using System.Collections;
using Microsoft.IdentityModel.Protocols;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinic_Management_System.Views.StaffView
{
    

	// Dữ liệu trống (x)
	// Dữ liệu không đúng định dạng (x)
	// Dữ liệu nằm ngoài miền giá trị lưu vào database


	public sealed partial class AddMedicalExaminationForm : Page
    {
		public AddMedicalExaminationFormViewModel viewModel { get; set; } = new AddMedicalExaminationFormViewModel();
		public AddMedicalExaminationForm()
        {
            this.InitializeComponent();
			this.DataContext = viewModel;

			viewModel.AddCompleted += ViewModel_AddCompleted;
		}

        
		private void Add_Button(object sender, RoutedEventArgs e)
		{
            viewModel.AddMedicalExaminationForm();
		}

		private void Cancel_Button(object sender, RoutedEventArgs e)
		{
			viewModel.Reset();
		}

		private void Set_Gender(object sender, RoutedEventArgs e)
		{
			if (sender is MenuFlyoutItem menuItem)
			{
				GenderDropdown.Content = menuItem.Text;
			}
		}


		private async void ViewModel_AddCompleted(bool isSuccess, int statusCode, string message)
		{
			string displayMessage;

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
				XamlRoot = this.XamlRoot
			};

			if(isSuccess)
			{
				viewModel.Reset();
			}

			await dialog.ShowAsync();
		}
	}
}
