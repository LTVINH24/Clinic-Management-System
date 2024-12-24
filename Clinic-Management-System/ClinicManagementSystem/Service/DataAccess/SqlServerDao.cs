using ClinicManagementSystem.Model;
using ClinicManagementSystem.Model.Statistic;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ClinicManagementSystem.Service.DataAccess.IDao;

namespace ClinicManagementSystem.Service.DataAccess
{
    public class SqlServerDao : IDao
    {
		/// <summary>
		/// Lấy chuỗi kết nối database
		/// </summary>
		/// <returns>Chuỗi kết nối</returns>
		private static string GetConnectionString()
        {
            var connectionString = """
                    Server = localhost,1433;
                    Database = ClinicManagementSystemDatabase;
                    User Id = sa;
                    Password = SqlServer@123;
                    TrustServerCertificate = True;
                """;
            return connectionString;
        }
        private readonly string _connectionString = GetConnectionString();



		//==============================================Helper===========================================
		/// <summary>
		/// Xứ lí xác thực người dùng
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>Thông tin người dùng</returns>
		public (int, string, string, string, string, string) Authentication(string username, string password)
        {

            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT id,name, role,password,phone,birthday,gender,address FROM EndUser WHERE username = @Username";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            var reader = command.ExecuteReader();
            int id = 0;
            string name = "";
            string role = "";
            string hassPassword = "";
            string phone = "";
            DateTime birthday;
            string gender;
            string address;
            var Password = new Password();
            if (reader.Read())
            {
                id = (int)reader["id"];
                name = (string)reader["name"];
                role = reader["role"].ToString();
                hassPassword = reader["password"].ToString();
                phone = reader["phone"].ToString();
                birthday = (DateTime)reader["birthday"];
                gender = reader["gender"].ToString();
                address = reader["address"].ToString();
                connection.Close();
                if (Password.VerifyPassword(password, hassPassword))
                {
                    return (id, name, role, phone, gender, address);
                }
                return (0, "", "", "", "", "");
            }
            else
            {
                connection.Close();
                return (0, "", "", "", "", "");
            }
            //return (0, "", "admin", "", "", "");
        }

