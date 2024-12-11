using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClinicManagementSystem.Service.DataAccess.IDao;

namespace ClinicManagementSystem.ViewModel
{
	public class PatientViewModel : INotifyPropertyChanged
	{
		IDao _dao;

		private ObservableCollection<Patient> _patients;
		private ObservableCollection<PageInfo> _pageinfos;
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; } = 0;
		public int RowsPerPage { get; set; }
		public string Keyword { get; set; } = "";
		public PageInfo SelectedPageInfoItem { get; set; } = new PageInfo();
		public Patient PatientEdit { get; set; }

		public ObservableCollection<PageInfo> PageInfos
		{
			get => _pageinfos ??= new ObservableCollection<PageInfo>();
			set
			{
				_pageinfos = value;
			}
		}

		public ObservableCollection<Patient> Patients
		{
			get => _patients ??= new ObservableCollection<Patient>();
			set
			{
				_patients = value;
			}
		}

		private Dictionary<string, SortType> _sortOptions = new();

		public event PropertyChangedEventHandler PropertyChanged;

		public PatientViewModel()
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
				return $"Displaying {Patients.Count}/{RowsPerPage} of total {TotalItems} item(s)";
			}
		}
		private bool _sortById = false;

		/// <summary>
		/// Chuyển đến trang tiếp theo
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
		/// Chuyển đến trang trước
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
		/// Load dữ liệu từ database
		/// </summary>
		public void LoadData()
		{
			var (items, count) = _dao.GetPatients(CurrentPage, RowsPerPage, Keyword, _sortOptions);

			Patients.Clear();

			foreach (var item in items)
			{
				Patients.Add(item);
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
		/// Tìm kiếm dữ liệu
		/// </summary>
		public void Search()
		{
			CurrentPage = 1;
			LoadData();
		}

		/// <summary>
		/// Chỉnh sửa thông tin bệnh nhân
		/// </summary>
		/// <param name="patient"></param>
		public void Edit(Patient patient)
		{
			PatientEdit = patient;
		}

		/// <summary>
		/// Hủy chỉnh sửa thông tin bệnh nhân
		/// </summary>
		public void Cancel()
		{
			LoadData();
		}

		/// <summary>
		/// Cập nhật thông tin bệnh nhân
		/// </summary>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool Update()
		{
			bool success = _dao.UpdatePatient(PatientEdit);
			return success;
		}

		/// <summary>
		/// Xóa thông tin bệnh nhân
		/// </summary>
		/// <returns>True nếu xóa thành công, False nếu xóa thất bại</returns>
		public bool Delete()
		{
			bool success = _dao.DeletePatient(PatientEdit);
			return success;
		}
	}
}
