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

		


		public event Action<string, int> AddCompleted;
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool isValidData()
		{
			return !string.IsNullOrWhiteSpace(Patient.Name)
				&& !string.IsNullOrWhiteSpace(Patient.Email)
				&& !string.IsNullOrWhiteSpace(Patient.ResidentId)
				&& !string.IsNullOrWhiteSpace(Patient.Address)
				&& Patient.DoB != null
				&& !string.IsNullOrWhiteSpace(Patient.Gender)
				&& !string.IsNullOrWhiteSpace(MedicalExaminationForm.Symptoms);
		}


		// Success, 200: Thêm thành công bệnh nhân và phiếu khám bệnh
		// Success, 201: Thêm thành công phiếu khám bệnh, bệnh nhân đã tồn tại
		// Failed, 300: Thiếu thông tin
		// Failed, 400: Thêm thất bại
		public void AddMedicalExaminationForm()
		{
			if(!isValidData() || SelectedDoctor == null)
			{
				AddCompleted?.Invoke("Please fill in all information!", 300);
				return;
			}
			else
			{
				var (isExist, patientId) = _dao.checkPatientExists(Patient.ResidentId);

				if(isExist)
				{
					bool examinationSaved = _dao.AddMedicalExaminationForm(patientId, MedicalExaminationForm);
					if (examinationSaved)
					{
						AddCompleted?.Invoke("Success", 201);
					}
					else
					{
						AddCompleted?.Invoke("Failed", 400);
					}
				}
				else
				{
					var (isAdded, newPatientId) = _dao.AddPatient(Patient);
					if (isAdded && newPatientId != 0)
					{

						bool examinationSaved = _dao.AddMedicalExaminationForm(newPatientId, MedicalExaminationForm);
						if (examinationSaved)
						{
							AddCompleted?.Invoke("Success", 200);
						}
						else
						{
							AddCompleted?.Invoke("Failed", 400);
						}
					}
					else
					{
						AddCompleted?.Invoke("Failed.", 400);
					}
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

		public void Reset()
		{
			Patient = new Patient();
			MedicalExaminationForm = new MedicalExaminationForm();
			SelectedDoctor = null;
			DoctorNameFilter = null;
			SpecialtyFilter = null;
			LoadDoctors();
		}
	}
}
