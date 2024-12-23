using System.Collections.ObjectModel;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Model;
using System.ComponentModel;
using System.Linq;

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

        private static ObservableCollection<MedicineSelection> _lastSelectedMedicines;
        public static ObservableCollection<MedicineSelection> LastSelectedMedicines
        {
            get => _lastSelectedMedicines ?? (_lastSelectedMedicines = new ObservableCollection<MedicineSelection>());
            set => _lastSelectedMedicines = value;
        }

        public ObservableCollection<MedicineSelection> SelectedMedicines { get; set; }

        public MedicineSelectionViewModel()
        {
            _dataAccess = new SqlServerDao();
            AvailableMedicines = new ObservableCollection<MedicineSelection>();
            SelectedMedicines = new ObservableCollection<MedicineSelection>();
            LoadAvailableMedicines();
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
            if (LastSelectedMedicines == null || !LastSelectedMedicines.Any()) return;

            foreach (var medicine in LastSelectedMedicines)
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
            System.Diagnostics.Debug.WriteLine($"Loaded {LastSelectedMedicines.Count} medicines from static storage");
        }

        public void SaveSelectedMedicines()
        {
            var selectedMedicines = AvailableMedicines.Where(m => m.IsSelected).ToList();
            LastSelectedMedicines.Clear();
            foreach (var medicine in selectedMedicines)
            {
                LastSelectedMedicines.Add(medicine);
            }
            System.Diagnostics.Debug.WriteLine($"Saved {LastSelectedMedicines.Count} medicines to static storage");
        }
    }
}
