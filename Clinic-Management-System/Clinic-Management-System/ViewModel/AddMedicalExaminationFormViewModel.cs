using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.ViewModel
{
    public class AddMedicalExaminationFormViewModel
    {
        IDao _dao;


		public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();
		// Filter
		//public ObservableCollection<Doctor> FilteredDoctors { get; set; } = new ObservableCollection<Doctor>();

		//private string _doctorNameFilter;
		//public string DoctorNameFilter
		//{
		//	get { return _doctorNameFilter; }
		//	set
		//	{
		//		_doctorNameFilter = value;
				
		//	}
		//}


		private Doctor _selectedDoctor;
		public Doctor SelectedDoctor
		{
			get { return _selectedDoctor; }
			set
			{
				_selectedDoctor = value;
				MedicalExaminationForm.DoctorId = value.Id;
			}
		}


		public AddMedicalExaminationFormViewModel()
        {
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadDoctors();
		}

        public Patient Patient { get; set; } = new Patient();
		public MedicalExaminationForm MedicalExaminationForm { get; set; } = new MedicalExaminationForm();

		


		public event Action<string> AddCompleted;

		public void AddMedicalExaminationForm()
		{
			var (patientSaved, patientId) = _dao.AddPatient(Patient);
			if (patientSaved && patientId != 0)
			{
				
				bool examinationSaved = _dao.AddMedicalExaminationForm(patientId, MedicalExaminationForm);
				if (examinationSaved)
				{
					AddCompleted?.Invoke("Success");
				}
				else
				{
					AddCompleted?.Invoke("Failed");
				}
			}
		}

		private void LoadDoctors()
		{
			var doctors = _dao.GetInforDoctor();
			Doctors.Clear();
			foreach (var doctor in doctors)
			{
				Doctors.Add(doctor);
			}
		}

		//private List<Doctor> FilterDoctor(string doctorName, string specialtyName)
		//{
		//	var doctors = _dao.GetInforDoctor();
		//	Doctors.Clear();
		//	foreach (var doctor in doctors)
		//	{
		//		if (doctorName != null && specialtyName != null)
		//		{
		//			if (doctor.name.Contains(doctorName) && doctor.SpecialtyName.Contains(specialtyName))
		//			{
		//				Doctors.Add(doctor);
		//			}
		//		}
		//		else if (doctorName != null)
		//		{
		//			if (doctor.name.Contains(doctorName))
		//			{
		//				Doctors.Add(doctor);
		//			}
		//		}
		//		else if (specialtyName != null)
		//		{
		//			if (doctor.SpecialtyName.Contains(specialtyName))
		//			{
		//				Doctors.Add(doctor);
		//			}
		//		}
		//		else
		//		{
		//			Doctors.Add(doctor);
		//		}
		//	}
		//	return Doctors.ToList();
		//}
	}
}
