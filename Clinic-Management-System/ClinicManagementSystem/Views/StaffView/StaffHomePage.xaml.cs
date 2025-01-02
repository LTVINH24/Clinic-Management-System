using ClinicManagementSystem.ViewModel.EndUser;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.StaffView
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StaffHomePage : Page
	{
		public StaffHomePageViewModel ViewModel { get; set; }

		public StaffHomePage()
		{
			ViewModel = new StaffHomePageViewModel();
			this.InitializeComponent();
		}
		/// <summary>
		/// Hàm chuyển hướng sang trang danh sách phiếu khám bệnh
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToListMedicalExaminationForm(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ListMedicalExaminationForm));
		}
		/// <summary>
		/// Hàm chuyển hướng sang trang danh sách bệnh nhân
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToListPatient(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ListPatient));
		}
		/// <summary>
		/// Hàm chuyển hướng sang trang thêm phiếu khám bệnh
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToAddMedicalExaminationForm(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(AddMedicalExaminationForm));
		}
	}
}
