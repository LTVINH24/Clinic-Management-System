using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using System;

namespace ClinicManagementSystem.ViewModel
{
    public class UnbilledPrescriptionsViewModel : BaseViewModel
    {
        private IDao _dao;
        private string _keyword = "";
        private int _currentPage = 1;
        private int _totalPages;
        private int _totalItems = 0;
        private int _pageSize = 10;
        private ObservableCollection<Prescription> _prescriptions;

        public string Keyword
        {
            get => _keyword;
            set
            {
                if (SetProperty(ref _keyword, value))
                {
                    Search();
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public int TotalItems
		{
			get => _totalItems;
			set => SetProperty(ref _totalItems, value);
		}

        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        public ObservableCollection<Prescription> Prescriptions
        {
            get => _prescriptions ??= new ObservableCollection<Prescription>();
            set => SetProperty(ref _prescriptions, value);
        }

        public UnbilledPrescriptionsViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            LoadPrescriptions();
        }

        public void Search()
        {
            CurrentPage = 1;
            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            var (prescriptions, totalCount) = _dao.GetPrescriptionsByPage(
                CurrentPage,
                PageSize,
                "false", // isBilled = false
                Keyword
            );

            Prescriptions.Clear();
            foreach (var prescription in prescriptions)
            {
                Prescriptions.Add(prescription);
            }

            TotalItems = totalCount;
            TotalPages = (totalCount + PageSize - 1) / PageSize;
        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadPrescriptions();
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPrescriptions();
            }
        }
    }
} 