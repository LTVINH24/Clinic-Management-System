using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class ExaminedFormsPage : Page
    {
        public ExaminedFormsViewModel ViewModel { get; }

        public ExaminedFormsPage()
        {
            this.InitializeComponent();
            ViewModel = new ExaminedFormsViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedForm != null)
            {
                Frame.Navigate(typeof(ExaminedFormDetailPage));
                var a = test.SelectedItem as MedicalExaminationForm;
                ViewModel.SelectedForm = a;

            }
        }
    }
} 