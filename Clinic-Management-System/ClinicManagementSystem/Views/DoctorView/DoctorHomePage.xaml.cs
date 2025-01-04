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
using ClinicManagementSystem.ViewModel.EndUser;
using ClinicManagementSystem.Views.StaffView;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClinicManagementSystem.Views.DoctorView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorHomePage : Page
    {
        public DoctorHomePageViewModel ViewModel { get; set; }
        private Frame _navigationFrame;
        
        public DoctorHomePage()
        {
            ViewModel = new DoctorHomePageViewModel();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Frame navigationFrame)
            {
                _navigationFrame = navigationFrame;
            }
            else
            {
                throw new InvalidOperationException("Không thể lấy Frame từ tham số điều hướng.");
            }
        }

        private void NavigateToListMedicalExaminationForm(object sender, RoutedEventArgs e)
        {
            _navigationFrame.Navigate(typeof(MedicalExaminationPage), _navigationFrame);
        }
        
        private void NavigateToExaminedForm(object sender, RoutedEventArgs e)
        {
            _navigationFrame.Navigate(typeof(ExaminedFormsPage), _navigationFrame);
        }

        private void NavigateToAddMedicalExaminationForm(object sender, RoutedEventArgs e)
        {

        }
    }
}
