using Clinic_Management_System.Command;
using Clinic_Management_System.Model;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Windows.Input;

namespace Clinic_Management_System.ViewModel
{
    public class DiagnosisViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;

        public MedicalExaminationForm MedicalExaminationForm { get; private set; }
        public MedicalRecord MedicalRecord { get; private set; }

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

        public ICommand SaveCommand { get; }

        public DiagnosisViewModel()
        {
            _dataAccess = new SqlServerDao();
            SaveCommand = new RelayCommand(SaveDiagnosis);
        }

        public void LoadData(int medicalExaminationFormId)
        {
            // Load MedicalExaminationForm data
            MedicalExaminationForm = _dataAccess.GetMedicalExaminationFormById(medicalExaminationFormId);

            // Try to load corresponding MedicalRecord
            MedicalRecord = _dataAccess.GetMedicalRecordByExaminationFormId(medicalExaminationFormId);

            // If MedicalRecord doesn't exist, create it using MedicalExaminationForm details
            if (MedicalRecord == null && MedicalExaminationForm != null)
            {
                MedicalRecord = _dataAccess.CreateMedicalRecordFromForm(MedicalExaminationForm);
            }
        }

        private void SaveDiagnosis()
        {
            if (MedicalRecord != null)
            {
                // Save or update the current MedicalRecord
                _dataAccess.UpdateMedicalRecord(MedicalRecord);
            }
        }
    }
}
