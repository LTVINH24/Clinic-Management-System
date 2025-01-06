using ClinicManagementSystem.Service.DataAccess;
using ClinicManagementSystem.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.ViewModel.EndUser
{
	/// <summary>
	/// ViewModel cho trang chủ của bác sĩ
	/// </summary>
	public class DoctorHomePageViewModel : INotifyPropertyChanged
	{
		private readonly IDao _dao;
		public int TodayFormsCount { get; set; }
		public int TodayCompletedFormsCount { get; set; }
		public int MonthlyPatientCount { get; set; }
		public int MonthlyPrescriptionsCount { get; set; }

		public DoctorHomePageViewModel()
		{
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadData();
		}

		public void LoadData()
		{
			var doctorId = UserSessionService.Instance.GetLoggedInUserId();

			TodayFormsCount = _dao.GetTodayFormsByDoctorId(doctorId);
			TodayCompletedFormsCount = _dao.GetTodayCompletedFormsByDoctorId(doctorId);
			MonthlyPatientCount = _dao.GetMonthlyPatientCountByDoctorId(doctorId);
			MonthlyPrescriptionsCount = _dao.GetMonthlyPrescriptionCountByDoctorId(doctorId);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
