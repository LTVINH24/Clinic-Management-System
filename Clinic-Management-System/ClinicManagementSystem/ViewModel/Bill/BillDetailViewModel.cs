using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    public class BillDetailViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private Bill _bill;

        public BillDetailViewModel()
        {
            _dataAccess = new SqlServerDao();
        }

        public Bill Bill
        {
            get => _bill;
            set => SetProperty(ref _bill, value);
        }

        public void LoadData(int billId)
        {
            Bill = _dataAccess.GetBillById(billId);
        }
    }
} 