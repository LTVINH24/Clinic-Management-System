using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Service;
using static ClinicManagementSystem.Service.DataAccess.IDao;
using ClinicManagementSystem.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace ClinicManagementSystem.ViewModel.EndUser
{
	public class AccountViewModel : INotifyPropertyChanged
	{
		IDao _dao;
		private UserViewModel UserViewModel { get; set; } = new UserViewModel();
		private ObservableCollection<User> _users;
		private ObservableCollection<PageInfo> _pageinfos;
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; } = 0;
		public int RowsPerPage { get; set; }
		public string Keyword { get; set; } = "";
		public PageInfo SelectedPageInfoItem { get; set; } = new PageInfo();
		public User UserEdit { get; set; } = new User();
		public ObservableCollection<PageInfo> PageInfos
		{
			get => _pageinfos ??= new ObservableCollection<PageInfo>();
			set
			{
				_pageinfos = value;
			}
		}
		public ObservableCollection<User> Users
		{
			get => _users ??= new ObservableCollection<User>();
			set => _users = value;
		}
		private Dictionary<string, SortType> _sortOptions = new();

		public AccountViewModel()
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
				return $"Displaying {Users.Count}/{RowsPerPage} of total {TotalItems} item(s)";
			}
		}
		private bool _sortById = false;

		public event PropertyChangedEventHandler PropertyChanged;

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

			var (items, count) = _dao.GetUsers(
				CurrentPage, RowsPerPage, Keyword,
				_sortOptions
			);
			Users.Clear();
			foreach (var user in items)
			{
				Users.Add(user);
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
		public void Edit(User useredit)
		{
			UserEdit = useredit;
		}
		public void Cancel()
		{
			UserEdit = new User();
		}
		public bool Update()
		{
			var Password = new Password();	
			string temp = UserEdit.password;
			UserEdit.password = Password.HashPassword(UserEdit.password);
			bool sucess = _dao.UpdateUser(UserEdit);
			UserEdit.password = temp;
			return sucess;
		}
		public bool Delete()
		{
			return _dao.DeleteUser(UserEdit);
		}
	}
}
