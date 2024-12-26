using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class ExaminedFormDetailPage : Page
    {
        public ExaminedFormDetailViewModel ViewModel { get; }

        public ExaminedFormDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new ExaminedFormDetailViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MedicalExaminationForm form)
            {
                ViewModel.LoadData(form);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
} 