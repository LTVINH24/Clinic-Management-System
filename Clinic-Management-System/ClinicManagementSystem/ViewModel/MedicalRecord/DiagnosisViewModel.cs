using ClinicManagementSystem.Command;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ClinicManagementSystem.ViewModel
{
    public class DiagnosisViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;

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

        public ObservableCollection<MedicineSelection> SelectedMedicines { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand SaveMedicinesCommand { get; }

        public DiagnosisViewModel()
        {
            _dataAccess = new SqlServerDao();
            SelectedMedicines = new ObservableCollection<MedicineSelection>();
            SaveCommand = new RelayCommand(SaveDiagnosis);
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
		/// Thêm thuốc được chọn vào danh sách thuốc đã chọn
		/// </summary>
		/// <param name="medicineSelection"></param>
		public void AddSelectedMedicine(MedicineSelection medicineSelection)
        {
            var existingMedicine = SelectedMedicines.FirstOrDefault(m => m.Medicine.Id == medicineSelection.Medicine.Id);
            if (existingMedicine != null)
            {
                existingMedicine.SelectedQuantity = medicineSelection.SelectedQuantity;
                existingMedicine.SelectedDosage = medicineSelection.SelectedDosage;
            }
            else
            {
                SelectedMedicines.Add(medicineSelection);
            }
        }

		/// <summary>
		/// Cập nhật danh sách thuốc đã chọn
		/// </summary>
		/// <param name="selectedMedicines"></param>
		public void UpdateSelectedMedicines(ObservableCollection<MedicineSelection> selectedMedicines)
        {
            SelectedMedicines.Clear();
            foreach (var medicine in selectedMedicines)
            {
                SelectedMedicines.Add(medicine);
            }
            OnPropertyChanged(nameof(SelectedMedicines));
        }
    }
}
