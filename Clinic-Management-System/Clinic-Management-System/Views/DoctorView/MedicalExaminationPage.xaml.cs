using Clinic_Management_System.Model;
using Clinic_Management_System.ViewModel;
using Clinic_Management_System.Views.DoctorView;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Clinic_Management_System.Views
{
    public sealed partial class MedicalExaminationPage : Page
    {
        public MedicalExaminationPage()
        {
            this.InitializeComponent();
            this.DataContext = new MedicalExaminationViewModel(); // Gán DataContext tại đây
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Frame navigationFrame)
            {
                ((MedicalExaminationViewModel)this.DataContext).NavigationFrame = navigationFrame;
            }
            else
            {
                throw new InvalidOperationException("Không thể lấy Frame từ tham số điều hướng.");
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is MedicalExaminationForm selectedForm)
            {
                // Sử dụng NavigationFrame của ViewModel để điều hướng
                ((MedicalExaminationViewModel)this.DataContext).NavigateToDiagnosisPage(selectedForm);
            }
        }
    }
}
