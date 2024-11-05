using Clinic_Management_System.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Clinic_Management_System.Service.DataAccess.IDao;

namespace Clinic_Management_System.Service.DataAccess
{
    public class SqlServerDao : IDao
    {
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
            SELECT count(*) over() as Total, id, name, role, username,password,phone,birthday,address,gender,entropy
            FROM EndUser
            WHERE Name like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
            var command = new SqlCommand(sql, connection);
            AddParameters(command, ("@Skip", (page - 1) * rowsPerPage), ("@Take", rowsPerPage), ("@Keyword", $"%{keyword}%"));
            var reader = command.ExecuteReader();
            int count = -1;
            string entropy = "";
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
                entropy = (string)reader["entropy"];
                user.password = DecryptionPassword((string)reader["password"], entropy);
                result.Add(user);
            }

            connection.Close();
            return new Tuple<List<User>, int>(
                result, count
            );
        }
        public (int,string,string,string,string,string) Authentication(string username , string password )
        {

            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT id,name, role,password,phone,birthday,gender,address,entropy FROM EndUser WHERE username = @Username";
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            var reader = command.ExecuteReader();

            int id= 0;
            string name = "";
            string role = "";
            string encryptedPasswordInBase64 = "";
            string phone = "";
            DateTime birthday;
            string gender;
            string address;
            string entropyInBase64 = "";
            if (reader.Read())
            {
                id =(int) reader["id"];
                name =(string)reader["name"];
                role = reader["role"].ToString();
                encryptedPasswordInBase64 = reader["password"].ToString();
                phone=reader["phone"].ToString();
                birthday = (DateTime)reader["birthday"];
                gender = reader["gender"].ToString() ;
                address = reader["address"].ToString() ;
                entropyInBase64 = reader["entropy"].ToString();
                connection.Close();
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordInBase64);
                var entropyInBytes = Convert.FromBase64String(entropyInBase64);
                var passwordInBytes = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                var passwordGetFromDatabase = Encoding.UTF8.GetString(passwordInBytes);
                if (password == passwordGetFromDatabase)
                {
                    return (id,name,role,phone,gender,address);
                }
                return (0,"","","","","");
            }
            else
            {
                connection.Close();
                return(0, "", "", "", "", "");
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
        public bool UpdateUser(User info, string entropyUserEdit)
        {
            var connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var sql = "update EndUser set name=@name, role=@role,username=@username,password=@password,phone=@phone,birthday=@birthday,address=@address,gender=@gender,entropy=@entropy where id=@id";
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
                ("@gender", info.gender),
                ("@entropy", entropyUserEdit)
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
        public string DecryptionPassword(string encryptedPasswordInBase64, string entropyInBase64)
        {
            var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordInBase64);
            var entropyInBytes = Convert.FromBase64String(entropyInBase64);
            var passwordInBytes = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
            var password = Encoding.UTF8.GetString(passwordInBytes);
            return password;
        }
    }
    
}
