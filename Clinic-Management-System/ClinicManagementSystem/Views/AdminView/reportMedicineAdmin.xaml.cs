using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.ViewModel.Statistic;
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

namespace ClinicManagementSystem.Views.AdminView
{
	/// <summary>
	/// Trang báo cáo thuốc
	/// </summary>
	public sealed partial class reportMedicineAdmin : Page
    {
        public StatisticMedicineViewModel ViewModel { get; set; }
        public reportMedicineAdmin()
        {
            this.InitializeComponent();
           ViewModel = new StatisticMedicineViewModel();
            DataContext = ViewModel;
        }
		/// <summary>
		/// Hàm xử lí sự kiện khi click vào nút xem thống kê
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void viewStatistic(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
            ViewModel.UpdateChart();
        }

		/// <summary>
		/// Hàm xử lí sự kiện khi click vào nút xuất file excel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExportExcel(object sender, RoutedEventArgs e)
        {
            ViewModel.MedicineExportToExcel();
        }
    }
}
