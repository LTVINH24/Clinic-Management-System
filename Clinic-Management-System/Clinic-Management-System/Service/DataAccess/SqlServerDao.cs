using Clinic_Management_System.Model.DoctorModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.Service.DataAccess
{
    public class SqlServerDao : IDao
    {
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

        public string Authentication(string username , string password )
        {
            if (username == null || password == null)
            {
                return "";
            }
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = $"SELECT role FROM EndUser WHERE username = @Username AND password = @Password";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            var role = command.ExecuteScalar();
            connection.Close();
            return role != null ? role.ToString() : "";
        }

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
                            Symptom = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),          // Symptom
                            DoctorId = reader.GetInt32(5)                                               // DoctorId
                        };

                        forms.Add(form);
                    }
                }
            }

            return forms;
        }

        /*====================================================================================================*/

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
                            Symptom = reader.IsDBNull(reader.GetOrdinal("symptom")) ? (string)null : reader.GetString(reader.GetOrdinal("symptom")),
                            DoctorId = reader.GetInt32(reader.GetOrdinal("doctorId"))
                        };
                    }
                }
            }
            return null;
        }

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

        /*====================================================================================================*/

        public void SavePrescription(Prescription prescription)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Prescription (MedicalRecordId, MedicineId, Quantity) " +
                    "VALUES (@MedicalRecordId, @MedicineId, @Quantity)", connection);

                command.Parameters.AddWithValue("@MedicalRecordId", prescription.MedicalRecordId);
                //command.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
                //command.Parameters.AddWithValue("@Quantity", prescription.Quantity);

                command.ExecuteNonQuery();
            }
        }

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

        public List<Medicine> GetAvailableMedicines()
        {
            var medicines = new List<Medicine>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, Quantity FROM Medicine WHERE Quantity > 0", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medicines.Add(new Medicine
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
            }

            return medicines;
        }

    }
}
