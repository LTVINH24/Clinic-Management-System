using ClinicManagementSystem.Command;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ClinicManagementSystem.ViewModel
{
    public class DiagnosisViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private ObservableCollection<MedicineSelection> _selectedMedicines;
        private decimal _totalAmount;

        public MedicalExaminationForm MedicalExaminationForm { get; private set; }
        public MedicalRecord MedicalRecord { get; private set; }
        public Patient Patient { get; private set; }

        public string Diagnosis
        {
            get => MedicalRecord?.Diagnosis;
            set
            {
                if (MedicalRecord != null)
                {
                    MedicalRecord.Diagnosis = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MedicineSelection> SelectedMedicines
        {
            get => _selectedMedicines;
            set => SetProperty(ref _selectedMedicines, value);
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            private set
            {
                if (SetProperty(ref _totalAmount, value))
                {
                    OnPropertyChanged(nameof(FormattedTotalAmount));
                }
            }
        }

        public string FormattedTotalAmount => $"{TotalAmount:N0} VND";

        public ICommand SaveCommand { get; }
        public ICommand SaveMedicinesCommand { get; }
        public ICommand NavigateToMedicineSelectionCommand { get; }
        public event EventHandler<ObservableCollection<MedicineSelection>> RequestNavigateToMedicineSelection;

        public DiagnosisViewModel()
        {
            _dataAccess = new SqlServerDao();
            SelectedMedicines = new ObservableCollection<MedicineSelection>();
            SaveCommand = new RelayCommand(SaveDiagnosis);
            NavigateToMedicineSelectionCommand = new RelayCommand(NavigateToMedicineSelection);
        }

		/// <summary>
		/// Load dữ liệu từ cơ sở dữ liệu dựa vào Id của phiếu khám bệnh
		/// </summary>
		/// <param name="medicalExaminationFormId"></param>
		public void LoadData(int medicalExaminationFormId)
        {
            // Load MedicalExaminationForm data
            MedicalExaminationForm = _dataAccess.GetMedicalExaminationFormById(medicalExaminationFormId);

            // Load Patient data
            if (MedicalExaminationForm != null)
            {
                Patient = _dataAccess.GetPatientById(MedicalExaminationForm.PatientId);
            }

            // Try to load corresponding MedicalRecord
            MedicalRecord = _dataAccess.GetMedicalRecordByExaminationFormId(medicalExaminationFormId);

            // If MedicalRecord doesn't exist, create it using MedicalExaminationForm details
            if (MedicalRecord == null && MedicalExaminationForm != null)
            {
                MedicalRecord = _dataAccess.CreateMedicalRecordFromForm(MedicalExaminationForm);
            }

            // Load medicines from static storage
            System.Diagnostics.Debug.WriteLine($"Before loading: LastSelectedMedicines has {MedicineSelectionViewModel.LastSelectedMedicines.Count} items");
            
            SelectedMedicines.Clear();
            foreach (var medicine in MedicineSelectionViewModel.LastSelectedMedicines)
            {
                SelectedMedicines.Add(medicine);
                System.Diagnostics.Debug.WriteLine($"Added medicine: {medicine.Medicine.Name}");
            }
            
            // Tính tổng tiền sau khi load danh sách thuốc
            CalculateTotalAmount();
            
            System.Diagnostics.Debug.WriteLine($"After loading: SelectedMedicines has {SelectedMedicines.Count} items");
            System.Diagnostics.Debug.WriteLine($"Total amount: {FormattedTotalAmount}");
            
            OnPropertyChanged(nameof(SelectedMedicines));
        }

		/// <summary>
		/// Lưu thông tin chẩn đoán 
		/// </summary>
		private void SaveDiagnosis()
        {
            if (MedicalRecord != null)
            {
                // Save or update the current MedicalRecord
                _dataAccess.UpdateMedicalRecord(MedicalRecord);
            }
        }

		/// <summary>
		/// Cập nhật danh sách thuốc được chọn từ MedicineSelectionPage
		/// </summary>
		/// <param name="medicines"></param>
		public void UpdateSelectedMedicines(ObservableCollection<MedicineSelection> medicines)
        {
            if (medicines == null) return;

            System.Diagnostics.Debug.WriteLine($"UpdateSelectedMedicines called with {medicines.Count} medicines");
            
            SelectedMedicines.Clear();
            foreach (var medicine in medicines)
            {
                SelectedMedicines.Add(medicine);
                System.Diagnostics.Debug.WriteLine($"Added medicine: {medicine.Medicine.Name} - Quantity: {medicine.SelectedQuantity} - Price: {medicine.Medicine.Price}");
            }

            // Cập nhật LastSelectedMedicines
            MedicineSelectionViewModel.LastSelectedMedicines.Clear();
            foreach (var medicine in medicines)
            {
                MedicineSelectionViewModel.LastSelectedMedicines.Add(medicine);
            }
            
            // Tính tổng tiền sau khi cập nhật danh sách thuốc
            CalculateTotalAmount();
            
            System.Diagnostics.Debug.WriteLine($"Updated LastSelectedMedicines, now has {MedicineSelectionViewModel.LastSelectedMedicines.Count} items");
            OnPropertyChanged(nameof(SelectedMedicines));
        }

        private void NavigateToMedicineSelection()
        {
            RequestNavigateToMedicineSelection?.Invoke(this, SelectedMedicines);
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = SelectedMedicines.Sum(m => m.Medicine.Price * m.SelectedQuantity);
            System.Diagnostics.Debug.WriteLine($"CalculateTotalAmount called. Total: {FormattedTotalAmount}");
        }
    }
}
