using ClinicManagementSystem.ViewModel.EndUser;
using ClinicManagementSystem.Views.StaffView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.AdminView
{
	/// <summary>
	/// Trang chủ của admin
	/// </summary>
	public sealed partial class AdminHomePage : Page
    {
       public AdminHomePageViewModel ViewModel { get; set; }
        public AdminHomePage()
        {
            this.InitializeComponent();
            ViewModel = new AdminHomePageViewModel();
        }
		/// <summary>
		/// Hàm chuyển hướng đến trang thêm tài khoản
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToAddUser(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addAccount));

        }
		/// <summary>
		/// Hàm chuyển hướng đến trang báo cáo doanh thu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToReportBill(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(reportBillAdmin));
        }
		/// <summary>
		/// Hàm chuyển hướng đến trang hiển thị danh sách tài khoản
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigateToListUser(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(listAccount));

        }
    }
}
