using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Command;
using System;
using System.Linq;

namespace ClinicManagementSystem.ViewModel
{
    public class BillListViewModel : BaseViewModel
    {
        private readonly IDao _dao;
        private ObservableCollection<Bill> _bills;
        private string _keyword = "";
        private int _currentPage = 1;
        private int _totalPages;
        private int _totalItems = 0;
        private int _pageSize = 10;
        private ObservableCollection<PageInfo> _pageInfos;
        private PageInfo _selectedPageInfo;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public BillListViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            LoadBills();
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills ??= new ObservableCollection<Bill>();
            set => SetProperty(ref _bills, value);
        }

        public ObservableCollection<PageInfo> PageInfos
        {
            get => _pageInfos ??= new ObservableCollection<PageInfo>();
            set => SetProperty(ref _pageInfos, value);
        }

        public PageInfo SelectedPageInfo
        {
            get => _selectedPageInfo;
            set => SetProperty(ref _selectedPageInfo, value);
        }

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

        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    Search();
                }
            }
        }

        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    Search();
                }
            }
        }

        public void Search()
        {
            CurrentPage = 1;
            LoadBills();
        }

        private void LoadBills()
        {
            var (bills, totalCount) = _dao.GetBillsByPage(
                CurrentPage, 
                _pageSize, 
                Keyword, 
                StartDate, 
                EndDate
            );

            Bills.Clear();
            foreach (var bill in bills)
            {
                Bills.Add(bill);
            }

            if (totalCount != TotalItems)
            {
                TotalItems = totalCount;
                TotalPages = TotalItems / _pageSize + 
                    (TotalItems % _pageSize == 0 ? 0 : 1);
            }

            PageInfos.Clear();
            int startPage = Math.Max(1, CurrentPage - 1);
            int endPage = Math.Min(startPage + 2, TotalPages);
            
            if (endPage - startPage < 2 && startPage > 1)
            {
                startPage = Math.Max(1, endPage - 2);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                PageInfos.Add(new PageInfo
                {
                    Page = i,
                    Total = TotalPages
                });
            }
            
            SelectedPageInfo = new PageInfo { Page = CurrentPage, Total = TotalPages };
        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadBills();
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadBills();
            }
        }

        public void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                LoadBills();
            }
        }

        public void ClearFilter()
        {
            StartDate = null;
            EndDate = null;
            Keyword = "";
            LoadBills();
        }
    }
} 
