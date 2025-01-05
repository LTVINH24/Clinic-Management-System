using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System;
using ClinicManagementSystem.Helper;

namespace ClinicManagementSystem.Views.DoctorView
{
    /// <summary>
    /// DiagnosisPage là trang chẩn đoán
    /// </summary>
    public sealed partial class DiagnosisPage : Page
    {
        public DiagnosisViewModel ViewModel { get; }

        public DiagnosisPage()
        {
            this.InitializeComponent();
            ViewModel = new DiagnosisViewModel();
            ViewModel.NavigationFrame = this.Frame;
            this.DataContext = ViewModel;
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
		/// Xử lý sự kiện khi nhấn nút Save
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lưu chẩn đoán và đơn thuốc
                bool success = ViewModel.SaveDiagnosisAndPrescription();
                
                if (success)
                {
                    // Quay về trang trước
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                _ = DialogHelper.ShowMessage("Lỗi", ex.Message, this.Content.XamlRoot);
            }
        }

		/// <summary>
		/// Xử lý sự kiện khi nhấn nút Back
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Chỉ quay về trang trước
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
            // Lưu chẩn đoán
            ViewModel.SaveDiagnosis();

            // Truyền formId khi navigate
            var medicineSelectionPage = new MedicineSelectionPage();
            medicineSelectionPage.MedicineSelectionConfirmed += OnMedicineSelectionConfirmed;
            
            // Truyền formId qua parameter
            Frame.Navigate(typeof(MedicineSelectionPage), ViewModel.MedicalExaminationForm.Id);
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                ViewModel.Diagnosis = textBox.Text;
            }
        }
    }
}
