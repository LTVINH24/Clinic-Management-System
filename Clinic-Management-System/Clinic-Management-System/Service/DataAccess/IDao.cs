using Clinic_Management_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Service.DataAccess
{
    public interface IDao
    {
        public enum SortType
        {
            Ascending,
            Descending
        }
		//Example
		//Tuple<List<User>, int> GetEmployees(
		//    int page, int rowsPerPage,
		//    string keyword,
		//    Dictionary<string, SortType> sortOptions
		//); 

		//bool DeleteEmployee(int id);
		//bool AddEmployee(User info);

		//bool UpdateEmployee(User info);
		(int, string) Authentication(string username, string password);
		(bool, int) AddPatient(Patient patient);
		bool AddMedicalExaminationForm(int patientId, MedicalExaminationForm medicalExaminationForm);
		public List<Doctor> GetInforDoctor();
	}
}
