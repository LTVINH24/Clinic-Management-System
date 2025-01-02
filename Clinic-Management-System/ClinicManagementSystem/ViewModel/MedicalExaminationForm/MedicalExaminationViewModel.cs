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
	public class MedicalExaminationViewModel : BaseViewModel
	{
		private readonly SqlServerDao _dataAccess;
		private ObservableCollection<MedicalExaminationForm> _examinationForms;
		private ObservableCollection<PageInfo> _pageInfos;
		private PageInfo _selectedPageInfo;
		private Frame _navigationFrame;
		private int _currentPage = 0;
		private const int PageSize = 2;
		private readonly int doctorId;
		private string _keyword = "";

		public MedicalExaminationViewModel()
		{
			_dataAccess = new SqlServerDao();
			doctorId = UserSessionService.Instance.GetLoggedInUserId();
			LoadExaminationForms();
		}

		public Frame NavigationFrame
		{
			get => _navigationFrame;
			set => _navigationFrame = value;
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

		public string Keyword
		{
			get => _keyword;
			set
			{
				if (SetProperty(ref _keyword, value))
				{
					System.Diagnostics.Debug.WriteLine($"Keyword changed to: {value}");
					_currentPage = 0;
					LoadExaminationForms();
				}
			}
		}

		private void LoadExaminationForms()
		{
			System.Diagnostics.Debug.WriteLine($"Loading forms with keyword: {Keyword}");
			var (forms, totalCount) = _dataAccess.GetDoctorPendingExaminationForms(
				doctorId, 
				_currentPage + 1, 
				PageSize,
				Keyword
			);
			System.Diagnostics.Debug.WriteLine($"Total count: {totalCount}, Forms count: {forms.Count}");

			ExaminationForms.Clear();
			foreach (var form in forms)
			{
				ExaminationForms.Add(form);
			}

			int totalPages = (totalCount + PageSize - 1) / PageSize;
			totalPages = Math.Max(1, totalPages);

			PageInfos.Clear();
			for (int i = 1; i <= totalPages; i++)
			{
				PageInfos.Add(new PageInfo { Page = i, Total = totalPages });
			}
			
			System.Diagnostics.Debug.WriteLine($"Current page: {_currentPage + 1}, Total pages: {totalPages}");
			
			SelectedPageInfo = PageInfos.FirstOrDefault(p => p.Page == _currentPage + 1) 
				?? new PageInfo { Page = 1, Total = totalPages };
		}

		public void GoToNextPage()
		{
			System.Diagnostics.Debug.WriteLine($"Attempting to go to next page. Current: {_currentPage + 1}");
			if (_currentPage < (PageInfos.Count - 1))
			{
				_currentPage++;
				LoadExaminationForms();
			}
		}

		public void GoToPreviousPage()
		{
			System.Diagnostics.Debug.WriteLine($"Attempting to go to previous page. Current: {_currentPage + 1}");
			if (_currentPage > 0)
			{
				_currentPage--;
				LoadExaminationForms();
			}
		}

		public void GoToPage(int page)
		{
			System.Diagnostics.Debug.WriteLine($"Attempting to go to page {page}");
			if (page >= 1 && page <= PageInfos.Count)
			{
				_currentPage = page - 1;
				LoadExaminationForms();
			}
		}

		public void NavigateToDiagnosisPage(MedicalExaminationForm selectedForm)
		{
			NavigationFrame?.Navigate(typeof(DiagnosisPage), selectedForm);
		}
	}
}

