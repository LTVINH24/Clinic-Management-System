﻿using ClinicManagementSystem.ViewModel.Statistic;
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
	/// Trang báo cáo thông tin khám bệnh
	/// </summary>
	public sealed partial class reportPatientVisitsAdmin : Page
    {
        public StatisticMedicalExaminationViewModel ViewModel { get; set; }  
        public reportPatientVisitsAdmin()
        {
            this.InitializeComponent();
            ViewModel = new StatisticMedicalExaminationViewModel();
            DataContext = ViewModel;

        }
		/// <summary>
		/// Hàm xử lý sự kiện khi nhấn vào nút xem thống kê
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void viewStatistic(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
            ViewModel.UpdateChart();
        }
    }
}
