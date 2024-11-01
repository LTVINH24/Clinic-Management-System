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
        public string Authentication(string username , string password )
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
    }
}
