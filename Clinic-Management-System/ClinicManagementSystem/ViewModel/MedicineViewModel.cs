using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using static ClinicManagementSystem.Service.DataAccess.IDao;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using Windows.System;
namespace ClinicManagementSystem.ViewModel
{
    public class MedicineViewModel : INotifyPropertyChanged
    { 
        IDao _dao;
        private ObservableCollection<Medicine> _medicines;
        public ObservableCollection<Medicine> Medicines
        {
            get=> _medicines ??= new ObservableCollection<Medicine>();
            set => _medicines = value;
        }
        public Medicine MedicineEdit { get; set; } = new Medicine();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; } = 0;
        public int RowsPerPage { get; set; }
        public string Keyword { get; set; } = "";
        private Dictionary<string, SortType> _sortOptions = new();
        public MedicineViewModel()
        {
            RowsPerPage = 5;
            CurrentPage = 1;
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            LoadMedicines();
        }
        public PageInfo SelectedPageInfoItem { get; set; } = new PageInfo();
        private ObservableCollection<PageInfo> _pageinfos;
        public ObservableCollection<PageInfo> PageInfos
        {
            get => _pageinfos ??= new ObservableCollection<PageInfo>();
            set
            {
                _pageinfos = value;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _sortById = false;
        public bool SortById
        {
            get => _sortById;
            set
            {
                _sortById = value;
                if (value == true)
                {
                    _sortOptions.Add("Name", SortType.Ascending);
                }
                else
                {
                    if (_sortOptions.ContainsKey("Name"))
                    {
                        _sortOptions.Remove("Name");
                    }
                }
                LoadMedicines();
            }
        }
        private void LoadMedicines()
        {
            var (items, count) = _dao.GetMedicines(
                CurrentPage, RowsPerPage, Keyword,
                _sortOptions
            );
            Medicines.Clear();
            foreach (var medicine in items)
            {
                Medicines.Add(medicine);
            }
            if (count != TotalItems)
            {
                TotalItems = count;
                TotalPages = TotalItems / RowsPerPage +
                    (TotalItems % RowsPerPage == 0 ? 0 : 1);
            }
            PageInfos.Clear();
            for (int i = CurrentPage; i <= Math.Min(CurrentPage + 2, TotalPages); i++)
            {
                PageInfos.Add(new PageInfo
                {
                    Page = i,
                    Total = TotalPages
                });
            }

            SelectedPageInfoItem = new PageInfo { Page = CurrentPage, Total = TotalPages };

        }
        public void Search()
        {
            CurrentPage = 1;
            LoadMedicines();
        }
        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadMedicines();
            }
        }
        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadMedicines();
            }
        }
        public void GoToPage(int page)
        {
            CurrentPage = page;
            LoadMedicines();
        }
        public bool UpdateMedicine()
        {
            bool success = _dao.UpdateMedicine(MedicineEdit);
            LoadMedicines();
            return success;

        }
        public bool AddMedicine()
        {
             bool success= _dao.CreateMedicine(MedicineEdit);
            LoadMedicines();
            return success;

        }
        public void EditMedicine(Medicine medicine)
        {
            MedicineEdit = medicine;
        }
        public bool DeleteMedicine(Medicine newMedicine)
        {
            bool success =_dao.DeleteMedicine(newMedicine);
            LoadMedicines();
            return success;
        }
        public void CancelMedicine()
        {
            MedicineEdit = new Medicine();
        }
    }
}
