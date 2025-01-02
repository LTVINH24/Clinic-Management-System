using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using OxyPlot;

namespace ClinicManagementSystem.ViewModel.EndUser
{

	public class UserViewModel : INotifyPropertyChanged
	{
		IDao _dao;
		public string NewSpecialty {  get; set; }
		public User user { get; set; } = new User();
		private ObservableCollection<Specialty> _specialties;
		public ObservableCollection<Specialty> Specialties
		{
			get => _specialties ??= new ObservableCollection<Specialty>();
			set => _specialties = value;
		}
		public Specialty selectedSpecialty { get; set; } = new Specialty();
		public string Room { get; set; }
		public UserViewModel()
		{
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadSpecialties();
		}

		/// <summary>
		/// Tạo mới user
		/// </summary>
		/// <param name="user"></param>
		/// <returns>Thông báo tạo thành công hay thất bại</returns>
		public string CreateUser()
		{
			if (_dao.CheckUserExists(user.username))
			{
				return "Username already exists";
			}
			else
			{
				var Password = new Password();
				
				var success = true;
				if (user.role == "doctor")
				{
					if(selectedSpecialty.id==0)
					{
						return "Please choose specialty";
					}
					if (Room == "" || Room == null)
                    {
                        return "Please enter room";
                    }
                    user.password = Password.HashPassword(user.password);
                    success = _dao.CreateUserRoleDoctor(user, selectedSpecialty.id, Room);
					
				}
				else
				{
                    user.password = Password.HashPassword(user.password);
                    success = _dao.CreateUser(user);
				}
				Room = "";
				NewSpecialty = "";
                user = new User();
                return success ? "" : "Create false";
			}

		}

		/// <summary>
		/// Load dữ liệu chuyên khoa
		/// </summary>
		public void LoadSpecialties()
		{
			var specialties = _dao.GetSpecialty();
			Specialties.Clear();
			foreach (var specialtiesItem in specialties)
			{
				Specialties.Add(specialtiesItem);
			}
		}

		/// <summary>
		/// Tạo mới chuyên khoa
		/// </summary>
		/// <returns>True nếu tạo thành công, False nếu tạo thất bại</returns>
		public bool CreateNewSpecialty()
		{
			if (NewSpecialty != null && NewSpecialty != "")
			{
				var (success,newSpecialtyId)= _dao.CreateSpecialty(NewSpecialty);
				if(success)
				{
                    selectedSpecialty.id = newSpecialtyId;
					return true;

                }
				return false;
			}
			return false;
		}
		public event PropertyChangedEventHandler PropertyChanged;

	}

}
