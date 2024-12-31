using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.EndUser
{
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

        public void LoadData()
        {
            TotalPatientCount = _dao.GetTotalPatientsCount();
            TotalUserCount = _dao.GetTotalUsersCount();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
