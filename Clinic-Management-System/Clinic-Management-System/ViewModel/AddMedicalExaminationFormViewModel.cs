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
using System.Net.Mail;
using Clinic_Management_System.Helper;
using static Clinic_Management_System.Service.DataAccess.IDao;

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
				if (_selectedDoctor != null)
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




		public event Action<bool, int, string> AddCompleted;
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public (bool, int, string) isValidData()
		{
			IsValidData isValid = new IsValidData();

			if (!isValid.IsValidName(Patient.Name))
				return (false, 301, "Invalid name.");
			if (!isValid.IsValidEmail(Patient.Email))
				return (false, 302, "Invalid email.");
			if (!isValid.IsValidResedentID(Patient.ResidentId))
				return (false, 303, "Invalid Resident ID.");
			if (!isValid.IsValidAddress(Patient.Address))
				return (false, 304, "Invalid address.");
			if (!isValid.IsValidDatePicker(Patient.DoB))
				return (false, 305, "Invalid date of birth.");
			if (!isValid.IsValidDescription(Patient.Gender))
				return (false, 306, "Invalid gender.");
			if (!isValid.IsValidDescription(MedicalExaminationForm.Symptoms))
				return (false, 307, "Invalid symptoms.");

			return (true, 200, "Valid data.");

		}


		// Success, 200: Thêm thành công bệnh nhân và phiếu khám bệnh
		// Success, 201: Thêm thành công phiếu khám bệnh, bệnh nhân đã tồn tại
		// Failed, 301: Name không hợp lệ
		// Failed, 302: Email không hợp lệ
		// Failed, 303: ResidentID không hợp lệ
		// Failed, 304: Address không hợp lệ
		// Failed, 305: DoB không hợp lệ
		// Failed, 306: Gender không hợp lệ
		// Failed, 307: Symptoms không hợp lệ
		// Failed, 308: Chua chọn Doctor
		// Failed, 400: Thêm thất bại
		public void AddMedicalExaminationForm()
		{

			var (isValid, code, message) = isValidData();

			if (!isValid)
			{
				AddCompleted?.Invoke(false, code, message);
				return;
			}

			if (SelectedDoctor == null)
			{
				AddCompleted?.Invoke(false, 308, "Doctor not selected.");
				return;
			}


			var (isExist, patientId) = _dao.checkPatientExists(Patient.ResidentId);

			if (isExist)
			{
				bool examinationSaved = _dao.AddMedicalExaminationForm(patientId, MedicalExaminationForm);
				if (examinationSaved)
				{
					AddCompleted?.Invoke(true, 201, "Medical examination form added successfully. Medical examination form added for existing patient.");
				}
				else
				{
					AddCompleted?.Invoke(false, 400, "Failed to add medical examination form for existing patient.");
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
						AddCompleted?.Invoke(true, 200, "Patient and medical examination form added successfully.");
					}
					else
					{
						AddCompleted?.Invoke(false, 400, "Failed to add medical examination form for new patient.");
					}
				}
				else
				{
					AddCompleted?.Invoke(false, 400, "Failed to add new patient.");
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
		//==================================List MedicalExaminationForm===============================
		
	}
}
