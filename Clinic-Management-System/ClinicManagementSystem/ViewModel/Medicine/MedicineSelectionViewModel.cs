using System.Collections.ObjectModel;
using System.Windows.Input;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Command;
using System.Collections.Generic;
using System;
using ClinicManagementSystem.Model;

namespace ClinicManagementSystem.ViewModel
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
