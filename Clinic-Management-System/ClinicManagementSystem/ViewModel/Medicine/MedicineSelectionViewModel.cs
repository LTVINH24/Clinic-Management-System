using System.Collections.ObjectModel;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Model;
using System.ComponentModel;

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

        public ObservableCollection<MedicineSelection> SelectedMedicines { get; set; }

        public MedicineSelectionViewModel()
        {
            _dataAccess = new SqlServerDao();
            AvailableMedicines = new ObservableCollection<MedicineSelection>();
            SelectedMedicines = new ObservableCollection<MedicineSelection>();
            LoadAvailableMedicines();
        }

        private void LoadAvailableMedicines()
        {
            var medicines = _dataAccess.GetAvailableMedicines();
            AvailableMedicines.Clear();
            foreach (var medicine in medicines)
            {
                AvailableMedicines.Add(new MedicineSelection { Medicine = medicine });
            }
        }
    }
}
