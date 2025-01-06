using ClinicManagementSystem.Model;
using ClinicManagementSystem.Model.Statistic;
using ClinicManagementSystem.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ClinicManagementSystem.Service.DataAccess.IDao;

namespace ClinicManagementSystem.Service.DataAccess
{
    public class SqlServerDao : IDao
    {

        private readonly string _connectionString;

		/// <summary>
		/// Constructor
		/// </summary>
		public SqlServerDao()
        {
            _connectionString = ConfigurationManager.AppSetting
                .GetConnectionString("DefaultConnection");
        }


		//==============================================Helper===========================================
		/// <summary>
		/// Xứ lí xác thực người dùng
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>Thông tin người dùng bao gồm id, name, role, phone, gender, address</returns>
		public (int, string, string, string, string, string) Authentication(string username, string password)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = $"SELECT id,name, role,password,phone,birthday,gender,address,status FROM EndUser WHERE username = @Username";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            var reader = command.ExecuteReader();
            int id = 0;
            string name = "";
            string role = "";
            string hassPassword = "";
            string phone = "";
            string status = "";
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
                status = reader["status"].ToString();

                connection.Close();
                if (Password.VerifyPassword(password, hassPassword) && status != "locked")
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
                    SELECT count(*) over() as Total, id, name, role, username,phone,birthday,address,gender,status
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
                user.address = (string)reader["address"];
                user.phone = (string)reader["phone"];
                user.status = (string)reader["status"];
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
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

            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string passwordString = "";
            if (info.password != null && info.password!="")
            {
                passwordString = "password=@password, ";
            } 
                

