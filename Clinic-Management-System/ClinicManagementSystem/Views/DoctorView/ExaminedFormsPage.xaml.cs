using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.ViewModel;

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
                Frame.Navigate(typeof(ExaminedFormDetailPage), ViewModel.SelectedForm);
            }
        }
    }
} 