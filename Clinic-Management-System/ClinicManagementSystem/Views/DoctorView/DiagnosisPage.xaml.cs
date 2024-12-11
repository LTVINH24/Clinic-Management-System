using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class DiagnosisPage : Page
    {
        public DiagnosisPage()
        {
            this.InitializeComponent();
            this.DataContext = new DiagnosisViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MedicalExaminationForm selectedForm)
            {
                ((DiagnosisViewModel)this.DataContext).LoadData(selectedForm.Id);
            }
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Điều hướng trở về DoctorPage
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void SelectMedicinesButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Navigate to the medicine selection page
            var medicineSelectionPage = new MedicineSelectionPage();
            medicineSelectionPage.MedicineSelectionConfirmed += OnMedicineSelectionConfirmed;
            Frame.Navigate(typeof(MedicineSelectionPage), null, null);
        }

        private void OnMedicineSelectionConfirmed(object sender, ObservableCollection<MedicineSelection> selectedMedicines)
        {
            var viewModel = (DiagnosisViewModel)this.DataContext;
            viewModel.UpdateSelectedMedicines(selectedMedicines);
        }
    }
}
