using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using System.Collections.ObjectModel;

namespace ClinicManagementSystem.ViewModel
{
    /// <summary>
    /// ViewModel cho ExaminedFormDetail
    /// </summary>
    public class ExaminedFormDetailViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private MedicalExaminationForm _form;
        private Patient _patient;
        private string _diagnosis;
        private string _nextExaminationDate;
        private ObservableCollection<MedicineSelection> _medicines;

        public ExaminedFormDetailViewModel()
        {
            _dataAccess = new SqlServerDao();
            Medicines = new ObservableCollection<MedicineSelection>();
        }

        public MedicalExaminationForm Form
        {
            get => _form;
            set => SetProperty(ref _form, value);
        }

        public Patient Patient
        {
            get => _patient;
            set => SetProperty(ref _patient, value);
        }

        public string Diagnosis
        {
            get => _diagnosis;
            set => SetProperty(ref _diagnosis, value);
        }

        public string NextExaminationDate
        {
            get => _nextExaminationDate;
            set => SetProperty(ref _nextExaminationDate, value);
        }

        public ObservableCollection<MedicineSelection> Medicines
        {
            get => _medicines;
            set => SetProperty(ref _medicines, value);
        }

        public void LoadData(MedicalExaminationForm form)
        {
            Form = form;
            
            if (form != null)
            {
                Patient = _dataAccess.GetPatientById(form.PatientId);
                
                var medicalRecord = _dataAccess.GetMedicalRecordByExaminationFormId(form.Id);
                if (medicalRecord != null)
                {
                    Diagnosis = medicalRecord.Diagnosis;
                }

                var prescription = _dataAccess.GetPrescriptionByFormId(form.Id);
                if (prescription != null)
                {
                    NextExaminationDate = prescription.NextExaminationDate.HasValue 
                        ? prescription.NextExaminationDate.Value.ToString("dd/MM/yyyy")
                        : null;
                }

                var formMedicines = _dataAccess.GetMedicineSelectionsByFormId(form.Id);
                Medicines = new ObservableCollection<MedicineSelection>(formMedicines);
            }
        }
    }
} 