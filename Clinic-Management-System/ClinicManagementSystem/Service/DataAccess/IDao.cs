using ClinicManagementSystem.Model;
using ClinicManagementSystem.Model.Statistic;
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
		bool LockUser(int id,string status);
		Tuple<List<Medicine>, int> GetMedicines(
		 int page, int rowsPerPage,
		 string keyword,
		 Dictionary<string, SortType> sortOptions, int daysRemaining
        );
		User GetUserById(int userId);
		public int GetTotalUsersCount();
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
			string statusFilter,
			Dictionary<string, SortType> sortOptions);

		bool UpdateMedicalExaminationForm(MedicalExaminationForm form);

		bool DeleteMedicalExaminationForm(MedicalExaminationForm form);

		public MedicalExaminationFormDetail GetMedicalExaminationFormDetail(int formId);
		
		public List<MedicalExaminationStatistic> GetMedicalExaminationStatisticsByDate(DateTimeOffset startDate, DateTimeOffset endDate);

		public int GetTodayMedicalExaminationFormsCount();

		public int GetTotalPatientsCount();

		public int GetTodayNewPatientsCount();

		public int GetPendingFormsCount();

		public (List<MedicalExaminationForm>, int) GetDoctorExaminationForms(
			int doctorId,
			int currentPage,
			int pageSize,
			string isExaminated,
			string keyword = "",
			DateTimeOffset? startDate = null,
			DateTimeOffset? endDate = null
		);
		public MedicalExaminationForm GetMedicalExaminationFormById(int id);

		public int GetTodayFormsByDoctorId(int id);

		public int GetTodayCompletedFormsByDoctorId(int id);

		//========================================================================================



		//========================================Medicine========================================
		bool CreateMedicine(Medicine medicine);
		bool UpdateMedicine(Medicine medicine);
		bool DeleteMedicine(Medicine medicine);
		List<MedicineStatistic> GetTopMedicineStatistic(DateTimeOffset startDate, DateTimeOffset endDate, int n, string sortString);
		List<MedicineStatistic> GetMedicineStatistic(DateTimeOffset startDate, DateTimeOffset endDate);
        public List<MedicineSelection> GetMedicineSelectionsByFormId(int formId);
		public (List<MedicineSelection>, int) GetMedicinesByPage(
            int currentPage, 
            int pageSize, 
            string keyword = ""
        );
		public void UpdateMedicineQuantity(int medicineId, int quantityChange);
		//========================================================================================

        //========================================Specialty=======================================
        public List<Specialty> GetSpecialty();
		public (bool success, int specialtyId) CreateSpecialty(string specialty);
        //========================================================================================



        //========================================Patient=========================================
        (bool, int) AddPatient(Patient patient);

		(bool, int) checkPatientExists(string residentId);

		Tuple<List<Patient>, int> GetPatients(
			int page,
			int rowsPerPage,
			string keyword,
			DateTimeOffset? startDate,
			DateTimeOffset? endDate,
			Dictionary<string, SortType> sortOptions
		);

		public int GetMonthlyPatientCountByDoctorId(int id);

		bool UpdatePatient(Patient patient);

		bool DeletePatient(Patient patient);

		public Patient GetPatientById(int patientId);
        //========================================================================================

		//========================================Prescription=========================================
		public Prescription GetPrescriptionById(int id);
		public (List<Prescription>, int) GetPrescriptionsByPage(
			int page,
			int pageSize,
			string isBilled,
			string keyword = ""
		);
		//========================================================================================

        //=========================================Bill==========================================
        public List<BillStatistic> GetBillStatistic(DateTimeOffset startDate, DateTimeOffset endDate);
		public (List<Bill>, int) GetBillsByPage(
			int currentPage, 
			int pageSize, 
			string keyword = "", 
			DateTimeOffset? startDate = null, 
			DateTimeOffset? endDate = null,
            string status = ""
		);
		public Bill GetBillById(int id);
		public bool SaveBill(Bill bill);
		public bool UpdatePrescriptionBillStatus(int prescriptionId, string isBilled);

		public int GetMonthlyPrescriptionCountByDoctorId(int id);


		//=========================================Bill==========================================
		public List<BillStatistic> GetBillStatistic(DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
