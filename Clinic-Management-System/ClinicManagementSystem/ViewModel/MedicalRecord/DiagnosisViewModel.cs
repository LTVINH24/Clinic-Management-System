using ClinicManagementSystem.Command;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using ClinicManagementSystem.Helper;

namespace ClinicManagementSystem.ViewModel
{
    public class DiagnosisViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private ObservableCollection<MedicineSelection> _selectedMedicines;
        private decimal _totalAmount;
        private bool _canSave;
        private bool _isSaved;
        private DateTimeOffset? _nextExaminationDate;
        private static Dictionary<int, DateTimeOffset?> _nextExaminationDateByFormId 
            = new Dictionary<int, DateTimeOffset?>();

        public MedicalExaminationForm MedicalExaminationForm { get; private set; }
        public MedicalRecord MedicalRecord { get; private set; }
        public Patient Patient { get; private set; }
        public Frame NavigationFrame { get; set; }

        public string Diagnosis
        {
            get => MedicalRecord?.Diagnosis;
            set
            {
                if (MedicalRecord != null)
                {
                    MedicalRecord.Diagnosis = value;
                    OnPropertyChanged();
                    System.Diagnostics.Debug.WriteLine($"Diagnosis changed to: {value}");
                    ValidateSaveButton();
                }
            }
        }

        public ObservableCollection<MedicineSelection> SelectedMedicines
        {
            get => _selectedMedicines;
            set
            {
                if (SetProperty(ref _selectedMedicines, value))
                {
                    if (_selectedMedicines != null)
                    {
                        _selectedMedicines.CollectionChanged += (s, e) =>
                        {
                            ValidateSaveButton();
                            CalculateTotalAmount();
                        };
                    }
                }
            }
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

        public bool CanSave
        {
            get => _canSave && !IsSaved;
            set
            {
                if (SetProperty(ref _canSave, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                if (SetProperty(ref _isSaved, value))
                {
                    // Thông báo UI cập nhật trạng thái của tất cả các controls
                    OnPropertyChanged(nameof(CanEditDiagnosis));
                    OnPropertyChanged(nameof(CanSelectMedicines));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public bool CanEditDiagnosis => !IsSaved;
        public bool CanSelectMedicines => !IsSaved;

        public DateTimeOffset? NextExaminationDate
        {
            get => _nextExaminationDate;
            set
            {
                if (SetProperty(ref _nextExaminationDate, value))
                {
                    // Lưu vào Dictionary khi có thay đổi
                    if (MedicalExaminationForm != null)
                    {
                        _nextExaminationDateByFormId[MedicalExaminationForm.Id] = value;
                    }
                }
            }
        }

        public DateTimeOffset MinNextExaminationDate => DateTimeOffset.Now;

        public DiagnosisViewModel()
        {
            _dataAccess = new SqlServerDao();
            SelectedMedicines = new ObservableCollection<MedicineSelection>();
            CanSave = false;
            IsSaved = false;
        }

		/// <summary>
		/// Load dữ liệu từ cơ sở dữ liệu dựa vào Id của phiếu khám bệnh
		/// </summary>
		/// <param name="formId"></param>
		public void LoadData(int formId)
        {
            MedicalExaminationForm = _dataAccess.GetMedicalExaminationFormById(formId);
            if (MedicalExaminationForm != null)
            {
                MedicalRecord = _dataAccess.GetMedicalRecordByExaminationFormId(formId);

                // Chỉ load ngày tái khám từ Dictionary
                if (_nextExaminationDateByFormId.ContainsKey(formId))
                {
                    NextExaminationDate = _nextExaminationDateByFormId[formId];
                }
                else
                {
                    // Nếu chưa có trong Dictionary thì set null
                    NextExaminationDate = null;
                }
            }

            // Kiểm tra trạng thái đã khám
            if (MedicalExaminationForm?.IsExaminated == "true")
            {
                IsSaved = true; // Điều này sẽ kích hoạt việc khóa các controls
            }
            else
            {
                IsSaved = false; // Cho phép chỉnh sửa nếu chưa khám
            }

            // Load Patient data
            if (MedicalExaminationForm != null)
            {
                Patient = _dataAccess.GetPatientById(MedicalExaminationForm.PatientId);
            }

            // Try to load corresponding MedicalRecord
            MedicalRecord = _dataAccess.GetMedicalRecordByExaminationFormId(formId);

            // If MedicalRecord doesn't exist, create it using MedicalExaminationForm details
            if (MedicalRecord == null && MedicalExaminationForm != null)
            {
                MedicalRecord = _dataAccess.CreateMedicalRecordFromForm(MedicalExaminationForm);
            }

            // Load medicines for this specific form
            SelectedMedicines.Clear();
            var formMedicines = MedicineSelectionViewModel.GetMedicineSelectionsForForm(formId);
            foreach (var medicine in formMedicines)
            {
                SelectedMedicines.Add(medicine);
            }
            
            // Tính tổng tiền sau khi load danh sách thuốc
            CalculateTotalAmount();
            
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

        private void ValidateSaveButton()
        {
            bool canSave = !string.IsNullOrWhiteSpace(MedicalRecord?.Diagnosis);
            System.Diagnostics.Debug.WriteLine($"ValidateSaveButton called: {canSave}");
            CanSave = canSave;
        }

        public void OnDiagnosisChanged()
        {
            ValidateSaveButton();
        }

        public void OnMedicinesChanged()
        {
            ValidateSaveButton();
        }

        public void LoadExaminationForm(MedicalExaminationForm form)
        {
            MedicalExaminationForm = form;
            
            // Kiểm tra trạng thái đã khám
            if (form.IsExaminated?.ToLower() == "true")
            {
                IsSaved = true; // Điều này sẽ tự động khóa các controls thông qua binding
            }
            
            // Load dữ liệu
            LoadData(form.Id);
        }

        public bool SaveDiagnosisAndPrescription()
        {
            // 1. Lưu chẩn đoán
            SaveDiagnosis();

            // 2. Lưu đơn thuốc và ngày tái khám
            bool prescriptionSaved = _dataAccess.SavePrescription(
                MedicalExaminationForm.Id,
                SelectedMedicines.ToList(),
                NextExaminationDate  // Thêm tham số ngày tái khám
            );

            if (!prescriptionSaved)
            {
                throw new Exception("Không thể lưu đơn thuốc!");
            }

            // 3. Cập nhật trạng thái đã lưu
            IsSaved = true;

            // 4. Xóa dữ liệu tạm trong Dictionary
            ClearNextExaminationDate(MedicalExaminationForm.Id);

            return true;
        }

        // Thêm phương thức để xóa dữ liệu khi không cần thiết nữa
        public static void ClearNextExaminationDate(int formId)
        {
            if (_nextExaminationDateByFormId.ContainsKey(formId))
            {
                _nextExaminationDateByFormId.Remove(formId);
            }
        }
    }
}
