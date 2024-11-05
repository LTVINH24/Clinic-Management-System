using System.Collections.ObjectModel;
using System.Windows.Input;
using Clinic_Management_System.Model.DoctorModel;
using Clinic_Management_System.Service.DataAccess;
using Clinic_Management_System.Command;
using System.Collections.Generic;
using System;

namespace Clinic_Management_System.ViewModel.DoctorViewModel
{
    public class MedicineSelectionViewModel : BaseViewModel
    {
        private readonly SqlServerDao _dataAccess;

        public ObservableCollection<Medicine> AvailableMedicines { get; set; } = new ObservableCollection<Medicine>();
        public event Action<List<Medicine>> MedicinesSelected;

        public ICommand ConfirmSelectionCommand { get; }

        public MedicineSelectionViewModel()
        {
            _dataAccess = new SqlServerDao();
            LoadAvailableMedicines();
            ConfirmSelectionCommand = new RelayCommand(ConfirmSelection);
        }

        private void LoadAvailableMedicines()
        {
            var medicines = _dataAccess.GetAvailableMedicines();
            AvailableMedicines.Clear();
            foreach (var medicine in medicines)
            {
                AvailableMedicines.Add(medicine);
            }
        }

        private void ConfirmSelection()
        {
            // Logic xác nhận lựa chọn thuốc và gửi sự kiện
            MedicinesSelected?.Invoke(new List<Medicine>(AvailableMedicines));
        }
    }
}
