using ClinicManagementSystem.Model;
using ClinicManagementSystem.Service;
using ClinicManagementSystem.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using ClinicManagementSystem.Helper;
using static ClinicManagementSystem.Service.DataAccess.IDao;

namespace ClinicManagementSystem.ViewModel
{
	/// <summary>
	/// ViewModel cho AddMedicalExaminationForm
	/// </summary>
	public class AddMedicalExaminationFormViewModel : INotifyPropertyChanged
	{
		IDao _dao;


		public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();
		private string _keyword;
		public string Keyword
		{
			get { return _keyword; }
			set
			{
				_keyword = value;
				OnPropertyChanged(nameof(Keyword));
				LoadDoctors(_keyword);
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

		private DateTimeOffset _selectedDate = DateTimeOffset.Now;
		public DateTimeOffset SelectedDate
		{
			get => _selectedDate;
			set
			{
				_selectedDate = value;
				Patient.DoB = value; 
				OnPropertyChanged(nameof(SelectedDate));
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

		/// <summary>
		/// Kiểm tra dữ liệu nhập vào có hợp lệ không
		/// </summary>
		/// <returns>
		///		Biến bool: Cho biết thành công
		///		Biến int: Mã lỗi
		///		Biến string: Thông báo
		/// </returns>
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
			if (!isValid.IsValidDescription(MedicalExaminationForm.VisitType))
				return (false, 309, "Invalid visit type.");

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
		// Failed, 308: Chưa chọn Doctor
		// Failed, 309: Chưa chọn Visit Type
		// Failed, 400: Thêm thất bại

		/// <summary>
		/// Thêm phiếu khám bệnh
		/// </summary>
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
				Patient.Id = patientId;
				bool patientUpdated = _dao.UpdatePatient(Patient);
				bool examinationSaved = _dao.AddMedicalExaminationForm(patientId, MedicalExaminationForm);
				if (examinationSaved)
				{
					AddCompleted?.Invoke(true, 201, "Medical Examination Form added successfully.\nMedical Examination Form added for existing patient.");
				}
				else
				{
					AddCompleted?.Invoke(false, 400, "Failed to add Medical Examination Form for existing patient.");
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
						AddCompleted?.Invoke(true, 200, "Patient and Medical Examination Form added successfully.");
					}
					else
					{
						AddCompleted?.Invoke(false, 400, "Failed to add Medical Examination Form for new patient.");
					}
				}
				else
				{
					AddCompleted?.Invoke(false, 400, "Failed to add new patient.");
				}
			}
		}
		/// <summary>
		/// Load danh sách bác sĩ
		/// </summary>
		/// <param name="keyword"></param>
		public void LoadDoctors(string keyword = null)
		{
			var doctors = _dao.GetInforDoctor();
			Doctors.Clear();

			foreach (var doctor in doctors)
			{
				if (string.IsNullOrEmpty(keyword) ||
					doctor.name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
					doctor.SpecialtyName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
				{
					Doctors.Add(doctor);
				}
			}
			OnPropertyChanged(nameof(Doctors));
		}

		/// <summary>
		/// Reset dữ liệu
		/// </summary>
		public void Reset()
		{
			Patient = new Patient();
			MedicalExaminationForm = new MedicalExaminationForm();
			SelectedDoctor = null;
			Keyword = string.Empty;
			SelectedDate = DateTimeOffset.Now;
			LoadDoctors();
		}
	}
}
