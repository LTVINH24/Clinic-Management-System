using ClinicManagementSystem.ViewModel;
using ClinicManagementSystem.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using System.Collections.ObjectModel;
using System;
using ClinicManagementSystem.Service.DataAccess;

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

            if (e.Parameter is ObservableCollection<MedicineSelection> selectedMedicines)
            {
                foreach (var medicine in selectedMedicines)
                {
                    var availableMedicine = ViewModel.AvailableMedicines.FirstOrDefault(m => m.Medicine.Id == medicine.Medicine.Id);
                    if (availableMedicine != null)
                    {
                        availableMedicine.IsSelected = true;
                        availableMedicine.SelectedQuantity = medicine.SelectedQuantity;
                        availableMedicine.SelectedDosage = medicine.SelectedDosage;
                    }
                }
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
            var selectedMedicines = ViewModel.AvailableMedicines.Where(m => m.IsSelected).ToList();
            int totalAmount = 0;

            foreach (var medicineSelection in selectedMedicines)
            {
                if (medicineSelection.SelectedQuantity <= 0)
                {
                    // Notify the user that the selected quantity is invalid
                    ShowMessage("Selected quantity must be greater than zero.");
                    return;
                }

                if (medicineSelection.SelectedDosage <= 0)
                {
                    // Notify the user that the selected quantity is invalid
                    ShowMessage("Selected dosage must be greater than zero.");
                    return;
                }

                if (medicineSelection.SelectedQuantity > medicineSelection.Medicine.Quantity)
                {
                    // Notify the user that the selected quantity exceeds the available quantity
                    ShowMessage($"Selected quantity for {medicineSelection.Medicine.Name} exceeds available quantity.");
                    return;
                }
                
                if (medicineSelection.SelectedDosage > medicineSelection.SelectedQuantity)
                {
                    // Notify the user that the selected quantity exceeds the available quantity
                    ShowMessage($"Selected dosage for {medicineSelection.Medicine.Name} must not greater than selected quantity.");
                    return;
                }

                if (!ViewModel.SelectedMedicines.Any(m => m.Medicine.Id == medicineSelection.Medicine.Id))
                {
                    ViewModel.SelectedMedicines.Add(medicineSelection);
                }

                // Calculate the total amount
                totalAmount += medicineSelection.Medicine.Price * medicineSelection.SelectedQuantity;
            }

            // Update the medicine quantities
            var dao = new SqlServerDao();
            dao.UpdateMedicineQuantities(selectedMedicines);

            //// Create a new prescription
            //var prescription = new Prescription
            //{
            //    Time = DateTime.Now,
            //    MedicineId = selectedMedicines.First().Medicine.Id,
            //    Quantity = selectedMedicines.First().SelectedQuantity,
            //    Dosage = selectedMedicines.First().SelectedDosage,
            //    MedicalExaminationFormId = GetCurrentMedicalRecordId() // Assuming this is the same as MedicalRecordId
            //};

            //// Save the prescription in the database
            //dao.SavePrescription(prescription);

            //// Insert the bill in the database
            //int prescriptionId = GetCurrentPrescriptionId();
            //dao.InsertBill(prescriptionId, totalAmount);

            // Trigger the event to send selected medicines back to DiagnosisPage
            MedicineSelectionConfirmed?.Invoke(this, new ObservableCollection<MedicineSelection>(ViewModel.SelectedMedicines));

            // Navigate back to DiagnosisPage
            Frame.GoBack();
        }
		/// <summary>
		/// Hàm hiển thị thông báo
		/// </summary>
		/// <param name="message"></param>
		private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Invalid Selection",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
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
    }
}
