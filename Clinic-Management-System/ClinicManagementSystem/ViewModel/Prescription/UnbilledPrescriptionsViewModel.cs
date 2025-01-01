using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    public class UnbilledPrescriptionsViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private ObservableCollection<Prescription> _unbilledPrescriptions;
        private Prescription _selectedPrescription;

        public UnbilledPrescriptionsViewModel()
        {
            _dataAccess = new SqlServerDao();
            LoadUnbilledPrescriptions();
        }

        public ObservableCollection<Prescription> UnbilledPrescriptions
        {
            get => _unbilledPrescriptions;
            set => SetProperty(ref _unbilledPrescriptions, value);
        }

        public Prescription SelectedPrescription
        {
            get => _selectedPrescription;
            set => SetProperty(ref _selectedPrescription, value);
        }

        private void LoadUnbilledPrescriptions()
        {
            var prescriptions = _dataAccess.GetPrescriptionsByBillStatus(false);
            UnbilledPrescriptions = new ObservableCollection<Prescription>(prescriptions);
        }

        // Thêm phương thức để refresh danh sách
        public void RefreshList()
        {
            LoadUnbilledPrescriptions();
        }

        // Thêm phương thức để cập nhật trạng thái bill
        public bool UpdateBillStatus(int prescriptionId)
        {
            bool success = _dataAccess.UpdatePrescriptionBillStatus(prescriptionId, true);
            if (success)
            {
                RefreshList(); // Tải lại danh sách sau khi cập nhật
            }
            return success;
        }
    }
} 