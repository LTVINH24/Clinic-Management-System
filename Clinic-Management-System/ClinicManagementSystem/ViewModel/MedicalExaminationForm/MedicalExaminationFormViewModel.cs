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
		public IDao _dao;

		private ObservableCollection<MedicalExaminationForm> _medicalExaminationForms;
		private ObservableCollection<PageInfo> _pageinfos;
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; } = 0;
		public int RowsPerPage { get; set; }
		public string Keyword { get; set; } = "";
		private DateTimeOffset? _startDate;
		public DateTimeOffset? StartDate
		{
			get => _startDate;
			set
			{
				_startDate = value;
				LoadData();
			}
		}

		private DateTimeOffset? _endDate;
		public DateTimeOffset? EndDate
		{
			get => _endDate;
			set
			{
				_endDate = value;
				LoadData();
			}
		}
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

		/// <summary>
		/// Chuyển đến trang kế tiếp
		/// </summary>
		public void GoToNextPage()
		{
			if (CurrentPage < TotalPages)
			{
				CurrentPage++;
				LoadData();
			}
		}

		/// <summary>
		/// Chuyển đến trang trước đó
		/// </summary>
		public void GoToPreviousPage()
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				LoadData();
			}
		}

		/// <summary>
		/// Chuyển đến trang được chỉ định
		/// </summary>
		/// <param name="page"></param>
		public void GoToPage(int page)
		{
			CurrentPage = page;
			LoadData();
		}

		/// <summary>
		/// Load dữ liệu phiếu khám bệnh
		/// </summary>
		public void LoadData()
		{
			var (items, count) = _dao.GetMedicalExaminationForms(
				CurrentPage, 
				RowsPerPage, 
				Keyword,
				StartDate,
				EndDate,
				_sortOptions
			);

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

		/// <summary>
		/// Tìm kiếm phiếu khám bệnh	
		/// </summary>
		public void Search()
		{
			CurrentPage = 1;
			LoadData();
		}

		/// <summary>
		/// Chỉnh sửa phiếu khám bệnh
		/// </summary>
		/// <param name="formEdit"></param>
		public void Edit(MedicalExaminationForm formEdit)
		{
			FormEdit = formEdit;
		}

		/// <summary>
		/// Hủy chỉnh sửa
		/// </summary>
		public void Cancel()
		{
			LoadData();
		}

		public (bool, string) isValidDataUpdate()
		{
			IsValidData isValid = new IsValidData();

			if(isValid.IsValidDescription(FormEdit.Symptoms) == false)
			{
				return (false, "Invalid symptoms.");
			}

			return (true, "Valid data.");
		}

		/// <summary>
		/// Cập nhật phiếu khám bệnh
		/// </summary>
		/// <returns></returns>
		public (bool, string) Update()
		{
			var (isValid, message) = isValidDataUpdate();

			if (!isValid)
			{
				return (false, message);
			}

			bool success = _dao.UpdateMedicalExaminationForm(FormEdit);
			return (success, "Success");
		}

		/// <summary>
		/// Xóa phiếu khám bệnh
		/// </summary>
		/// <returns></returns>
		public bool Delete()
		{
			bool success = _dao.DeleteMedicalExaminationForm(FormEdit);
			return success;
		}
	}
}
