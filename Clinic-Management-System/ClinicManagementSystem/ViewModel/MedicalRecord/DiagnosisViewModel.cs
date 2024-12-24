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

            // Load medicines for this specific form
            SelectedMedicines.Clear();
            var formMedicines = MedicineSelectionViewModel.GetMedicineSelectionsForForm(medicalExaminationFormId);
            foreach (var medicine in formMedicines)
            {
                SelectedMedicines.Add(medicine);
            }
            
            System.Diagnostics.Debug.WriteLine($"Loaded {SelectedMedicines.Count} medicines for form {medicalExaminationFormId}");
            
            // Tính tổng tiền sau khi load danh sách thuốc
            CalculateTotalAmount();
            
            System.Diagnostics.Debug.WriteLine($"After loading: SelectedMedicines has {SelectedMedicines.Count} items");
            System.Diagnostics.Debug.WriteLine($"Total amount: {FormattedTotalAmount}");
            
            OnPropertyChanged(nameof(SelectedMedicines));
        }

		/// <summary>
		/// Lưu thông tin chẩn đoán 
		/// </summary>
		public void SaveDiagnosis()
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
            if (medicines == null || MedicalExaminationForm == null) return;

            // Lấy danh sách thuốc theo formId
            var formMedicines = MedicineSelectionViewModel.GetMedicineSelectionsForForm(MedicalExaminationForm.Id);
            
            SelectedMedicines.Clear();
            foreach (var medicine in formMedicines)
            {
                SelectedMedicines.Add(medicine);
                System.Diagnostics.Debug.WriteLine($"Added medicine: {medicine.Medicine.Name}");
            }
            
            CalculateTotalAmount();
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
