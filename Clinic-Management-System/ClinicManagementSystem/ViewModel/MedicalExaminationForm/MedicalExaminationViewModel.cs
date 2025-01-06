using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ClinicManagementSystem.Command;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Views.DoctorView;
using Microsoft.UI.Xaml.Controls;
using Windows.Networking.NetworkOperators;

namespace ClinicManagementSystem.ViewModel
{
	/// <summary>
	/// ViewModel cho MedicalExamination
	/// </summary>
	public class MedicalExaminationViewModel : BaseViewModel
	{
		private readonly SqlServerDao _dataAccess;
		private Frame _navigationFrame;
		private ObservableCollection<MedicalExaminationForm> _examinationForms;
		private string _keyword = "";
		private int _currentPage = 1;
		private int _totalPages;
		private int _totalItems = 0;
		private int _pageSize = 10;
		private ObservableCollection<PageInfo> _pageInfos;
		private PageInfo _selectedPageInfo;
		private Frame _navigationFrame;
		private readonly int doctorId;
		private DateTimeOffset? _startDate;
		private DateTimeOffset? _endDate;

		public MedicalExaminationViewModel()
		{
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			doctorId = UserSessionService.Instance.GetLoggedInUserId();
			LoadExaminationForms();
		}

		public Frame NavigationFrame
		{
			get => _navigationFrame;
			set => _navigationFrame = value;
		}

		public ICommand PreviousPageCommand { get; }
		public ICommand NextPageCommand { get; }

		public MedicalExaminationViewModel()
		{
			_dataAccess = new SqlServerDao();
			ExaminationForms = new ObservableCollection<MedicalExaminationForm>();
			PreviousPageCommand = new RelayCommand(PreviousPage);
			NextPageCommand = new RelayCommand(NextPage);
			LoadExaminationForms(); // Gọi phương thức để tải dữ liệu
		}

		/// <summary>
		/// Tải dữ liệu các phiếu khám từ cơ sở dữ liệu
		/// </summary>
		private void LoadExaminationForms()
		{
            // Lấy danh sách phiếu khám chưa khám
            var forms = _dataAccess.GetMedicalExaminationForms()
                .Where(f => f.IsExaminated == "false")
                .ToList();
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

		public void NavigateToDiagnosisPage(MedicalExaminationForm selectedForm)
		{
			NavigationFrame?.Navigate(typeof(DiagnosisPage), selectedForm);
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

