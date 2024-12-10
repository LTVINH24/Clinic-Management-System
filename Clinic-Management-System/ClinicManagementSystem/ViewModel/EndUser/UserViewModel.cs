﻿using ClinicManagementSystem.Service;
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

namespace ClinicManagementSystem.ViewModel.EndUser
{

	public class UserViewModel : INotifyPropertyChanged
	{
		IDao _dao;
		public User user { get; set; } = new User();
		// private ValidData valid { get; set; } = new ValidData();
		private ObservableCollection<Specialty> _specialties;
		public ObservableCollection<Specialty> Specialties
		{
			get => _specialties ??= new ObservableCollection<Specialty>();
			set => _specialties = value;
		}
		public Specialty selectedSpecialty { get; set; }
		public string Room { get; set; }
		public UserViewModel()
		{
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadSpecialties();
		}

		public string CreateUser(User user)
		{


			if (_dao.CheckUserExists(user.username))
			{
				return "Username already exists";
			}
			else
			{
				var Password = new Password();
				user.password = Password.HashPassword(user.password);
				var success = true;
				if (user.role == "doctor")
				{

					success = _dao.CreateUserRoleDoctor(user, selectedSpecialty.id, Room);
				}
				else
				{
					success = _dao.CreateUser(user);
				}
				return success ? "" : "Create false";
			}

		}

		private void LoadSpecialties()
		{
			var specialties = _dao.GetSpecialty();
			Specialties.Clear();
			foreach (var specialtiesItem in specialties)
			{
				Specialties.Add(specialtiesItem);
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

	}

}