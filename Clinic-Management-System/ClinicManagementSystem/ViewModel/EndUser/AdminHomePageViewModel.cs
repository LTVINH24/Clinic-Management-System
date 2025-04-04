﻿using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.EndUser
{
    /// <summary>
    /// ViewModel cho AdminHomePage
    /// </summary>
    public class AdminHomePageViewModel : INotifyPropertyChanged
    {
        private readonly IDao _dao;
        public int TotalPatientCount { get; set; }
        public int TotalUserCount { get; set; }

        public AdminHomePageViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            LoadData();
        }

		/// <summary>
		/// Load dữ liệu gồm tổng số bệnh nhân và tổng số người dùng
		/// </summary>
		public void LoadData()
        {
            TotalPatientCount = _dao.GetTotalPatientsCount();
            TotalUserCount = _dao.GetTotalUsersCount();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