		/// <summary>
		/// Thêm các tham số vào câu lệnh sql
		/// </summary>
		/// <param name="command"></param>
		/// <param name="parameters"></param>
		private void AddParameters(SqlCommand command, params (string ParameterName, object Value)[] parameters)
        {
            foreach (var (parameterName, value) in parameters)
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

		//===============================================================================================



		//================================================EndUser========================================
		/// <summary>
		/// Lấy danh sách người dùng
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rowsPerPage"></param>
		/// <param name="keyword"></param>
		/// <param name="sortOptions"></param>
		/// <returns>Danh sách người dùng</returns>
		public Tuple<List<User>, int> GetUsers(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions)
        {
            var result = new List<User>();
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sortString = "ORDER BY ";
            bool useDefault = true;
            foreach (var item in sortOptions)
            {
                useDefault = false;
                if (item.Key == "Name")
                {
                    if (item.Value == SortType.Ascending)
                    {
                        sortString += "Name asc ";
                    }
                    else
                    {
                        sortString += "Name desc ";
                    }
                }
            }
            if (useDefault)
            {
                sortString += "ID ";
            }


            var sql = $"""
            SELECT count(*) over() as Total, id, name, role, username,password,phone,birthday,address,gender
            FROM EndUser
            WHERE Name like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            var reader = command.ExecuteReader();
            int count = -1;
            while (reader.Read())
            {
                if (count == -1)
                {
                    count = (int)reader["Total"];
                }
                var user = new User();
                user.id = (int)reader["id"];
                user.name = (string)reader["name"];
                user.username = (string)reader["username"];
                user.gender = (string)reader["gender"];
                user.role = (string)reader["role"];
                user.birthday = (DateTime)reader["birthday"];
                user.password = (string)reader["password"];
                user.address = (string)reader["address"];
                user.phone = (string)reader["phone"];
                result.Add(user);
            }
            connection.Close();
            return new Tuple<List<User>, int>(
                result, count
            );
        }

		/// <summary>
		/// Kiểm tra người dùng tồn tại
		/// </summary>
		/// <param name="username"></param>
		/// <returns>True nếu người dùng tồn tại, False nếu người dùng không tồn tại</returns>
		public bool CheckUserExists(string username)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT count(*) FROM EndUser WHERE username = @Username";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
               ("@Username", username)
               );
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

		/// <summary>
		/// Tạo người dùng
		/// </summary>
		/// <param name="user"></param>
		/// <returns>True nếu tạo người dùng thành công</returns>
		public bool CreateUser(User user)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"insert into EndUser(name,role,username,password,phone,birthday,address,gender) values(@name,@role,@username,@password,@phone,@birthday,@address,@gender) ";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@name", user.name),
                ("@role", user.role),
                ("@username", user.username),
                ("@password", user.password),
                ("@phone", user.phone),
                ("@birthday", user.birthday),
                ("@address", user.address),
                ("@gender", user.gender)
                );

            int count = command.ExecuteNonQuery();
            bool success = count == 1;
            connection.Close();
            return success;
        }

		/// <summary>
		/// Tạo người dùng là bác sĩ
		/// </summary>
		/// <param name="user"></param>
		/// <param name="specialty"></param>
		/// <param name="room"></param>
		/// <returns>Trả về true nếu tạo thành công</returns>
		public bool CreateUserRoleDoctor(User user, int specialty, string room)
        {
            bool success = true;
            success = CreateUser(user);

            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string queryGetId = $"SELECT id FROM EndUser WHERE username = @Username";
            var commandGetId = new SqlCommand(queryGetId, connection);
            AddParameters(commandGetId, ("@Username", user.username));
            int userId = (int)commandGetId.ExecuteScalar();
            string query = $"insert into Doctor(userId,specialtyId,room) values(@userId,@specialtyId,@room) ";
            var command = new SqlCommand(query, connection);
            AddParameters(command, ("@userId", userId), ("@specialtyId", specialty), ("@room", room));
            int count = command.ExecuteNonQuery();
            success = success && (count == 1);
            connection.Close();
            return success;
        }

		/// <summary>
		/// Cập nhật thông tin người dùng
		/// </summary>
		/// <param name="info"></param>
		/// <returns>Trả về true nếu cập nhật thành công</returns>
		public bool UpdateUser(User info)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var sql = "update EndUser set name=@name, role=@role,username=@username,password=@password,phone=@phone,birthday=@birthday,address=@address,gender=@gender where id=@id";
            var command = new SqlCommand(sql, connection);
            AddParameters(command,
                ("@id", info.id),
                ("@name", info.name),
                ("@role", info.role),
                ("@username", info.username),
                ("@password", info.password),
                ("@phone", info.phone),
                ("@birthday", info.birthday),
                ("@address", info.address),
                ("@gender", info.gender)
                );
            int count = command.ExecuteNonQuery();
            bool success = count == 1;
            connection.Close();
            return success;
        }

		/// <summary>
		/// Xóa người dùng
		/// </summary>
		/// <param name="user"></param>
		/// <returns>Trả về true nếu xóa thành công</returns>
		public bool DeleteUser(User user)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            int result = 0;
            if (user.role == "doctor")
            {
                string query = $"Delete from Doctor where userId =@userId ";
                var command = new SqlCommand(query, connection);
                AddParameters(command, ("@userId", user.id));
                result = command.ExecuteNonQuery();
            }
            string querydelete = $"Delete from EndUser where id =@userId";
            var commandDelete = new SqlCommand(querydelete, connection);
            AddParameters(commandDelete, ("@userId", user.id));
            result = commandDelete.ExecuteNonQuery();
            connection.Close();
            return result > 0;
        }
		//=============================================================================================



		//================================================Specialty========================================
		/// <summary>
		/// Lấy danh sách chuyên khoa
		/// </summary>
		/// <returns>Danh sách khoa</returns>
		public List<Specialty> GetSpecialty()
        {
            var specialties = new List<Specialty>();
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT id,name FROM  Specialty";
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var specialty = new Specialty();
                specialty.id = (int)reader["id"];
                specialty.name = (string)reader["name"];
                specialties.Add(specialty);
            }
            connection.Close();
            return specialties;
        }
		//=============================================================================================



		//================================================Medicine========================================
		/// <summary>
		/// Lấy danh sách thuốc
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rowsPerPage"></param>
		/// <param name="keyword"></param>
		/// <param name="sortOptions"></param>
		/// <returns>Danh sách thuốc và số lượng thuốc</returns>
		public Tuple<List<Medicine>, int> GetMedicines(
                int page, int rowsPerPage,
                string keyword,
                Dictionary<string, SortType> sortOptions)
        {
            var result = new List<Medicine>();
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sortString = "ORDER BY ";
            bool useDefault = true;
            foreach (var item in sortOptions)
            {
                useDefault = false;
                if (item.Key == "Name")
                {
                    if (item.Value == SortType.Ascending)
                    {
                        sortString += "Name asc ";
                    }
                    else
                    {
                        sortString += "Name desc ";
                    }
                }
            }
            if (useDefault)
            {
                sortString += "ID ";
            }


            var sql = $"""
            SELECT count(*) over() as Total, id, name, manufacturer, price,quantity,quantityimport
            FROM Medicine
            WHERE Name like @Keyword and isDeleted != 'true'
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            var reader = command.ExecuteReader();
            int count = -1;
            while (reader.Read())
            {
                if (count == -1)
                {
                    count = (int)reader["Total"];
                }
                var medicine = new Medicine();
                medicine.Id = (int)reader["id"];
                medicine.Name = (string)reader["name"];
                medicine.Manufacturer = (string)reader["manufacturer"];
                medicine.Price = (int)reader["price"];
                medicine.Quantity = (int)reader["quantity"];
                medicine.QuantityImport = (int)reader["quantityimport"];

                result.Add(medicine);
            }
            connection.Close();
            return new Tuple<List<Medicine>, int>(
                result, count
            );
        }

		/// <summary>
		/// Tạo thuốc
		/// </summary>
		/// <param name="medicine"></param>
		/// <returns>True nếu tạo thành công, False nếu tạo thất bại</returns>
		public bool CreateMedicine(Medicine medicine)
        {
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"insert into Medicine(name,manufacturer,price,quantity, quantityimport,DateImport ,ExpDate,MfgDate) values(@name,@manufacturer,@price,@quantity,@quantityimport,@DateImport,@ExpDate,@MfgDate)";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@name", medicine.Name),
                ("@manufacturer", medicine.Manufacturer),
                ("@price", medicine.Price),
                ("@quantity", medicine.QuantityImport),
                ("@quantityimport", medicine.QuantityImport),
                ("@ExpDate", medicine.ExpDate),
                ("@MfgDate", medicine.MfgDate),
                ("@DateImport",medicine.DateImport));
            int count = command.ExecuteNonQuery();
            connection.Close();
            return count == 1;
        }

