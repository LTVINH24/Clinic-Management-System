using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;

namespace ClinicManagementSystem.ViewModel
{
    public class UnbilledPrescriptionsDetailViewModel : BaseViewModel
    {
        private readonly IDao _dao;
        private Prescription _prescription;
        private Patient _patient;
        private MedicalExaminationForm _form;
        private ObservableCollection<MedicineSelection> _medicines;
        private int _totalAmount;
        private bool _isGetMedicine;
        private int _examinationFee;
        private int _finalTotal;
        private bool _isExaminationFeeLocked;
        private bool _canSave;

        public UnbilledPrescriptionsDetailViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Medicines = new ObservableCollection<MedicineSelection>();
            IsGetMedicine = false;
            _isExaminationFeeLocked = false;
            _canSave = false;
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
                if (!IsExaminationFeeLocked && SetProperty(ref _examinationFee, value))
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

        public bool IsExaminationFeeLocked
        {
            get => _isExaminationFeeLocked;
            set
            {
                if (SetProperty(ref _isExaminationFeeLocked, value))
                {
                    OnPropertyChanged(nameof(IsExaminationFeeEditable));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public bool IsExaminationFeeEditable => !IsExaminationFeeLocked;

        public bool CanSave
        {
            get => IsExaminationFeeLocked;
        }

        public void LoadData(int prescriptionId)
        {
            // Load prescription
            Prescription = _dao.GetPrescriptionById(prescriptionId);

            if (Prescription != null)
            {
                // Load form
                Form = _dao.GetMedicalExaminationFormById(Prescription.MedicalExaminationFormId);

                if (Form != null)
                {
                    // Load patient
                    Patient = _dao.GetPatientById(Form.PatientId);

                    // Load medicines
                    var formMedicines = _dao.GetMedicineSelectionsByFormId(Form.Id);
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
                // 1. Tạo và lưu bill
                var bill = new Bill
                {
                    PrescriptionId = Prescription.Id,
                    TotalAmount = FinalTotal, // Dùng FinalTotal thay vì TotalAmount
                    CreatedDate = DateTime.Now,
                    IsGetMedicine = IsGetMedicine ? "true" : "false"
                };

                bool success = _dao.SaveBill(bill);
                if (!success) return false;

                // 2. Cập nhật số lượng thuốc trong kho nếu có lấy thuốc
                if (IsGetMedicine)
                {
                    foreach (var medicine in Medicines)
                    {
                        try
                        {
                            _dao.UpdateMedicineQuantity(
                                medicine.Medicine.Id, 
                                -medicine.SelectedQuantity
                            );
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }

                // 3. Cập nhật trạng thái đơn thuốc
                success = _dao.UpdatePrescriptionBillStatus(Prescription.Id, "true");
                return success;
            }
            catch
            {
                return false;
            }
        }
    }
} 