using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    public class UnbilledPrescriptionsDetailViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private Prescription _prescription;
        private Patient _patient;
        private MedicalExaminationForm _form;
        private ObservableCollection<MedicineSelection> _medicines;
        private int _totalAmount;
        private bool _isGetMedicine;
        private int _examinationFee;
        private int _finalTotal;

        public UnbilledPrescriptionsDetailViewModel()
        {
            _dataAccess = new SqlServerDao();
            Medicines = new ObservableCollection<MedicineSelection>();
            IsGetMedicine = true;  // Mặc định là có lấy thuốc
        }

        public Prescription Prescription
        {
            get => _prescription;
            set => SetProperty(ref _prescription, value);
        }

        public Patient Patient
        {
            get => _patient;
            set => SetProperty(ref _patient, value);
        }

        public MedicalExaminationForm Form
        {
            get => _form;
            set => SetProperty(ref _form, value);
        }

        public ObservableCollection<MedicineSelection> Medicines
        {
            get => _medicines;
            set => SetProperty(ref _medicines, value);
        }

        public int TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        public int ExaminationFee
        {
            get => _examinationFee;
            set
            {
                if (SetProperty(ref _examinationFee, value))
                {
                    CalculateFinalTotal();
                }
            }
        }

        public int FinalTotal
        {
            get => _finalTotal;
            set => SetProperty(ref _finalTotal, value);
        }

        public bool IsGetMedicine
        {
            get => _isGetMedicine;
            set
            {
                if (SetProperty(ref _isGetMedicine, value))
                {
                    CalculateFinalTotal();
                }
            }
        }

        public void LoadData(int prescriptionId)
        {
            // Load prescription
            Prescription = _dataAccess.GetPrescriptionById(prescriptionId);

            if (Prescription != null)
            {
                // Load form
                Form = _dataAccess.GetMedicalExaminationFormById(Prescription.MedicalExaminationFormId);

                if (Form != null)
                {
                    // Load patient
                    Patient = _dataAccess.GetPatientById(Form.PatientId);

                    // Load medicines
                    var formMedicines = _dataAccess.GetMedicineSelectionsByFormId(Form.Id);
                    Medicines = new ObservableCollection<MedicineSelection>(formMedicines);

                    // Calculate total amount
                    CalculateTotalAmount();
                }
            }
        }

        private void CalculateTotalAmount()
        {
            int total = 0;
            foreach (var medicine in Medicines)
            {
                total += (int)(medicine.Medicine.Price * medicine.SelectedQuantity);
            }
            TotalAmount = total;
        }

        private void CalculateFinalTotal()
        {
            FinalTotal = ExaminationFee;
            if (IsGetMedicine)
            {
                FinalTotal += TotalAmount;
            }
        }

        public bool SaveBill()
        {
            try
            {
                var bill = new Bill
                {
                    PrescriptionId = Prescription.Id,
                    TotalAmount = TotalAmount,
                    CreatedDate = DateTime.Now,
                    IsGetMedicine = IsGetMedicine ? "true" : "false"
                };

                bool success = _dataAccess.SaveBill(bill);

                if (success)
                {
                    success = _dataAccess.UpdatePrescriptionBillStatus(Prescription.Id, true);
                }

                return success;
            }
            catch
            {
                return false;
            }
        }
    }
} 