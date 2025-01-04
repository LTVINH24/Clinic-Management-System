using System.Collections.ObjectModel;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Model;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ClinicManagementSystem.ViewModel
{
    public class MedicineSelectionViewModel : BaseViewModel
    {
        private readonly IDao _dataAccess;

        private ObservableCollection<MedicineSelection> _availableMedicines;
        public ObservableCollection<MedicineSelection> AvailableMedicines
        {
            get => _availableMedicines;
            set
            {
                if (_availableMedicines != value)
                {
                    _availableMedicines = value;
                    OnPropertyChanged(nameof(AvailableMedicines));
                }
            }
        }

        private static Dictionary<int, ObservableCollection<MedicineSelection>> _medicineSelectionsByFormId 
            = new Dictionary<int, ObservableCollection<MedicineSelection>>();

        private int _currentFormId;

        private string _searchText;
        private ObservableCollection<MedicineSelection> _filteredMedicines;

        private int _currentPage = 1;
        private int _totalPages;
        private int _totalItems = 0;
        private int _pageSize = 10;

        private ObservableCollection<PageInfo> _pageInfos;
        private PageInfo _selectedPageInfo;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    LoadAvailableMedicines();
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set => SetProperty(ref _totalPages, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterMedicines();
                }
            }
        }

        public ObservableCollection<MedicineSelection> FilteredMedicines
        {
            get => _filteredMedicines;
            private set => SetProperty(ref _filteredMedicines, value);
        }

        public ObservableCollection<PageInfo> PageInfos
        {
            get => _pageInfos;
            set => SetProperty(ref _pageInfos, value);
        }

        public PageInfo SelectedPageInfo
        {
            get => _selectedPageInfo;
            set
            {
                if (SetProperty(ref _selectedPageInfo, value) && value != null)
                {
                    CurrentPage = value.Page;
                    LoadAvailableMedicines();
                }
            }
        }

        public MedicineSelectionViewModel()
        {
            _dataAccess = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            AvailableMedicines = new ObservableCollection<MedicineSelection>();
            PageInfos = new ObservableCollection<PageInfo>();
            _currentFormId = -1;
            LoadAvailableMedicines();
            FilteredMedicines = new ObservableCollection<MedicineSelection>(AvailableMedicines);
        }

		/// <summary>
		/// Load dữ liệu thuốc có sẵn từ database
		/// </summary>
		private void LoadAvailableMedicines()
        {
            var (medicines, totalCount) = _dataAccess.GetMedicinesByPage(CurrentPage, _pageSize, SearchText);
            
            AvailableMedicines.Clear();
            foreach (var medicine in medicines)
            {
                AvailableMedicines.Add(medicine);
            }

            if (totalCount != _totalItems)
            {
                _totalItems = totalCount;
                TotalPages = (_totalItems + _pageSize - 1) / _pageSize;
                UpdatePageInfos();
            }

            FilterMedicines();
        }

        private void UpdatePageInfos()
        {
            PageInfos.Clear();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageInfos.Add(new PageInfo { Page = i, Total = TotalPages });
            }
            SelectedPageInfo = PageInfos.FirstOrDefault(p => p.Page == CurrentPage);
        }

        public void InitializeWithSelectedMedicines(ObservableCollection<MedicineSelection> selectedMedicines)
        {
            if (selectedMedicines == null) return;

            foreach (var medicine in selectedMedicines)
            {
                var availableMedicine = AvailableMedicines
                    .FirstOrDefault(m => m.Medicine.Id == medicine.Medicine.Id);
                if (availableMedicine != null)
                {
                    availableMedicine.IsSelected = true;
                    availableMedicine.SelectedQuantity = medicine.SelectedQuantity;
                    availableMedicine.SelectedDosage = medicine.SelectedDosage;
                }
            }
        }

        public void LoadFromLastSelection()
        {
            if (_currentFormId == -1) return;

            var formMedicines = GetMedicineSelectionsForForm(_currentFormId);
            if (formMedicines == null || !formMedicines.Any()) return;

            foreach (var medicine in formMedicines)
            {
                var availableMedicine = AvailableMedicines
                    .FirstOrDefault(m => m.Medicine.Id == medicine.Medicine.Id);
                if (availableMedicine != null)
                {
                    availableMedicine.IsSelected = true;
                    availableMedicine.SelectedQuantity = medicine.SelectedQuantity;
                    availableMedicine.SelectedDosage = medicine.SelectedDosage;
                }
            }
            System.Diagnostics.Debug.WriteLine($"Loaded {formMedicines.Count} medicines for form {_currentFormId}");
        }

        public void SaveSelectedMedicines()
        {
            if (_currentFormId == -1) return;

            var selectedMedicines = AvailableMedicines.Where(m => m.IsSelected).ToList();
            ClearMedicineSelectionsForForm(_currentFormId);
            var formMedicines = GetMedicineSelectionsForForm(_currentFormId);
            
            foreach (var medicine in selectedMedicines)
            {
                formMedicines.Add(medicine);
            }
            
            System.Diagnostics.Debug.WriteLine($"Saved {selectedMedicines.Count} medicines for form {_currentFormId}");
        }

        public void InitializeWithFormId(int formId)
        {
            _currentFormId = formId;
        }

        public static ObservableCollection<MedicineSelection> GetMedicineSelectionsForForm(int formId)
        {
            if (!_medicineSelectionsByFormId.ContainsKey(formId))
            {
                _medicineSelectionsByFormId[formId] = new ObservableCollection<MedicineSelection>();
            }
            return _medicineSelectionsByFormId[formId];
        }

        public static void ClearMedicineSelectionsForForm(int formId)
        {
            if (_medicineSelectionsByFormId.ContainsKey(formId))
            {
                _medicineSelectionsByFormId[formId].Clear();
            }
        }

        private void FilterMedicines()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredMedicines = new ObservableCollection<MedicineSelection>(AvailableMedicines);
            }
            else
            {
                var filtered = AvailableMedicines.Where(m => 
                    m.Medicine.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                FilteredMedicines = new ObservableCollection<MedicineSelection>(filtered);
            }
            
            System.Diagnostics.Debug.WriteLine($"Filtered medicines count: {FilteredMedicines.Count}");
        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        public void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
            }
        }
    }
}
