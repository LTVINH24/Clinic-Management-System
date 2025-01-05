using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    /// <summary>
    /// ViewModel cho BillDetail
    /// </summary>
    public class BillDetailViewModel : BaseViewModel
    {
        private readonly IDao _dao;
        private Bill _bill;

        public BillDetailViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }

        public Bill Bill
        {
            get => _bill;
            set => SetProperty(ref _bill, value);
        }

        public void LoadData(int billId)
        {
            Bill = _dao.GetBillById(billId);
        }
    }
} 