            var sql = $"""update EndUser set name=@name, {passwordString}phone=@phone,birthday=@birthday,address=@address,gender=@gender where id=@id""";
            var command = new SqlCommand(sql, connection);
            AddParameters(command,
                ("@id", info.id),
                ("@name", info.name),
                ("@phone", info.phone),
                ("@birthday", info.birthday),
                ("@address", info.address),
                ("@gender", info.gender)
                );
            if (info.password != null && info.password != "")
            {
                AddParameters(command, ("@password", info.password));
            }
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
		public bool LockUser(int id,string status)
        {
            SqlConnection connection = new SqlConnection( _connectionString);
            connection.Open();
            string query = "update EndUser set status = @status where id=@id";
            var command = new SqlCommand(query, connection);
            AddParameters(command, ("@status", status), ("@id", id));
            var result = command.ExecuteNonQuery();
            connection.Close();
            return result > 0;
        }
        public int GetTotalUsersCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(*) FROM EndUser", connection);
                return (int)command.ExecuteScalar();
            }
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
            // var _connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(_connectionString);
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
		/// <summary>
		/// Hàm tạo chuyên khoa
		/// </summary>
		/// <param name="specialty"></param>
		/// <returns></returns>
		public (bool success, int specialtyId) CreateSpecialty(string specialty)
        {
            // string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = $"""
            INSERT INTO Specialty (name) 
            OUTPUT INSERTED.Id 
            VALUES (@name)
        """;
                using (var command = new SqlCommand(query, connection))
                {
                    AddParameters(command, ("@name", specialty));
                    int specialtyId = (int)command.ExecuteScalar(); // Lấy ID của specialty vừa tạo
                    return (true, specialtyId);
                }
            }
        }

		/// <summary>
		/// Hàm cập nhật chuyên khoa
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public User GetUserById(int userId)
        {
            User user = new User();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = $"""
                select id, name,phone,address,gender,birthday
                from EndUser
                where id = @id
                """;
            var command = new SqlCommand(query, connection);
            AddParameters(command,("@id",userId));
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                user.id = (int)reader["id"];
               user.name = (string)reader["name"];
                user.phone = (string)reader["phone"];
                user.address = (string)reader["address"];
                user.gender = (string)reader["gender"];
                user.birthday = (DateTime)reader["birthday"];
            }
            connection.Close();
            return user;
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
                Dictionary<string, SortType> sortOptions, int daysRemaining)
        {
            string filter = "";
            if(daysRemaining > 0)
            {
                filter = "AND DATEDIFF(day, GETDATE(), ExpDate) <= @DaysRemaining";
            }    
            var result = new List<Medicine>();
            SqlConnection connection = new SqlConnection(_connectionString);
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
                    SELECT count(*) over() as Total, id, name, manufacturer, price,quantity,quantityimport,expdate,mfgdate,dateimport
                    FROM Medicine
                    WHERE Name like @Keyword and isDeleted != 'true' {filter}
                    {sortString} 
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
                """;
            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            if (daysRemaining > 0)
            {
                AddParameters(command, ("@DaysRemaining", daysRemaining));
            }
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
                medicine.ExpDate = (DateTime)reader["expdate"];
                medicine.MfgDate = (DateTime)reader["mfgdate"];
                medicine.DateImport =(DateTime)reader["dateimport"];



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
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
            // string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(_connectionString);
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

		/// <summary>
		/// Lấy danh sách thuốc còn trong kho
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="n"></param>
		/// <param name="sortString"></param>
		/// <returns>Danh sách thuốc bán và có doanh thu nhiều nhất</returns>
		public List<MedicineStatistic> GetTopMedicineStatistic(DateTimeOffset startDate, DateTimeOffset endDate, int n, string sortString)
        {
            var result = new List<MedicineStatistic>();
            using (var connection = new SqlConnection(_connectionString))
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
		/// Lấy danh sách thuốc bán theo ngày
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns>Danh sách thuốc theo ngày</returns>
		public List<MedicineStatistic> GetMedicineStatistic(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<MedicineStatistic>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"""
                    SELECT 
                    CAST(p.time AS DATE) as Date,
                    m.name as MedicineName, 
                    ISNULL(SUM(pd.quantity), 0) as QuantitySold, 
                    ISNULL(SUM(pd.quantity * m.price), 0) as MoneySold
                    FROM Medicine m 
                    JOIN PrescriptionDetail pd ON m.id = pd.medicineId
                    JOIN Prescription p ON pd.prescriptionId = p.id
                    JOIN Bill b ON p.id = b.prescriptionId
                    WHERE p.time BETWEEN @startDate AND @endDate
                    GROUP BY CAST(p.time AS DATE), m.id, m.name
                    ORDER BY Date DESC
                 """;
                    using (var command = new SqlCommand(query, connection))
                    {
                        AddParameters(command, ("@startDate", startDate), ("@endDate", endDate));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new MedicineStatistic
                                {
                                    Date = Convert.ToDateTime(reader["Date"]),
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
                return result;
            }
        }
        /// <summary>
        /// Lấy danh sách thuốc còn trong kho
        /// </summary>
        /// <returns>Danh sách thuốc</returns>
        /// 
        public List<Medicine> GetAvailableMedicines()
        {
            var medicines = new List<Medicine>();
            using (var connection = new SqlConnection(_connectionString))
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
        
        /// <summary>
        /// Lấy danh sách thuốc đã kê cho một phiếu khám bệnh
        /// </summary>
        /// <param name="formId">ID của phiếu khám bệnh</param>
        /// <returns>Danh sách các MedicineSelection</returns>
        public List<MedicineSelection> GetMedicineSelectionsByFormId(int formId)
        {
            var medicines = new List<MedicineSelection>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT m.id, m.name, m.manufacturer, m.price, " +
                    "m.quantity, m.expDate, m.mfgDate, pd.quantity, pd.dosage " +
                    "FROM Medicine m " +
                    "INNER JOIN PrescriptionDetail pd ON m.id = pd.medicineId " +
                    "INNER JOIN Prescription p ON pd.prescriptionId = p.id " +
                    "WHERE p.medicalExaminationFormId = @formId",
                    connection);

                command.Parameters.AddWithValue("@formId", formId);

                try
                {
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

                            var medicineSelection = new MedicineSelection
                            {
                                Medicine = medicine,
                                IsSelected = true,
                                SelectedQuantity = reader.GetInt32(7),    // prescribedQuantity
                                SelectedDosage = reader.GetString(8)      // prescribedDosage
                            };

                            medicines.Add(medicineSelection);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine($"Error in GetMedicineSelectionsByFormId: {ex.Message}");
                    return new List<MedicineSelection>();
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
		public List<MedicalExaminationForm> GetMedicalExaminationForms(int id)
        {
            var forms = new List<MedicalExaminationForm>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT f.id, f.patientId, f.staffId, f.doctorId, f.time, 
                               f.symptom, f.visitType, f.isExaminated,
                               p.name, p.email, p.residentId, p.address, p.birthday, p.gender
                        FROM MedicalExaminationForm f
                        INNER JOIN Patient p ON f.patientId = p.id
                        WHERE f.doctorId = @id
                        ORDER BY f.time DESC";
                    AddParameters(command, ("@id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            forms.Add(new MedicalExaminationForm
                            {
                                Id = reader.GetInt32(0),
                                PatientId = reader.GetInt32(1),
                                StaffId = reader.GetInt32(2),
                                DoctorId = reader.GetInt32(3),
                                Time = reader.GetDateTime(4),
                                Symptoms = reader.GetString(5),
                                VisitType = reader.GetString(6),
                                IsExaminated = reader.GetString(7),
                                Patient = new Patient
                                {
                                    Id = reader.GetInt32(1), // PatientId
                                    Name = reader.GetString(8),
                                    Email = reader.GetString(9),
                                    ResidentId = reader.GetString(10),
                                    Address = reader.GetString(11),
                                    DoB = reader.GetDateTime(12),
                                    Gender = reader.GetString(13)
                                }
                            });
                        }
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
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns>Danh sách phiếu khám bệnh và số lượng phiếu khám bệnh</returns>
		public Tuple<List<MedicalExaminationForm>, int> GetMedicalExaminationForms(
            int page,
            int rowsPerPage,
            string keyword,
            DateTimeOffset? startDate,
			DateTimeOffset? endDate,
            string statusFilter,
			Dictionary<string, SortType> sortOptions)
        {
            var result = new List<MedicalExaminationForm>();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            var whereClause = "WHERE m.staffId = @StaffId";
			if (!string.IsNullOrEmpty(keyword))
			{
                whereClause += @" AND (
                    p.name LIKE @Keyword OR
                    m.symptom LIKE @Keyword OR
                    d.name LIKE @Keyword
                )";
			}

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

			if (!string.IsNullOrEmpty(statusFilter))
			{
				if (statusFilter == "Examined")
				{
					whereClause += " AND m.IsExaminated = 'True'";
				}
				else if (statusFilter == "Pending")
				{
					whereClause += " AND m.IsExaminated = 'False'";
				}
			}

			if (startDate.HasValue)
			{
				whereClause += " AND CAST(time AS DATE) >= @StartDate";
			}
			if (endDate.HasValue)
			{
				whereClause += " AND CAST(time AS DATE) <= @EndDate";
			}

			var sql = $@"
                SELECT count(*) over() as Total, 
                       m.id, m.patientId, m.staffId, m.time, m.symptom, m.doctorId, m.visitType, m.isExaminated as status,
                       p.name as PatientName,
                       d.name as DoctorName
                FROM MedicalExaminationForm m
                JOIN Patient p ON m.patientId = p.id
                JOIN EndUser d ON m.doctorId = d.id
                {whereClause}
                {sortString}
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
            ";

			var command = new SqlCommand(sql, connection);
			AddParameters(command, 
                ("@Skip", (page - 1) * rowsPerPage), 
                ("@Take", rowsPerPage), 
                ("@Keyword", $"%{keyword}%"), 
                ("@StaffId", UserSessionService.Instance.LoggedInUserId));

			if (startDate.HasValue)
			{
				var localStartDate = startDate.Value.LocalDateTime.Date;
				AddParameters(command, ("@StartDate", localStartDate));
			}
			if (endDate.HasValue)
			{
				var localEndDate = endDate.Value.LocalDateTime.Date;
				AddParameters(command, ("@EndDate", localEndDate));
			}


			var reader = command.ExecuteReader();
            int count = -1;

            while (reader.Read())
            {
                if (count == -1)
                {
                    count = (int)reader["Total"];
                }

				var medicalExaminationForm = new MedicalExaminationForm
				{
					Id = (int)reader["id"],
					PatientId = (int)reader["patientId"],
					PatientName = reader["PatientName"].ToString(),
					StaffId = (int)reader["staffId"],
					Time = (DateTime)reader["time"],
					Symptoms = (string)reader["symptom"],
					DoctorId = (int)reader["doctorId"],
					DoctorName = reader["DoctorName"].ToString(),
					VisitType = (string)reader["visitType"],
					IsExaminated = (string)reader["status"] == "true" ? "Examined" : "Not examinated"
				};

				result.Add(medicalExaminationForm);
            }
            connection.Close();
            return new Tuple<List<MedicalExaminationForm>, int>(result, count);
        }

		public MedicalExaminationFormDetail GetMedicalExaminationFormDetail(int formId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

			var query = @"
                SELECT m.*, p.Name as PatientName, p.Email, p.Birthday, p.Gender,
                       u.Name as DoctorName, mr.Diagnosis,
                       pr.NextExaminationDate,
                       med.Name as MedicineName, pd.Dosage, pd.Quantity
                FROM MedicalExaminationForm m
                JOIN Patient p ON m.PatientId = p.Id 
                JOIN EndUser u ON m.DoctorId = u.Id
                LEFT JOIN MedicalRecord mr ON m.Id = mr.MedicalExaminationFormId
                LEFT JOIN Prescription pr ON m.Id = pr.MedicalExaminationFormId
                LEFT JOIN PrescriptionDetail pd ON pr.Id = pd.PrescriptionId
                LEFT JOIN Medicine med ON pd.MedicineId = med.Id
                WHERE m.Id = @FormId";

			var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@FormId", formId);

			var result = new MedicalExaminationFormDetail();
			var medicines = new List<PrescriptionMedicine>();

            var reader = command.ExecuteReader();
			while (reader.Read())
			{
                result.Id = (int)reader["Id"];
                result.PatientName = reader["PatientName"].ToString();
                result.PatientEmail = reader["Email"].ToString();
                result.DoctorName = reader["DoctorName"].ToString();
                result.Time = reader.GetDateTime(reader.GetOrdinal("Time"));
                result.Symptoms = reader["Symptom"].ToString();
                result.Diagnosis = reader["Diagnosis"]?.ToString();
                result.NextExaminationDate = reader["NextExaminationDate"] != DBNull.Value
                    ? Convert.ToDateTime(reader["NextExaminationDate"])
                    : null;

                if (!reader.IsDBNull(reader.GetOrdinal("MedicineName")))
				{
					medicines.Add(new PrescriptionMedicine
					{
						MedicineName = reader["MedicineName"].ToString(),
						Dosage = (string)reader["Dosage"],
						Quantity = (int)reader["Quantity"]
					});
				}
			}

			result.Medicines = medicines;
			return result;
		}

        public List<MedicalExaminationStatistic> GetMedicalExaminationStatisticsByDate(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<MedicalExaminationStatistic>();
            SqlConnection connection = new SqlConnection(_connectionString);
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

		public int GetTodayMedicalExaminationFormsCount()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM MedicalExaminationForm 
                    WHERE CAST(Time AS DATE) = @Today", connection);

				command.Parameters.AddWithValue("@today", DateTime.Now.Date);
				return (int)command.ExecuteScalar();
			}
		}

		public int GetPendingFormsCount()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(@"
            SELECT COUNT(*) 
            FROM MedicalExaminationForm mef
            LEFT JOIN MedicalRecord mr ON mef.Id = mr.MedicalExaminationFormID
            WHERE mr.Id IS NULL", connection);

				return (int)command.ExecuteScalar();
			}
		}

		/// <summary>
		/// Thêm bệnh nhân
		/// </summary>
		/// <param name="patient"></param>
		/// <returns>True và id bệnh nhân nếu tạo thành công, False và id bằng 0 nếu tạo thất bại</returns>
		public (bool, int) AddPatient(Patient patient)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            medicalExaminationForm.Time = DateTimeOffset.Now;
            int id = UserSessionService.Instance.LoggedInUserId;


            var command = new SqlCommand("INSERT INTO MedicalExaminationForm (PatientId, StaffId, DoctorId, Time, Symptom, VisitType, IsExaminated) " +
                "VALUES (@PatientId, @StaffId, @DoctorId, @Time, @Symptom, @VisitType, @IsExaminated)", connection);

            AddParameters(command,
                ("@PatientId", patientId),
                ("@StaffId", id),
                ("@DoctorId", medicalExaminationForm.DoctorId),
                ("@Time", medicalExaminationForm.Time),
                ("@Symptom", medicalExaminationForm.Symptoms),
                ("@VisitType", medicalExaminationForm.VisitType),
                ("@IsExaminated", "false"));

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
                            Time = DateTimeOffset.Parse(reader.GetDateTime(3).ToString()),
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
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

			form.Time = DateTimeOffset.Now;
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
				("@time", form.Time.Value),
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
		/// <summary>
		/// Lấy số lượng phiếu khám bệnh chưa được khám trong ngày
		/// </summary>
		/// <param name="doctorId"></param>
		/// <returns>Số phiếu khám bệnh chưa được khám</returns>
		public int GetTodayFormsByDoctorId(int doctorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM MedicalExaminationForm mef
                    WHERE CAST(mef.Time AS DATE) = CAST(GETDATE() AS DATE)
                    AND mef.IsExaminated = 'false'
                    AND mef.DoctorId = @DoctorId", connection);

                command.Parameters.AddWithValue("@DoctorId", doctorId);
                
                return (int)command.ExecuteScalar();
            }
        }
        
        public int GetTodayCompletedFormsByDoctorId(int doctorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM MedicalExaminationForm mef
                    WHERE CAST(mef.Time AS DATE) = CAST(GETDATE() AS DATE)
                    AND mef.DoctorId = @DoctorId
                    AND mef.IsExaminated = 'true'", connection);

                command.Parameters.AddWithValue("@DoctorId", doctorId);
                
                return (int)command.ExecuteScalar();
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
            // var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(_connectionString);
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
                    "UPDATE MedicalRecord " +
                    "SET doctorId = @DoctorId, time = @Time, diagnosis = @Diagnosis " +
                    "WHERE MedicalExaminationFormID = @MedicalExaminationFormID", 
                    connection);

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
		public bool SavePrescription(
            int medicalExaminationFormId, 
            List<MedicineSelection> selectedMedicines, 
            DateTimeOffset? nextExaminationDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo đơn thuốc mới
                        var insertPrescriptionCommand = new SqlCommand(
                            "INSERT INTO Prescription (time, medicalExaminationFormId, nextExaminationDate, isBilled) " +
                            "VALUES (@time, @medicalExaminationFormId, @nextExaminationDate, @isBilled); " +
                            "SELECT SCOPE_IDENTITY();", 
                            connection, 
                            transaction);

                        insertPrescriptionCommand.Parameters.AddWithValue("@time", DateTime.Now.Date);
                        insertPrescriptionCommand.Parameters.AddWithValue("@medicalExaminationFormId", medicalExaminationFormId);
                        insertPrescriptionCommand.Parameters.AddWithValue("@isBilled", "false");

                        // Xử lý nextExaminationDate
                        if (nextExaminationDate.HasValue)
                        {
                            insertPrescriptionCommand.Parameters.AddWithValue("@nextExaminationDate", nextExaminationDate.Value.DateTime);
                        }
                        else
                        {
                            insertPrescriptionCommand.Parameters.AddWithValue("@nextExaminationDate", DBNull.Value);
                        }

                        // Lấy ID của đơn thuốc vừa tạo
                        int prescriptionId = Convert.ToInt32(insertPrescriptionCommand.ExecuteScalar());

                        // 2. Thêm chi tiết đơn thuốc
                        foreach (var medicine in selectedMedicines)
                        {
                            var insertDetailCommand = new SqlCommand(
                                "INSERT INTO PrescriptionDetail (PrescriptionId, medicineId, quantity, dosage) " +
                                "VALUES (@prescriptionId, @medicineId, @quantity, @dosage)",
                                connection,
                                transaction);

                            insertDetailCommand.Parameters.AddWithValue("@prescriptionId", prescriptionId);
                            insertDetailCommand.Parameters.AddWithValue("@medicineId", medicine.Medicine.Id);
                            insertDetailCommand.Parameters.AddWithValue("@quantity", medicine.SelectedQuantity);
                            insertDetailCommand.Parameters.AddWithValue("@dosage", medicine.SelectedDosage);

                            insertDetailCommand.ExecuteNonQuery();
                        }

                        // 3. Cập nhật trạng thái đã khám
                        var updateFormCommand = new SqlCommand(
                            "UPDATE MedicalExaminationForm SET isExaminated = 'true' " +
                            "WHERE id = @formId",
                            connection,
                            transaction);

                        updateFormCommand.Parameters.AddWithValue("@formId", medicalExaminationFormId);
                        updateFormCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy thông tin đơn thuốc theo ID phiếu khám
        /// </summary>
        /// <param name="formId">ID của phiếu khám</param>
        /// <returns>Đơn thuốc nếu tồn tại, null nếu không tìm thấy</returns>
        public Prescription GetPrescriptionByFormId(int formId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT id, time, medicalExaminationFormId, nextExaminationDate " +
                    "FROM Prescription " +
                    "WHERE medicalExaminationFormId = @formId",
                    connection);
                
                command.Parameters.AddWithValue("@formId", formId);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Prescription
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Time = reader.GetDateTime(reader.GetOrdinal("time")),
                                MedicalExaminationFormId = reader.GetInt32(reader.GetOrdinal("medicalExaminationFormId")),
                                NextExaminationDate = reader.IsDBNull(reader.GetOrdinal("nextExaminationDate")) 
                                    ? (DateTime?)null 
                                    : reader.GetDateTime(reader.GetOrdinal("nextExaminationDate"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine($"Error in GetPrescriptionByFormId: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        public bool UpdatePrescriptionBillStatus(int prescriptionId, string isBilled)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE Prescription SET isBilled = @isBilled " +
                    "WHERE id = @prescriptionId",
                    connection);

                command.Parameters.AddWithValue("@isBilled", isBilled);
                command.Parameters.AddWithValue("@prescriptionId", prescriptionId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // Thêm phương thức lấy danh sách theo trạng thái in
        public List<Prescription> GetPrescriptionsByBillStatus(bool isBilled)
        {
            List<Prescription> prescriptions = new List<Prescription>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Prescription WHERE isBilled = @IsBilled";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsBilled", isBilled ? "true" : "false");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    var prescription = new Prescription
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                                        Time = reader.GetDateTime(reader.GetOrdinal("time")),
                                        MedicalExaminationFormId = reader.GetInt32(reader.GetOrdinal("medicalExaminationFormId")),
                                        NextExaminationDate = reader.IsDBNull(reader.GetOrdinal("nextExaminationDate")) 
                                            ? (DateTime?)null 
                                            : reader.GetDateTime(reader.GetOrdinal("nextExaminationDate")),
                                        IsBilled = reader.GetString(reader.GetOrdinal("isBilled"))  // Thay đổi từ GetBoolean sang GetString
                                    };
                                    prescriptions.Add(prescription);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error reading prescription: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
            }
            return prescriptions;
        }

        public Prescription GetPrescriptionById(int id)
        {
            Prescription prescription = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Prescription WHERE id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prescription = new Prescription
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    MedicalExaminationFormId = reader.GetInt32(reader.GetOrdinal("medicalExaminationFormId")),
                                    Time = reader.GetDateTime(reader.GetOrdinal("time")),
                                    IsBilled = reader.GetString(reader.GetOrdinal("isBilled"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error in GetPrescriptionById: {ex.Message}");
            }
            return prescription;
        }
		/// <summary>
		/// Lấy số đơn thuốc được kê trong tháng hiện tại của bác sĩ
		/// </summary>
		/// <param name="doctorId"></param>
		/// <returns>Số đơn thuốc được kê</returns>
		public int GetMonthlyPrescriptionCountByDoctorId(int doctorId) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT COUNT(DISTINCT p.Id)
                    FROM Prescription p
                    JOIN MedicalExaminationForm mef ON p.MedicalExaminationFormId = mef.Id
                    WHERE MONTH(mef.Time) = MONTH(GETDATE())
                    AND YEAR(mef.Time) = YEAR(GETDATE())
                    AND mef.DoctorId = @DoctorId", connection);

                command.Parameters.AddWithValue("@DoctorId", doctorId);
                
                return (int)command.ExecuteScalar();
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
			DateTimeOffset? startDate,
			DateTimeOffset? endDate,
			Dictionary<string, SortType> sortOptions
        )
        {
            var result = new List<Patient>();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            var whereClause = "WHERE 1=1";
            if (!string.IsNullOrEmpty(keyword))
            {
                whereClause += @" AND (
                    p.name LIKE @Keyword OR 
                    p.residentId LIKE @Keyword OR 
                    p.email LIKE @Keyword OR 
                    p.address LIKE @Keyword
                )";
            }

            if (startDate.HasValue)
            {
                whereClause += @" AND EXISTS (
                    SELECT 1 
                    FROM MedicalExaminationForm m 
                    JOIN Prescription pr ON m.id = pr.medicalExaminationFormId
                    WHERE m.patientId = p.id 
                    AND CAST(pr.NextExaminationDate AS DATE) >= @StartDate
                )";
            }
            if (endDate.HasValue)
            {
                whereClause += @" AND EXISTS (
                    SELECT 1 
                    FROM MedicalExaminationForm m 
                    JOIN Prescription pr ON m.id = pr.medicalExaminationFormId
                    WHERE m.patientId = p.id 
                    AND CAST(pr.NextExaminationDate AS DATE) <= @EndDate
                )";
            }

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
                SELECT count(*) over() as Total, p.id, p.name, p.residentId, p.email, p.gender, p.birthday, p.address,
                       (
                           SELECT TOP 1 pr.NextExaminationDate
                           FROM MedicalExaminationForm m 
                           LEFT JOIN Prescription pr ON m.id = pr.medicalExaminationFormId
                           WHERE m.patientId = p.id
                           ORDER BY pr.NextExaminationDate DESC
                       ) as NextExaminationDate
                FROM Patient p
                {whereClause}
                {sortString}
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
                """;

			var command = new SqlCommand(sql, connection);
            AddParameters(command, 
                ("@Skip", (page - 1) * rowsPerPage), 
                ("@Take", rowsPerPage), 
                ("@Keyword", $"%{keyword}%"),
                ("@CurrentDate", DateTime.Now.Date));

			if (startDate.HasValue)
			{
				var localStartDate = startDate.Value.LocalDateTime.Date;
				AddParameters(command, ("@StartDate", localStartDate));
			}
			if (endDate.HasValue)
			{
				var localEndDate = endDate.Value.LocalDateTime.Date;
				AddParameters(command, ("@EndDate", localEndDate));
			}

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
                patient.NextExaminationDate = reader["NextExaminationDate"] == DBNull.Value ? null : (DateTime?)reader["NextExaminationDate"];
                result.Add(patient);
            }

            connection.Close();
            return new Tuple<List<Patient>, int>(
                result, count
            );
        }
		/// <summary>
		/// Lấy tổng số bệnh nhân
		/// </summary>
		/// <returns>Tổng số bệnh nhân</returns>
		public int GetTotalPatientsCount()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand("SELECT COUNT(*) FROM Patient", connection);
				return (int)command.ExecuteScalar();
			}
		}
		/// <summary>
		/// Lấy số lượng bệnh nhân mới trong ngày
		/// </summary>
		/// <returns>Số lượng bệnh nhân mới trong ngày</returns>
		public int GetTodayNewPatientsCount()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(@"
                    SELECT COUNT(DISTINCT p.Id)
                    FROM Patient p
                    JOIN MedicalExaminationForm mef ON p.Id = mef.PatientId
                    WHERE CAST(mef.Time AS DATE) = @Today
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM MedicalExaminationForm mef2
                        WHERE mef2.PatientId = p.Id
                        AND CAST(mef2.Time AS DATE) < @Today
                    )", connection);

				command.Parameters.AddWithValue("@today", DateTime.Now.Date);

				return (int)command.ExecuteScalar();
			}
		}

		/// <summary>
		/// Cập nhật thông tin bệnh nhân
		/// </summary>
		/// <param name="patient"></param>
		/// <returns>True nếu cập nhật thành công, False nếu cập nhật thất bại</returns>
		public bool UpdatePatient(Patient patient)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
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
            SqlConnection connection = new SqlConnection(_connectionString);
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
		/// <summary>
		/// Lấy số lượng bệnh nhân được khám trong tháng hiện tại của bác sĩ
		/// </summary>
		/// <param name="doctorId"></param>
		/// <returns>Số lượng bệnh nhân được khám</returns>
		public int GetMonthlyPatientCountByDoctorId(int doctorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                    SELECT COUNT(DISTINCT mef.PatientId)
                    FROM MedicalExaminationForm mef
                    WHERE MONTH(mef.Time) = MONTH(GETDATE())
                    AND YEAR(mef.Time) = YEAR(GETDATE())
                    AND mef.DoctorId = @DoctorId
                    AND mef.IsExaminated = 'true'", connection);

                command.Parameters.AddWithValue("@DoctorId", doctorId);
                
                return (int)command.ExecuteScalar();
            }
        }
		//=========================================================================================================

		//=====================================================Bill================================================
		/// <summary>
		/// Cập nhật số lượng thuốc
		/// </summary>
		/// <param name="selectedMedicines"></param>
		public void UpdateMedicineQuantities(List<MedicineSelection> selectedMedicines)
        {
            using (var connection = new SqlConnection(_connectionString))
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
		/// <summary>
		/// Lấy thông tin hóa đơn theo ngày
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns>Danh sách hóa đơn theo ngày</returns>
		public List<BillStatistic> GetBillStatistic(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<BillStatistic>();
            SqlConnection connection = new SqlConnection(_connectionString);
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

        public bool SaveBill(Bill bill)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO Bill (prescriptionId, totalAmount, createDate, isGetMedicine) 
                                VALUES (@PrescriptionId, @TotalAmount, @CreateDate, @IsGetMedicine)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PrescriptionId", bill.PrescriptionId);
                        command.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                        command.Parameters.AddWithValue("@CreateDate", bill.CreatedDate);
                        command.Parameters.AddWithValue("@IsGetMedicine", bill.IsGetMedicine);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SaveBill: {ex.Message}");
                return false;
            }
        }
    }
}
