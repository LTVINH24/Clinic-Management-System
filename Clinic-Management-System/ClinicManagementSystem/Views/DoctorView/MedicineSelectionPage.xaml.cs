using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using System.Collections.ObjectModel;
using System;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Helper;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class MedicineSelectionPage : Page
    {
        private MedicineSelectionViewModel ViewModel => (MedicineSelectionViewModel)DataContext;

        public event EventHandler<ObservableCollection<MedicineSelection>> MedicineSelectionConfirmed;

        public MedicineSelectionPage()
        {
            this.InitializeComponent();
            this.DataContext = new MedicineSelectionViewModel();
        }
		/// <summary>
		/// Hàm điều hướng trang  
		/// </summary>
		/// <param name="e"></param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (e.Parameter is int formId)
            {
                // Khởi tạo ViewModel với formId
                var viewModel = new MedicineSelectionViewModel();
                viewModel.InitializeWithFormId(formId);
                this.DataContext = viewModel;
                
                // Load dữ liệu từ lần chọn trước
                viewModel.LoadFromLastSelection();
                
                System.Diagnostics.Debug.WriteLine($"Initialized MedicineSelectionPage with formId: {formId}");
            }
        }
		/// <summary>
		/// Hàm xử lý sự kiện khi người dùng nhấn nút quay lại
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
		/// <summary>
		/// Hàm xử lý sự kiện khi người dùng nhấn nút xác nhận
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMedicines = new ObservableCollection<MedicineSelection>(
                ViewModel.AvailableMedicines.Where(m => m.IsSelected)
            );

            if (ValidateSelections(selectedMedicines))
            {
                // Lưu danh sách thuốc đã chọn
                ViewModel.SaveSelectedMedicines();
                
                // Thông báo cho DiagnosisPage
                MedicineSelectionConfirmed?.Invoke(this, selectedMedicines);
                
                System.Diagnostics.Debug.WriteLine($"Confirmed selection of {selectedMedicines.Count} medicines");
                
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private bool ValidateSelections(ObservableCollection<MedicineSelection> selectedMedicines)
        {
            //int totalAmount = 0;

            foreach (var medicineSelection in selectedMedicines)
            {
                if (medicineSelection.SelectedQuantity <= 0)
                {
                    ShowMessage("Selected quantity must be greater than zero.");
                    return false;
                }

                if (medicineSelection.SelectedQuantity > medicineSelection.Medicine.Quantity)
                {
                    // Notify the user that the selected quantity exceeds the available quantity
                    ShowMessage($"Selected quantity for {medicineSelection.Medicine.Name} exceeds available quantity.");
                    return false;
                }
            }

            return true;
        }
		/// <summary>
		/// Hàm hiển thị thông báo
		/// </summary>
		/// <param name="message"></param>
		private async void ShowMessage(string message)
        {
            await DialogHelper.ShowMessage("Invalid Selection", message, this.Content.XamlRoot);
        }

        //private int GetCurrentMedicalRecordId()
        //{
        //    var dao = new SqlServerDao();
        //    // Giả sử bạn có một phương thức trong dao để lấy hồ sơ y tế hiện tại
        //    var medicalRecord = dao.GetMedicalRecordByExaminationFormId(GetCurrentMedicalExaminationFormId());
        //    return medicalRecord?.Id ?? 0; // Trả về ID của hồ sơ y tế hoặc 0 nếu không tìm thấy
        //}

        //private int GetCurrentPrescriptionId()
        //{
        //    // Implement this method to get the current prescription ID
        //    // This is just a placeholder implementation
        //    return 1;
        //}

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToPreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextPage();
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is int page)
            {
                ViewModel.GoToPage(page);
            }
        }

        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is PageInfo pageInfo)
            {
                ViewModel.GoToPage(pageInfo.Page);
            }
        }
    }
}
