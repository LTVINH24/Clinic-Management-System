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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clinic_Management_System.Views.StaffView
{
    

    // chưa xử lí patientID: làm sao liên kết vs patientID ở trên
	// chưa xử lí doctorID: hiển thị list
	// id thằng Staff là id thằng đăng nhập
	// Cancel thì làm gì???
	// 
	// trường hợp người dùng cố tình nhập sai


	public sealed partial class AddMedicalExaminationForm : Page
    {
		public AddMedicalExaminationFormViewModel viewModel { get; set; } = new AddMedicalExaminationFormViewModel();
		public AddMedicalExaminationForm()
        {
            this.InitializeComponent();
			this.DataContext = viewModel;
        }

		
        
		private void Add_Button(object sender, RoutedEventArgs e)
		{
            viewModel.AddMedicalExaminationForm();
		}

		private void Cancel_Button(object sender, RoutedEventArgs e)
		{

		}

		private void Set_Gender(object sender, RoutedEventArgs e)
		{
			if (sender is MenuFlyoutItem menuItem)
			{
				GenderDropdown.Content = menuItem.Text;
			}
		}

		private void OnFilterChanged(object sender, TextChangedEventArgs e)
		{
			
		}
	}
}
