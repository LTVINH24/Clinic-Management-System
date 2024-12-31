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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminHomePage : Page
    {
       public AdminHomePageViewModel ViewModel { get; set; }
        public AdminHomePage()
        {
            this.InitializeComponent();
            ViewModel = new AdminHomePageViewModel();
        }

        private void NavigateToAddUser(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addAccount));

        }

        

        private void NavigateToReportBill(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(reportBillAdmin));
        }

        private void NavigateToListUser(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(listAccount));

        }
    }
}
