using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using ClinicManagementSystem.Helper;
using static ClinicManagementSystem.Service.DataAccess.IDao;
using Windows.System;
namespace ClinicManagementSystem.ViewModel
{
	public class MedicalExaminationFormViewModel : INotifyPropertyChanged
	{
		IDao _dao;

		private ObservableCollection<MedicalExaminationForm> _medicalExaminationForms;
		private ObservableCollection<PageInfo> _pageinfos;
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; } = 0;
		public int RowsPerPage { get; set; }
		public string Keyword { get; set; } = "";
		public PageInfo SelectedPageInfoItem { get; set; } = new PageInfo();
		public MedicalExaminationForm FormEdit { get; set; }
		public ObservableCollection<PageInfo> PageInfos
		{
			get => _pageinfos ??= new ObservableCollection<PageInfo>();
			set
			{
				_pageinfos = value;
			}
		}
		public ObservableCollection<MedicalExaminationForm> MedicalExaminationForms
		{
			get => _medicalExaminationForms ??= new ObservableCollection<MedicalExaminationForm>();
			set
			{
				_medicalExaminationForms = value;
			}
		}

		private Dictionary<string, SortType> _sortOptions = new();

		public event PropertyChangedEventHandler PropertyChanged;

		public MedicalExaminationFormViewModel()
		{
			RowsPerPage = 10;
			CurrentPage = 1;
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadData();
		}

		public string Info
		{
			get
			{
				return $"Displaying {MedicalExaminationForms.Count}/{RowsPerPage} of total {TotalItems} item(s)";
			}
		}
		private bool _sortById = false;

		public bool SortById
		{
			get => _sortById;
			set
			{
				_sortById = value;
				if (value == true)
				{
					_sortOptions.Add("patientId", SortType.Ascending);
				}
				else
				{
					if (_sortOptions.ContainsKey("patientId"))
					{
						_sortOptions.Remove("patientId");
					}
				}
				LoadData();
			}
		}

		public void GoToNextPage()
		{
			if (CurrentPage < TotalPages)
			{
				CurrentPage++;
				LoadData();
			}
		}
		public void GoToPreviousPage()
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadData();
			}
		}
		public void GoToPage(int page)
		{
			CurrentPage = page;
			LoadData();
		}

		public void LoadData()
		{
			var (items, count) = _dao.GetMedicalExaminationForm(CurrentPage, RowsPerPage, Keyword, _sortOptions);

			MedicalExaminationForms.Clear();

			foreach (var item in items)
			{
				MedicalExaminationForms.Add(item);
			}

			if (count != TotalItems)
			{
				TotalItems = count;
				TotalPages = TotalItems / RowsPerPage +
					(TotalItems % RowsPerPage == 0 ? 0 : 1);
			}

			PageInfos.Clear();
			for (int i = CurrentPage; i <= Math.Min(CurrentPage + 2, TotalPages); i++)
			{
				PageInfos.Add(new PageInfo
				{
					Page = i,
					Total = TotalPages
				});
			}
			SelectedPageInfoItem = new PageInfo { Page = CurrentPage, Total = TotalPages };
		}

		public void Search()
		{
			CurrentPage = 1;
			LoadData();
		}

		public void Edit(MedicalExaminationForm formEdit)
		{
			FormEdit = formEdit;
		}

		public void Cancel()
		{
			LoadData();
		}

		public bool Update()
		{
			bool success = _dao.UpdateMedicalExaminationForm(FormEdit);
			return success;
		}

		public bool Delete()
		{
			bool success = _dao.DeleteMedicalExaminationForm(FormEdit);
			return success;
		}
	}
}
