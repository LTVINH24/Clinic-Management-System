using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class DiagnosisPage : Page
    {
        private DiagnosisViewModel ViewModel => (DiagnosisViewModel)DataContext;

        public DiagnosisPage()
        {
            this.InitializeComponent();
            var viewModel = new DiagnosisViewModel();
            viewModel.RequestNavigateToMedicineSelection += OnRequestNavigateToMedicineSelection;
            this.DataContext = viewModel;
        }

		/// <summary>
		/// Load dữ liệu khi điều hướng trang
		/// </summary>
		/// <param name="e"></param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MedicalExaminationForm selectedForm)
            {
                ViewModel.LoadData(selectedForm.Id);
            }
        }

		/// <summary>
		/// Xử lý sự kiện khi nhấn nút Back
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

		/// <summary>
		/// Xử lí sự kiện khi nhấn nút chọn thuốc
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SelectMedicinesButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Navigate to the medicine selection page
            var medicineSelectionPage = new MedicineSelectionPage();
            medicineSelectionPage.MedicineSelectionConfirmed += OnMedicineSelectionConfirmed;
            Frame.Navigate(typeof(MedicineSelectionPage), null, null);
        }

		/// <summary>
		/// Xử lý sự kiện khi xác nhận chọn thuốc
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="selectedMedicines"></param>
		private void OnMedicineSelectionConfirmed(object sender, ObservableCollection<MedicineSelection> selectedMedicines)
        {
            ViewModel.UpdateSelectedMedicines(selectedMedicines);
        }

        private void OnRequestNavigateToMedicineSelection(object sender, ObservableCollection<MedicineSelection> selectedMedicines)
        {
            var medicineSelectionPage = new MedicineSelectionPage();
            medicineSelectionPage.MedicineSelectionConfirmed += OnMedicineSelectionConfirmed;
            Frame.Navigate(typeof(MedicineSelectionPage), ViewModel.SelectedMedicines);
        }
    }
}


