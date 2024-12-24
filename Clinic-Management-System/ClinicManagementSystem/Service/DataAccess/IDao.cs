using ClinicManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Service.DataAccess
{
    public interface IDao
    {
        public enum SortType
        {
            Ascending,
            Descending
        }

		//============================================Helper=====================================
		(int, string, string, string, string, string) Authentication(string username, string password);
		//=======================================================================================



		//========================================EndUser========================================
		Tuple<List<User>, int> GetUsers(
			int page, int rowsPerPage,
			string keyword,
			Dictionary<string, SortType> sortOptions
		);

		bool CreateUser(User user);

		bool CreateUserRoleDoctor(User user, int specialty, string room);

		bool CheckUserExists(string username);

		bool UpdateUser(User info);

		bool DeleteUser(User user);
		
		public List<Specialty> GetSpecialty();
		Tuple<List<Medicine>, int> GetMedicines(
		 int page, int rowsPerPage,
		 string keyword,
		 Dictionary<string, SortType> sortOptions
		);
		//========================================================================================



		//========================================Doctor==========================================
		public List<Doctor> GetInforDoctor();
		//========================================================================================



		//========================================MedicalExaminationForm==========================
		bool AddMedicalExaminationForm(int patientId, MedicalExaminationForm medicalExaminationForm);

		public Tuple<List<MedicalExaminationForm>, int> GetMedicalExaminationForms(
			int page,
			int rowsPerPage,
			string keyword,
			DateTimeOffset? startDate,
			DateTimeOffset? endDate,
			Dictionary<string, SortType> sortOptions);

		bool UpdateMedicalExaminationForm(MedicalExaminationForm form);

		bool DeleteMedicalExaminationForm(MedicalExaminationForm form);
		//========================================================================================



		//========================================Medicine========================================
		bool CreateMedicine(Medicine medicine);
		bool UpdateMedicine(Medicine medicine);
		bool DeleteMedicine(Medicine medicine);
		List<MedicineStatistic> GetMedicineStatistic(DateTime startDate, DateTime endDate);
		//========================================================================================

		//========================================Specialty=======================================
		//========================================================================================



		//========================================Patient=========================================
		(bool, int) AddPatient(Patient patient);

		(bool, int) checkPatientExists(string residentId);

		Tuple<List<Patient>, int> GetPatients(
			int page,
			int rowsPerPage,
			string keyword,
			Dictionary<string, SortType> sortOptions
		);

		bool UpdatePatient(Patient patient);

		bool DeletePatient(Patient patient);
		//========================================================================================

    }
}