		/// <summary>
		/// Cập nhật thông tin thuốc
		/// </summary>
		/// <param name="medicine"></param>
		/// <returns>True nếu update thành công, False nếu update thất bại</returns>
		public bool UpdateMedicine(Medicine medicine)
        {
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"update Medicine set name=@name, manufacturer =@manufacturer, price =@price, quantity =@quantity, quantityimport=@quantityimport, ExpDate =@expdate ,MfgDate =@mfgdate where id =@id";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@name", medicine.Name),
                ("@manufacturer", medicine.Manufacturer),
                ("@price", medicine.Price),
                ("@quantity", medicine.Quantity),
                ("@quantityimport", medicine.QuantityImport),
                ("@ExpDate", medicine.ExpDate),
                ("@MfgDate", medicine.MfgDate),
                ("@id", medicine.Id)
                );
            int count = command.ExecuteNonQuery();
            connection.Close();
            return count == 1;
        }

		/// <summary>
		/// Xóa thuốc
		/// </summary>
		/// <param name="medicine"></param>
		/// <returns>True nếu xóa thành công, False nếu xóa thất bại</returns>
		public bool DeleteMedicine(Medicine medicine)
        {
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"update Medicine set isDeleted = 'true' where id=@id";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@id", medicine.Id));
            int count = command.ExecuteNonQuery();
            connection.Close();
            return count > 0;
        }

