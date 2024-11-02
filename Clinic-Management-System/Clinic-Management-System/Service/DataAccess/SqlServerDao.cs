using Clinic_Management_System.Model;
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
        public string Authentication(string username, string password)
        {
            if (username == null || password == null)
            {
                return "";
            }
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT role FROM EndUser WHERE username = @Username AND password = @Password";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            var role = command.ExecuteScalar();
            connection.Close();
            return role != null ? role.ToString() : "";
        }
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

        public bool AddPatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();



            var command = new SqlCommand("INSERT INTO Patient (Name , Email , ResidentId, Address , Birthday , Gender) VALUES (@Name , @Email , @ResidentId , @Address , @DoB , @Gender)", connection);
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = patient.Name;
            command.Parameters.Add("@Email", SqlDbType.NText).Value = patient.Email;
            command.Parameters.Add("@ResidentId", SqlDbType.NChar, 15).Value = patient.ResidentId;
            command.Parameters.Add("@Address", SqlDbType.NText).Value = patient.Address;
			command.Parameters.Add("@DoB", SqlDbType.Date).Value = patient.DoB;
            command.Parameters.Add("@Gender", SqlDbType.NChar, 10).Value = patient.Gender;

			int result = command.ExecuteNonQuery();
            return result > 0;

        }

        public bool AddMedicalExaminationForm(Patient patient ,MedicalExaminationForm medicalExaminationForm)
		{
			var connectionString = GetConnectionString();
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();

			DateTime currentDate = DateTime.Now;
            string formatDate = currentDate.ToString("yyyy-MM-dd");


            var command = new SqlCommand("INSERT INTO MedicalExaminationForm (StaffId, DoctorId, Time, Symptom) VALUES ( @StaffId, @DoctorId, @Time, @Symptom)", connection);
			command.Parameters.Add("@StaffId", SqlDbType.Int).Value = 1;
			command.Parameters.Add("@DoctorId", SqlDbType.Int).Value = 1;
			command.Parameters.Add("@Time", SqlDbType.Date).Value = formatDate;
			command.Parameters.Add("@Symptom", SqlDbType.NText).Value = medicalExaminationForm.Symptoms;
			
			int result = command.ExecuteNonQuery();
			return result > 0;
		}
	}
}
