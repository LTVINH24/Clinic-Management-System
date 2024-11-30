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
		(bool, int) AddPatient(Patient patient);
		bool AddMedicalExaminationForm(int patientId, MedicalExaminationForm medicalExaminationForm);
		public List<Doctor> GetInforDoctor();
		(bool, int) checkPatientExists(string residentId);
		public Tuple<List<MedicalExaminationForm>, int> GetMedicalExaminationForm(
			int page,
			int rowsPerPage,
			string keyword,
			Dictionary<string, SortType> sortOptions);

        Tuple<List<User>, int> GetUsers(
            int page, int rowsPerPage,
            string keyword,
            Dictionary<string, SortType> sortOptions
        );
        (int, string, string, string, string, string) Authentication (string username, string password);
        bool CreateUser( User user,string encryptedPasswordInBase64,string entropyInBase64);
        bool CreateUserRoleDoctor(User user, string encryptedPasswordInBase64, string entropyInBase64, int specialty, string room);

        bool CheckUserExists(string username);
        bool UpdateUser(User info,string entropyUserEdit);
        bool DeleteUser(User user);
        public List<Specialty> GetSpecialty();

    }
}
