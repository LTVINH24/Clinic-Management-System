﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManagementSystem.Model
{
	/// <summary>
	/// Lớp MedicineSelection chứa thông tin của thuốc được chọn gồm các thuộc tính Medicine, IsSelected, SelectedQuantity, SelectedDosage
	/// </summary>
	public class MedicineSelection : INotifyPropertyChanged
    {
        private Medicine _medicine;
        private bool _isSelected;
        private int _selectedQuantity;
        private string _selectedDosage;

        public Medicine Medicine
        {
            get => _medicine;
            set => SetProperty(ref _medicine, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set
            {
                if (SetProperty(ref _selectedQuantity, value))
                {
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public string SelectedDosage
        {
            get => _selectedDosage;
            set => SetProperty(ref _selectedDosage, value);
        }

        public int TotalPrice
        {
            get => Medicine != null ? Medicine.Price * SelectedQuantity : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
