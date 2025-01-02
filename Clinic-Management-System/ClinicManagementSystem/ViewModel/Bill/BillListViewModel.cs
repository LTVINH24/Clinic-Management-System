using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;

namespace ClinicManagementSystem.ViewModel
{
    public class BillListViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;
        private ObservableCollection<Bill> _bills;
        private Bill _selectedBill;

        public BillListViewModel()
        {
            _dataAccess = new SqlServerDao();
            LoadBills();
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set => SetProperty(ref _bills, value);
        }

        public Bill SelectedBill
        {
            get => _selectedBill;
            set => SetProperty(ref _selectedBill, value);
        }

        private void LoadBills()
        {
            var billsList = _dataAccess.GetAllBills();
            Bills = new ObservableCollection<Bill>(billsList);
        }
    }
} 