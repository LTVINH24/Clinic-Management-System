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

        Tuple<List<User>, int> GetUsers(
            int page, int rowsPerPage,
            string keyword,
            Dictionary<string, SortType> sortOptions
        );
        (int, string, string, string, string, string) Authentication (string username, string password);
        bool CreateUser( User user, string password, string entropy);
        bool CheckUserExists(string username);
        bool UpdateUser(User info,string entropyUserEdit);
    }
}
