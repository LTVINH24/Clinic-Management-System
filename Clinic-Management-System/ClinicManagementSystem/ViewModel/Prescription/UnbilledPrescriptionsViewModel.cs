using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using System;
using System.Linq;

namespace ClinicManagementSystem.ViewModel
{
    /// <summary>
    /// ViewModel cho UnbilledPrescriptions
    /// </summary>
    public class UnbilledPrescriptionsViewModel : BaseViewModel
    {
        private IDao _dao;
        private string _keyword = "";
        private int _currentPage = 1;
        private int _totalPages;
        private int _totalItems = 0;
        private int _pageSize = 10;
        private ObservableCollection<Prescription> _prescriptions;
        private ObservableCollection<PageInfo> _pageInfos;
        private PageInfo _selectedPageInfo;
        private bool _isUpdatingPageInfo = false;

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

        public ObservableCollection<PageInfo> PageInfos
        {
            get => _pageInfos ??= new ObservableCollection<PageInfo>();
            set => SetProperty(ref _pageInfos, value);
        }

        public PageInfo SelectedPageInfo
        {
            get => _selectedPageInfo;
            set
            {
                if (_isUpdatingPageInfo) return;
                
                if (SetProperty(ref _selectedPageInfo, value) && value != null)
                {
                    CurrentPage = value.Page;
                    LoadPrescriptions();
                }
            }
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
                "false",
                Keyword
            );

            Prescriptions.Clear();
            foreach (var prescription in prescriptions)
            {
                Prescriptions.Add(prescription);
            }

            TotalItems = totalCount;
            TotalPages = (totalCount + PageSize - 1) / PageSize;
            UpdatePageInfos();
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
        
        public void GoToPage(int page)
        {
            CurrentPage = page;
            LoadPrescriptions();
        }
        private void UpdatePageInfos()
        {
            try
            {
                _isUpdatingPageInfo = true;
                PageInfos.Clear();
                for (int i = 1; i <= TotalPages; i++)
                {
                    PageInfos.Add(new PageInfo { Page = i, Total = TotalPages });
                }
                
                var newSelectedPage = PageInfos.FirstOrDefault(p => p.Page == CurrentPage);
                SetProperty(ref _selectedPageInfo, newSelectedPage, nameof(SelectedPageInfo));
            }
            finally
            {
                _isUpdatingPageInfo = false;
            }
        }
    }
} 