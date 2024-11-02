using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Clinic_Management_System.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public Patient Patient { get; set; } = new Patient();
		public MedicalExaminationForm MedicalExaminationForm { get; set; } = new MedicalExaminationForm();

		public event Action<string> AddCompleted;

		public void AddMedicalExaminationForm()
		{
			bool patientSaved = _dao.AddPatient(Patient);
			if (patientSaved)
			{
				bool examinationSaved = _dao.AddMedicalExaminationForm(Patient, MedicalExaminationForm);
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
	}
}
