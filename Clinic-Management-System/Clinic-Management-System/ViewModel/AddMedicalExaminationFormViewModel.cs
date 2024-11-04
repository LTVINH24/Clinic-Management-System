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
    public class AddMedicalExaminationFormViewModel : INotifyPropertyChanged
    {
        IDao _dao;


		public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();

		private string _doctorNameFilter;
		private string _specialtyFilter;
		public string DoctorNameFilter
		{
			get { return _doctorNameFilter; }
			set
			{
				_doctorNameFilter = value;
				LoadDoctors(_doctorNameFilter, _specialtyFilter);

			}
		}

		public string SpecialtyFilter
		{
			get { return _specialtyFilter; }
			set
			{
				_specialtyFilter = value;
				OnPropertyChanged(nameof(SpecialtyFilter));
				LoadDoctors(_doctorNameFilter, _specialtyFilter);
			}
		}


		private Doctor _selectedDoctor;
		public Doctor SelectedDoctor
		{
			get { return _selectedDoctor; }
			set
			{
				_selectedDoctor = value;
				OnPropertyChanged(nameof(SelectedDoctor));
				if(_selectedDoctor != null)
				{
					MedicalExaminationForm.DoctorId = _selectedDoctor.Id;
				}
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
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

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

		public void LoadDoctors(string doctorNameFilter = null, string specialtyFilter = null)
		{

			var doctors = _dao.GetInforDoctor(); 
			Doctors.Clear(); 


			foreach (var doctor in doctors)
			{
				bool matchesName = string.IsNullOrEmpty(doctorNameFilter) ||
								   doctor.name.Contains(doctorNameFilter, StringComparison.OrdinalIgnoreCase);
				bool matchesSpecialty = string.IsNullOrEmpty(specialtyFilter) ||
										doctor.SpecialtyName.Contains(specialtyFilter, StringComparison.OrdinalIgnoreCase);

				if (matchesName && matchesSpecialty)
				{
					Doctors.Add(doctor); 
				}
			}

			OnPropertyChanged(nameof(Doctors));

		}
	}
}
