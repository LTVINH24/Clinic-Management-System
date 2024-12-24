using System.Collections.ObjectModel;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Model;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ClinicManagementSystem.ViewModel
{
    public class MedicineSelectionViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;

        private ObservableCollection<MedicineSelection> _availableMedicines;
        public ObservableCollection<MedicineSelection> AvailableMedicines
        {
            get { return _availableMedicines; }
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

        public MedicineSelectionViewModel()
        {
            _dataAccess = new SqlServerDao();
            AvailableMedicines = new ObservableCollection<MedicineSelection>();
            _currentFormId = -1;
            LoadAvailableMedicines();
            FilteredMedicines = new ObservableCollection<MedicineSelection>(AvailableMedicines);
        }

		/// <summary>
		/// Load dữ liệu thuốc có sẵn từ database
		/// </summary>
		private void LoadAvailableMedicines()
        {
            var medicines = _dataAccess.GetAvailableMedicines();
            AvailableMedicines.Clear();
            foreach (var medicine in medicines)
            {
                AvailableMedicines.Add(new MedicineSelection { Medicine = medicine });
            }
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
    }
}
