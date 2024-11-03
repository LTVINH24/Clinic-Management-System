using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Collections.Generic;
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


		public AddMedicalExaminationFormViewModel()
        {
			_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
			LoadDoctors();
		}

        public Patient Patient { get; set; } = new Patient();
		public MedicalExaminationForm MedicalExaminationForm { get; set; } = new MedicalExaminationForm();

		public List<Doctor> Doctors { get; set; } = new List<Doctor>();
		public Doctor SelectedDoctor { get; set; }

		public event Action<string> AddCompleted;

		public void AddMedicalExaminationForm()
		{
			var (patientSaved, patientId) = _dao.AddPatient(Patient);
			if (patientSaved && patientId != 0)
			{
				MedicalExaminationForm.DoctorId = SelectedDoctor.Id;
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
			var (doctors, _) = _dao.GetInforDoctor();
			Doctors = doctors;
		}

		
	}
}