		/// <summary>
		/// Cập nhật số lượng thuốc
		/// </summary>
		/// <param name="medicineId"></param>
		/// <param name="quantityChange"></param>
		public void UpdateMedicineQuantity(int medicineId, int quantityChange)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Medicine SET Quantity = Quantity + @QuantityChange WHERE Id = @MedicineId AND Quantity >= 0";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@QuantityChange", quantityChange);
                command.Parameters.AddWithValue("@MedicineId", medicineId);
                command.ExecuteNonQuery();
            }
        }
        public List<MedicineStatistic> GetMedicineStatistic(DateTimeOffset startDate, DateTimeOffset endDate, int n, string sortString)
        {
            var result = new List<MedicineStatistic>();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = $"""
                    SELECT TOP(@n) 
                    m.name as MedicineName, 
                    ISNULL(SUM(pd.quantity), 0) as QuantitySold, 
                    ISNULL(SUM(pd.quantity * m.price), 0) as MoneySold
                FROM Medicine m 
                JOIN PrescriptionDetail pd ON m.id = pd.medicineId
                JOIN Prescription p ON pd.prescriptionId = p.id
                JOIN Bill b ON p.id = b.prescriptionId
                WHERE p.time BETWEEN @startDate AND @endDate
                GROUP BY m.id, m.name
                order by {sortString} desc
                """;

                    using (var command = new SqlCommand(query, connection))
                    {
                        AddParameters(command, ("@startDate", startDate), ("@endDate", endDate), ("@n", n));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new MedicineStatistic
                                {
                                    MedicineName = reader["MedicineName"].ToString(),
                                    QuantitySold = Convert.ToInt32(reader["QuantitySold"]),
                                    Money = Convert.ToInt32(reader["MoneySold"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in GetMedicineStatistic: {ex.Message}");
                    throw;
                }
            }

            return result;
        }
        /// <summary>
        /// Lấy danh sách thuốc còn trong kho
        /// </summary>
        /// <returns>Danh sách thuốc</returns>
        /// 
        public List<Medicine> GetAvailableMedicines()
        {
            var medicines = new List<Medicine>();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var command = new SqlCommand("SELECT Id, Name, Manufacturer, Price, Quantity, ExpDate, MfgDate FROM Medicine", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var medicine = new Medicine
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetInt32(3),
                            Quantity = reader.GetInt32(4),
                            ExpDate = reader.GetDateTime(5),
                            MfgDate = reader.GetDateTime(6)
                        };
                        medicines.Add(medicine);
                    }
                }
            }

            return medicines;
        }

		//=============================================================================================



		//=============================================MedicalExaminationForm==========================
		/// <summary>
		/// Lấy danh sách phiếu khám bệnh
		/// </summary>
		/// <returns>Danh sách phiếu khám bệnh</returns>
		public List<MedicalExaminationForm> GetMedicalExaminationForms()
        {
            var forms = new List<MedicalExaminationForm>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, PatientId, StaffId, Time, Symptom, DoctorId FROM MedicalExaminationForm", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var form = new MedicalExaminationForm
                        {
                            Id = reader.GetInt32(0),                                                    // Id
                            PatientId = reader.GetInt32(1),                                             // PatientId
                            StaffId = reader.GetInt32(2),                                               // StaffId
                            Time = reader.GetDateTime(3),                                               // Time
                            Symptoms = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),          // Symptom
                            DoctorId = reader.GetInt32(5)                                               // DoctorId
                        };

                        forms.Add(form);
                    }
                }
            }

            return forms;
        }
		/// <summary>
		/// Lấy danh sách phiếu khám bệnh
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rowsPerPage"></param>
		/// <param name="keyword"></param>
		/// <param name="sortOptions"></param>
		/// <returns>Danh sách phiếu khám bệnh và số lượng phiếu khám bệnh</returns>
		public Tuple<List<MedicalExaminationForm>, int> GetMedicalExaminationForm(
            int page,
            int rowsPerPage,
            string keyword,
            Dictionary<string, SortType> sortOptions)
        {
            var result = new List<MedicalExaminationForm>();
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sortString = "ORDER BY ";
            bool useDefault = true;

            foreach (var item in sortOptions)
            {
                useDefault = false;
                if (item.Key == "patientId")
                {
                    if (item.Value == SortType.Ascending)
                    {
                        sortString += "patientId ASC, ";
                    }
                    else
                    {
                        sortString += "patientId DESC, ";
                    }
                }
            }

            if (useDefault)
            {
                sortString += "ID ";
            }

            var sql = $"""
				SELECT count(*) over() as Total, id, patientId, staffId, time, symptom, doctorId, visitType
				FROM MedicalExaminationForm
				WHERE symptom like @Keyword
				{sortString}
				OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
				""";

            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            var reader = command.ExecuteReader();
            int count = -1;

            while (reader.Read())
            {
                if (count == -1)
                {
                    count = (int)reader["Total"];
                }

                var medicalExaminationForm = new MedicalExaminationForm();
                medicalExaminationForm.Id = (int)reader["id"];
                medicalExaminationForm.PatientId = (int)reader["patientId"];
                medicalExaminationForm.StaffId = (int)reader["staffId"];
                medicalExaminationForm.Time = (DateTime)reader["time"];
                medicalExaminationForm.Symptoms = (string)reader["symptom"];
                medicalExaminationForm.DoctorId = (int)reader["doctorId"];
                medicalExaminationForm.VisitType = (string)reader["visitType"];

                result.Add(medicalExaminationForm);
            }
            connection.Close();
            return new Tuple<List<MedicalExaminationForm>, int>(result, count);
        }
        public List<MedicalExaminationStatistic> GetMedicalExaminationStatisticsByDate(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<MedicalExaminationStatistic>();
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"""
                select CONVERT(date, m.time) as Date, count(*) as total
                from MedicalExaminationForm m
                where time between @startDate and @endDate
                group by CONVERT(date, m.time)
                """;
            var command = new SqlCommand(query, connection);
            AddParameters(command, ("@startDate", startDate), ("@endDate", endDate));
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var MedicalExStatistic = new MedicalExaminationStatistic();
                MedicalExStatistic.date =(DateTime) reader["Date"];
                MedicalExStatistic.amount = (int)reader["total"];
                result.Add(MedicalExStatistic);
            }
            connection.Close();
            return result;
        }
        /// <summary>
        /// Thêm bệnh nhân
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>True và id bệnh nhân nếu tạo thành công, False và id bằng 0 nếu tạo thất bại</returns>
        public (bool, int) AddPatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();



            var command = new SqlCommand("INSERT INTO Patient (Name , Email , ResidentId, Address , Birthday , Gender) " +
                "VALUES (@Name , @Email , @ResidentId , @Address , @DoB , @Gender)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int);", connection);
            AddParameters(command,
                ("@Name", patient.Name),
                ("@Email", patient.Email),
                ("@ResidentId", patient.ResidentId),
                ("@Address", patient.Address),
                ("@DoB", patient.DoB),
                ("@Gender", patient.Gender));

            var result = command.ExecuteScalar();
            int id = result != null ? Convert.ToInt32(result) : 0;

            connection.Close();
            return (id > 0, id);
        }

		/// <summary>
		/// Kiểm tra bệnh nhân tồn tại
		/// </summary>
		/// <param name="residentId"></param>
		/// <returns>True và id nếu bệnh nhân tồn tại, False và 0 nếu bệnh nhân không tồn tại</returns>
		public (bool, int) checkPatientExists(string residentId)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "SELECT id FROM Patient WHERE ResidentId = @ResidentId";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@ResidentId", residentId));


            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int id = (int)reader["id"];
                connection.Close();
                return (true, id);
            }

            connection.Close();
            return (false, 0);

        }

		/// <summary>
		/// Thêm phiếu khám bệnh
		/// </summary>
		/// <param name="patientId"></param>
		/// <param name="medicalExaminationForm"></param>
		/// <returns>True nếu thêm thành công, False nếu thêm thất bại</returns>
		public bool AddMedicalExaminationForm(int patientId, MedicalExaminationForm medicalExaminationForm)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            DateTime currentDate = DateTime.Now;
            string formatDate = currentDate.ToString("yyyy-MM-dd");
            int id = UserSessionService.Instance.LoggedInUserId;


            var command = new SqlCommand("INSERT INTO MedicalExaminationForm (PatientId, StaffId, DoctorId, Time, Symptom, VisitType) VALUES (@PatientId, @StaffId, @DoctorId, @Time, @Symptom, @VisitType)", connection);



            AddParameters(command,
                ("@PatientId", patientId),
                ("@StaffId", id),
                ("@DoctorId", medicalExaminationForm.DoctorId),
                ("@Time", formatDate),
                ("@Symptom", medicalExaminationForm.Symptoms),
                ("@VisitType", medicalExaminationForm.VisitType));

            int result = command.ExecuteNonQuery();

            connection.Close();
            return result > 0;
        }

		/// <summary>
		/// Lấy thông tin phiếu khám bệnh theo id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Phiếu khám bệnh</returns>
		public MedicalExaminationForm GetMedicalExaminationFormById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM MedicalExaminationForm WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MedicalExaminationForm
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            PatientId = reader.GetInt32(reader.GetOrdinal("patientId")),
                            StaffId = reader.GetInt32(reader.GetOrdinal("staffId")),
                            Time = reader.GetDateTime(reader.GetOrdinal("time")),
                            Symptoms = reader.IsDBNull(reader.GetOrdinal("symptom")) ? (string)null : reader.GetString(reader.GetOrdinal("symptom")),
                            DoctorId = reader.GetInt32(reader.GetOrdinal("doctorId"))
                        };
                    }
                }
            }
            return null;
        }

		/// <summary>
		/// Cập nhật thông tin phiếu khám bệnh
		/// </summary>
		/// <param name="form"></param>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool UpdateMedicalExaminationForm(MedicalExaminationForm form)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

			var time = DateTime.Now;
			var sql = "update MedicalExaminationForm set " +
				"patientId=@patientId, " +
				"doctorId=@doctorId, " +
				"Time=@time, " +
				"symptom=@symptom, " +
                "visitType=@visitType " +
				"where id=@Id";

			var command = new SqlCommand(sql, connection);
			AddParameters(command,
				("@Id", form.Id),
				("@patientId", form.PatientId),
				("@doctorId", form.DoctorId),
				("@time", form.Time),
				("@symptom", form.Symptoms),
                ("@visitType", form.VisitType));

            int count = command.ExecuteNonQuery();
            bool success = count == 1;

            connection.Close();
            return success;
        }

		/// <summary>
		/// Xóa phiếu khám bệnh
		/// </summary>
		/// <param name="form"></param>
		/// <returns>True nếu xóa thành công, False nếu xóa thất bạu</returns>
		public bool DeleteMedicalExaminationForm(MedicalExaminationForm form)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Delete Bill
                    var deleteBillSql = @"DELETE FROM Bill
                                          WHERE prescriptionId IN (
                                               SELECT Id
                                               FROM Prescription
                                               WHERE MedicalExaminationFormId = @Id
                                          )";
                    var deleteBillCommand = new SqlCommand(deleteBillSql, connection, transaction);
                    AddParameters(deleteBillCommand, ("@Id", form.Id));
                    deleteBillCommand.ExecuteNonQuery();

					// Delete Prescripstion
					var deletePrescriptionSql = @"DELETE FROM Prescription
                                                  WHERE MedicalExaminationFormId = @Id
                                                  ";
					var deletePrescriptionCommand = new SqlCommand(deletePrescriptionSql, connection, transaction);
					AddParameters(deletePrescriptionCommand, ("@Id", form.Id));
					deletePrescriptionCommand.ExecuteNonQuery();

					// Delete MedicalExaminationForm
					var deleteMedicalExaminationFormSql = @"DELETE FROM MedicalExaminationForm
                                                            WHERE Id = @Id";
                    var deleteMedicalExaminationFormCommand = new SqlCommand(deleteMedicalExaminationFormSql, connection, transaction);
                    AddParameters(deleteMedicalExaminationFormCommand, ("@Id", form.Id));
                    int count = deleteMedicalExaminationFormCommand.ExecuteNonQuery();

                    // Commit transaction if successful
                    transaction.Commit();
                    return count == 1;
                }
                catch
                {
                    // Rollback
                    transaction.Rollback();
                    throw;
                }

            }
        }
		//==============================================================================================



		//================================================Doctor========================================
		/// <summary>
		/// Lấy danh sách bác sĩ
		/// </summary>
		/// <returns>Danh sách các bác sĩ</returns>
		public List<Doctor> GetInforDoctor()
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query1 = $"SELECT id, role FROM EndUser WHERE username = @Username AND password = @Password";
            var command1 = new SqlCommand(query1, connection);

            var query = $"""
							SELECT e.id, e.name as doctorName, s.id as specialtyId, s.name as specialtyName, d.room 
							FROM EndUser e JOIN Doctor d ON e.id = d.userId
								JOIN Specialty s ON d.specialtyId = s.id
							WHERE e.role = @Role;  
						""";
            var command = new SqlCommand(query, connection);
            AddParameters(command,
                ("@Role", "doctor"));

            var reader = command.ExecuteReader();
            var result = new List<Doctor>();

            while (reader.Read())
            {
                var doctor = new Doctor
                {
                    Id = (int)reader["id"],
                    name = reader["doctorName"].ToString(),
                    SpecialtyId = (int)reader["specialtyId"],
                    SpecialtyName = reader["specialtyName"].ToString(),
                    Room = reader["room"].ToString()
                };
                result.Add(doctor);
            }
            int count = result.Count;

            connection.Close();
            return result;
        }
		//===============================================================================================



		//========================================MedicalRecord==========================================
		/// <summary>
		/// Lấy thông tin bệnh án theo id
		/// </summary>
		/// <param name="medicalExaminationFormId"></param>
		/// <returns>Bệnh án</returns>
		public MedicalRecord GetMedicalRecordByExaminationFormId(int medicalExaminationFormId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM MedicalRecord WHERE MedicalExaminationFormID = @ExaminationFormId", connection);
                command.Parameters.AddWithValue("@ExaminationFormId", medicalExaminationFormId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MedicalRecord
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            DoctorId = reader.GetInt32(reader.GetOrdinal("doctorId")),
                            MedicalExaminationFormID = reader.GetInt32(reader.GetOrdinal("MedicalExaminationFormID")),
                            Time = reader.GetDateTime(reader.GetOrdinal("time")),
                            Diagnosis = reader.GetString(reader.GetOrdinal("diagnosis"))
                        };
                    }
                }
            }
            return null;
        }

		// New method to create a MedicalRecord using data from MedicalExaminationForm
		/// <summary>
		/// Tạo bệnh án từ phiếu khám bệnh
		/// </summary>
		/// <param name="form"></param>
		/// <returns>Bệnh án vừa được tạo</returns>
		public MedicalRecord CreateMedicalRecordFromForm(MedicalExaminationForm form)
        {
            var record = new MedicalRecord
            {
                DoctorId = form.DoctorId,
                MedicalExaminationFormID = form.Id,
                Time = DateTime.Now,  // Can use form.Time if needed
                Diagnosis = string.Empty
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO MedicalRecord (doctorId, MedicalExaminationFormID, time, diagnosis) " +
                    "VALUES (@DoctorId, @MedicalExaminationFormID, @Time, @Diagnosis); SELECT SCOPE_IDENTITY();", connection);

                command.Parameters.AddWithValue("@DoctorId", record.DoctorId);
                command.Parameters.AddWithValue("@MedicalExaminationFormID", record.MedicalExaminationFormID);
                command.Parameters.AddWithValue("@Time", record.Time);
                command.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);

                record.Id = Convert.ToInt32(command.ExecuteScalar());  // Capture new ID
            }

            return record;
        }

		/// <summary>
		/// Cập nhật thông tin bệnh án
		/// </summary>
		/// <param name="record"></param>
		public void UpdateMedicalRecord(MedicalRecord record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE MedicalRecord SET doctorId = @DoctorId, time = @Time, diagnosis = @Diagnosis " +
                    "WHERE MedicalExaminationFormID = @MedicalExaminationFormID", connection);

                command.Parameters.AddWithValue("@DoctorId", record.DoctorId);
                command.Parameters.AddWithValue("@Time", record.Time);
                command.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);
                command.Parameters.AddWithValue("@MedicalExaminationFormID", record.MedicalExaminationFormID);

                command.ExecuteNonQuery();
            }
        }
		//=========================================================================================================



		//==============================================Prescription===============================================
		/// <summary>
		/// Lưu dơn thuốc vào cơ sở dữ liệu
		/// </summary>
		/// <param name="prescription"></param>
		public void SavePrescription(Prescription prescription)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO Prescription (time, medicineId, quantity, dosage, medicalExaminationFormId) " +
                    "OUTPUT INSERTED.id VALUES (@Time, @MedicineId, @Quantity, @Dosage, @MedicalExaminationFormId)", connection))
                {
                    command.Parameters.AddWithValue("@Time", DateTime.Now);
                    command.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
                    command.Parameters.AddWithValue("@Quantity", prescription.Quantity);
                    command.Parameters.AddWithValue("@Dosage", prescription.Dosage);
                    command.Parameters.AddWithValue("@MedicalExaminationFormId", prescription.MedicalExaminationFormId);

                    prescription.Id = (int)command.ExecuteScalar();
                }
            }
        }

		//=========================================================================================================



		//===================================================Patient===============================================
		/// <summary>
		/// Lấy danh sách bệnh nhân từ cơ sở dữ liệu
		/// </summary>
		/// <param name="page"></param>
		/// <param name="rowsPerPage"></param>
		/// <param name="keyword"></param>
		/// <param name="sortOptions"></param>
		/// <returns>Danh sách bệnh nhân và số lượng bệnh nhân</returns>
		public Tuple<List<Patient>, int> GetPatients(
            int page, int rowsPerPage,
            string keyword,
            Dictionary<string, SortType> sortOptions
        )
        {
            var result = new List<Patient>();
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sortString = "ORDER BY ";
            bool useDefault = true;
            foreach (var item in sortOptions)
            {
                useDefault = false;
                if (item.Key == "Name")
                {
                    if (item.Value == SortType.Ascending)
                    {
                        sortString += "Name ASC, ";
                    }
                    else
                    {
                        sortString += "Name DESC, ";
                    }

                }
            }
            if (useDefault)
            {
                sortString += "ID ";
            }

            var sql = $"""
                SELECT count(*) over() as Total, id, name, residentId, email, gender, birthday, address
                FROM Patient
                WHERE Name like @Keyword
                {sortString}
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
                """;

            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            var reader = command.ExecuteReader();
            int count = -1;

            while (reader.Read())
            {
                if (count == -1)
                {
                    count = (int)reader["Total"];
                }
                var patient = new Patient();
                patient.Id = (int)reader["id"];
                patient.Name = (string)reader["name"];
                patient.ResidentId = (string)reader["residentId"];
                patient.Email = (string)reader["email"];
                patient.Gender = (string)reader["gender"];
                patient.DoB = (DateTime)reader["birthday"];
                patient.Address = (string)reader["address"];
                result.Add(patient);
            }

            connection.Close();
            return new Tuple<List<Patient>, int>(
                result, count
            );
        }
		/// <summary>
		/// Cập nhật thông tin bệnh nhân
		/// </summary>
		/// <param name="patient"></param>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool UpdatePatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var sql = @"update Patient set 
                        name=@name,
                        residentId=@residentId,
                        email=@email, 
                        gender=@gender, 
                        birthday=@birthday, 
                        address=@address
                        where id=@id";
            var command = new SqlCommand(sql, connection);
            AddParameters(command,
                ("@id", patient.Id),
                ("@name", patient.Name),
                ("@residentId", patient.ResidentId),
                ("@email", patient.Email),
                ("@gender", patient.Gender),
                ("@birthday", patient.DoB),
                ("@address", patient.Address));

            int count = command.ExecuteNonQuery();
            bool success = count == 1;

            connection.Close();
            return success;
        }

		/// <summary>
		/// Xóa bệnh nhân
		/// </summary>
		/// <param name="patient"></param>
		/// <returns>True nếu xóa thành công, False nếu xóa thất bại</returns>
		public bool DeletePatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var getMedicalExaminationFormsSql = @"SELECT Id FROM MedicalExaminationForm WHERE PatientId = @PatientId";
                    var getMedicalExaminationFormsCommand = new SqlCommand(getMedicalExaminationFormsSql, connection, transaction);
                    AddParameters(getMedicalExaminationFormsCommand, ("@PatientId", patient.Id));
                    var reader = getMedicalExaminationFormsCommand.ExecuteReader();

                    var formIdsToDelete = new List<int>();
                    while (reader.Read())
                    {
                        formIdsToDelete.Add(reader.GetInt32(0));
                    }
                    reader.Close();

                    foreach (var formId in formIdsToDelete)
                    {
                        var form = new MedicalExaminationForm { Id = formId };
                        bool delete = DeleteMedicalExaminationForm(form);
                        if (!delete)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    var deletePatientSql = @"DELETE FROM Patient WHERE Id = @Id";
                    var deletePatientCommand = new SqlCommand(deletePatientSql, connection, transaction);
                    AddParameters(deletePatientCommand, ("@Id", patient.Id));
                    int count = deletePatientCommand.ExecuteNonQuery();

                    transaction.Commit();
                    return count == 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

		/// <summary>
		/// Lấy thông tin bệnh nhân theo id
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns>Bệnh nhân được lấy thông tin</returns>
		public Patient GetPatientById(int patientId)
        {
            Patient patient = null;
            string query = "SELECT * FROM Patient WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", patientId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patient = new Patient
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            ResidentId = reader.GetString(reader.GetOrdinal("residentId")),
                            Address = reader.GetString(reader.GetOrdinal("address")),
                            DoB = reader.IsDBNull(reader.GetOrdinal("birthday")) ?
                                (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("birthday")),
                            Gender = reader.GetString(reader.GetOrdinal("gender"))
                        };
                    }
                }
            }

            return patient;
        }
		//=========================================================================================================

		//=====================================================Bill================================================
		/// <summary>
		/// Cập nhật số lượng thuốc
		/// </summary>
		/// <param name="selectedMedicines"></param>
		public void UpdateMedicineQuantities(List<MedicineSelection> selectedMedicines)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update medicine quantities
                        foreach (var medicineSelection in selectedMedicines)
                        {
                            var command = new SqlCommand("UPDATE Medicine SET Quantity = Quantity - @quantity WHERE Id = @id", connection, transaction);
                            command.Parameters.AddWithValue("@quantity", medicineSelection.SelectedQuantity);
                            command.Parameters.AddWithValue("@id", medicineSelection.Medicine.Id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //public void InsertBill(int prescriptionId, int totalAmount)
        //{
        //    using (var connection = new SqlConnection(GetConnectionString()))
        //    {
        //        connection.Open();
        //        var command = new SqlCommand("INSERT INTO Bill (prescriptionId, totalAmount) VALUES (@prescriptionId, @totalAmount)", connection);
        //        command.Parameters.AddWithValue("@prescriptionId", prescriptionId);
        //        command.Parameters.AddWithValue("@totalAmount", totalAmount);
        //        command.ExecuteNonQuery();
        //    }
        //}
        //=========================================================================================================


        //==========================================================Bill========================================
        public List<BillStatistic> GetBillStatistic(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<BillStatistic>();
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"""
                select CONVERT(date, b.CreateDate) as Date,sum(b.totalAmount) as TotalAmount
                from Bill b
                where b.CreateDate between @startDate and @endDate
                group by CONVERT(date, b.CreateDate)
                """;
            var command = new SqlCommand(query, connection);
            AddParameters(command, ("@startDate", startDate), ("@endDate", endDate));
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var BillStatistic = new BillStatistic();
                BillStatistic.CreateDate = (DateTime)reader["Date"];
                BillStatistic.TotalAmount = (int)reader["TotalAmount"];
                result.Add(BillStatistic);
            }
            connection.Close();
            return result;
        }

    }
}
