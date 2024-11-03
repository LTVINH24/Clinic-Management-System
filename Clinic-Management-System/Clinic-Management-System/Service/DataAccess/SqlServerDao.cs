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
        public (int,string) Authentication(string username , string password )
        {

            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT id, role,password,entropy FROM EndUser WHERE username = @Username";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            var reader = command.ExecuteReader();
            string encryptedPasswordInBase64 = "";
            string entropyInBase64 = "";
            string role = "";
            int id= 0;
            if (reader.Read())
            {
                id =(int) reader["id"];
                encryptedPasswordInBase64 = reader["password"].ToString();
                entropyInBase64 = reader["entropy"].ToString();
                role = reader["role"].ToString();
                connection.Close();
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordInBase64);
                var entropyInBytes = Convert.FromBase64String(entropyInBase64);
                var passwordInBytes = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                var passwordGetFromDatabase = Encoding.UTF8.GetString(passwordInBytes);
                if (password == passwordGetFromDatabase)
                {
                    return (id,role);
                }
                return (0,"");
            }
            else
            {
                connection.Close();
                return(0,"");
            }
        
        }
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
        public bool CreateUser(User user, string password,string entropy)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"insert into EndUser(name,role,username,password,phone,birthday,address,gender,entropy) values(@name,@role,@username,@password,@phone,@birthday,@address,@gender,@entropy) ";
            var command = new SqlCommand(query, connection);
            AddParameters(command, 
                ("@name", user.name),
                ("@role", user.role),
                ("@username", user.username),
                ("@password", password),
                ("@phone", user.phone),
                ("@birthday", user.birthday),
                ("@address", user.address),
                ("@gender", user.gender),
                ("@entropy", entropy)
                );
            int count = command.ExecuteNonQuery();
            bool success = count == 1;
            connection.Close();
            return success;
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
    }
}
