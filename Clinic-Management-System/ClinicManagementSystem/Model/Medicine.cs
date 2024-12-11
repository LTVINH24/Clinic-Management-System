using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicManagementSystem.Model
{
    public class Medicine : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _manufacturer;
        private int _price;
        private int _quantity;
        private DateTimeOffset _expDate;
        private DateTimeOffset _mfgDate;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set => SetProperty(ref _manufacturer, value);
        }

        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public DateTimeOffset ExpDate
        {
            get => _expDate;
            set => SetProperty(ref _expDate, value);
        }

        public DateTimeOffset MfgDate
        {
            get => _mfgDate;
            set => SetProperty(ref _mfgDate, value);
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
