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
	// Dữ liệu không đúng định dạng
	// Dữ liệu nằm ngoài miền giá trị


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


		private async void ViewModel_AddCompleted(string result, int statusCode)
		{
			string message;

			switch (statusCode)
			{
				case 200:
					message = "Successfully added medical examination form!";
					break;
				case 201:
					message = "Successfully added medical examination form, patient already exists!";
					break;
				case 300:
					message = $"Failed to added medical examination form. {result}";
					break;
				default:
					message = "Failed to added medical examination form.";
					break;
			}

			

			ContentDialog dialog = new ContentDialog
			{
				Title = "Notification",
				Content = message,
				CloseButtonText = "OK",
				XamlRoot = this.XamlRoot

			};
			if(statusCode == 200 || statusCode == 201)
			{
				viewModel.Reset();
			}

			await dialog.ShowAsync();
		}
	}
}
