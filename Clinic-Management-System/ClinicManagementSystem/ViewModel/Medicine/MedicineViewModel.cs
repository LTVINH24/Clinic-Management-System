using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ClinicManagementSystem.Model;
using static ClinicManagementSystem.Service.DataAccess.IDao;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using Windows.System;
namespace ClinicManagementSystem.ViewModel
{
	public class MedicineViewModel : INotifyPropertyChanged
	{
		IDao _dao;
		private ObservableCollection<Medicine> _medicines;
		public ObservableCollection<Medicine> Medicines
		{
			get => _medicines ??= new ObservableCollection<Medicine>();
			set => _medicines = value;
		}
		public Medicine MedicineEdit { get; set; } = new Medicine();
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; } = 0;
		public int RowsPerPage { get; set; }
		public string Keyword { get; set; } = "";
		private Dictionary<string, SortType> _sortOptions = new();
		public MedicineViewModel()
		{
			RowsPerPage = 5;
			CurrentPage = 1;
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadMedicines();
		}
		public PageInfo SelectedPageInfoItem { get; set; } = new PageInfo();
		private ObservableCollection<PageInfo> _pageinfos;
		public ObservableCollection<PageInfo> PageInfos
		{
			get => _pageinfos ??= new ObservableCollection<PageInfo>();
			set
			{
				_pageinfos = value;
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private bool _sortById = false;
		public bool SortById
		{
			get => _sortById;
			set
			{
				_sortById = value;
				if (value == true)
				{
					_sortOptions.Add("Name", SortType.Ascending);
				}
				else
				{
					if (_sortOptions.ContainsKey("Name"))
					{
						_sortOptions.Remove("Name");
					}
				}
				LoadMedicines();
			}
		}

		/// <summary>
		/// Load dữ liệu thuốc từ database
		/// </summary>
		private void LoadMedicines()
		{
			var (items, count) = _dao.GetMedicines(
				CurrentPage, RowsPerPage, Keyword,
				_sortOptions
			);
			Medicines.Clear();
			foreach (var medicine in items)
			{
				Medicines.Add(medicine);
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
		/// Tìm kiếm thuốc
		/// </summary>
		public void Search()
		{
			CurrentPage = 1;
			LoadMedicines();
		}

		/// <summary>
		/// Chuyển đến trang kế tiếp
		/// </summary>
		public void GoToNextPage()
		{
			if (CurrentPage < TotalPages)
			{
				CurrentPage++;
				LoadMedicines();
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
				LoadMedicines();
			}
		}

		/// <summary>
		/// Chuyển đến trang được chỉ định
		/// </summary>
		/// <param name="page"></param>
		public void GoToPage(int page)
		{
			CurrentPage = page;
			LoadMedicines();
		}

		/// <summary>
		/// Cập nhật thông tin thuốc
		/// </summary>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool UpdateMedicine()
		{
			bool success = _dao.UpdateMedicine(MedicineEdit);
			LoadMedicines();
			return success;

		}

		/// <summary>
		/// Thêm thuốc mới
		/// </summary>
		/// <returns>True nếu thêm thành công, False nếu thêm thất bại</returns>
		public bool AddMedicine()
		{
			bool success = _dao.CreateMedicine(MedicineEdit);
			LoadMedicines();
			return success;

		}

		/// <summary>
		/// Chỉnh sửa thông tin thuốc
		/// </summary>
		/// <param name="medicine"></param>
		public void EditMedicine(Medicine medicine)
		{
			MedicineEdit = medicine;
		}

		/// <summary>
		/// Xóa thuốc
		/// </summary>
		/// <param name="newMedicine"></param>
		/// <returns>True nếu xóa thành công, False nếu xóa thất bại</returns>
		public bool DeleteMedicine(Medicine newMedicine)
		{
			bool success = _dao.DeleteMedicine(newMedicine);
			LoadMedicines();
			return success;
		}

		/// <summary>
		/// Hủy thao tác
		/// </summary>
		public void CancelMedicine()
		{
			MedicineEdit = new Medicine();
		}
	}
}
