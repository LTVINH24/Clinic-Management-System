using System;
using System.Linq;
using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;

namespace ClinicManagementSystem.ViewModel
{
    public class ExaminedFormsViewModel : BaseViewModel
    {
        private IDao _dao;
        private string _keyword = "";
        private int _currentPage = 1;
        private int _totalPages;
        private int _totalItems = 0;
        private int _pageSize = 10;
        private ObservableCollection<PageInfo> _pageInfos;
        private PageInfo _selectedPageInfo;
        private ObservableCollection<MedicalExaminationForm> _examinationForms;
        private readonly int doctorId;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

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

        public ObservableCollection<MedicalExaminationForm> ExaminationForms
        {
            get => _examinationForms ??= new ObservableCollection<MedicalExaminationForm>();
            set => SetProperty(ref _examinationForms, value);
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

        public ExaminedFormsViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            doctorId = UserSessionService.Instance.LoggedInUserId;
            LoadExaminationForms();
        }

        public void Search()
        {
            CurrentPage = 1;
            LoadExaminationForms();
        }

        private void LoadExaminationForms()
        {
            var (forms, totalCount) = _dao.GetDoctorExaminationForms(
                doctorId, 
                CurrentPage, 
                PageSize,
                "true",  // examined forms
                Keyword,
                StartDate,
                EndDate
            );
            
            ExaminationForms.Clear();
            foreach (var form in forms)
            {
                ExaminationForms.Add(form);
            }

            if (totalCount != TotalItems)
            {
                TotalItems = totalCount;
                TotalPages = TotalItems / PageSize + 
                    (TotalItems % PageSize == 0 ? 0 : 1);
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
                LoadExaminationForms();
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadExaminationForms();
            }
        }

        public void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                LoadExaminationForms();
            }
        }

        public void ClearFilter()
        {
            Keyword = "";
            StartDate = null;
            EndDate = null;
            Search();
        }
    }
}