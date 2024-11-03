using Clinic_Management_System.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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



		private void AddParameters(SqlCommand command, params (string ParameterName, object Value)[] parameters)
		{
			foreach (var (parameterName, value) in parameters)
			{
				command.Parameters.AddWithValue(parameterName, value);
			}
		}


		public bool AddPatient(Patient patient)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();



            var command = new SqlCommand("INSERT INTO Patient (Name , Email , ResidentId, Address , Birthday , Gender) VALUES (@Name , @Email , @ResidentId , @Address , @DoB , @Gender)", connection);
            AddParameters(command,
                ("@Name", patient.Name),
                ("@Email", patient.Email),
                ("@ResidentId", patient.ResidentId),
                ("@Address", patient.Address),
                ("@DoB", patient.DoB),
                ("@Gender", patient.Gender));

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


            var command = new SqlCommand("INSERT INTO MedicalExaminationForm (DoctorId, Time, Symptom) VALUES ( @DoctorId, @Time, @Symptom)", connection);

            AddParameters(command,
                ("@DoctorId", medicalExaminationForm.DoctorId),
                ("@Time", formatDate),
                ("@Symptom", medicalExaminationForm.Symptoms));
			
			int result = command.ExecuteNonQuery();
			return result > 0;
		}
	}
}
