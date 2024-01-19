using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Users
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        void AddUser(User u);

        void UpdateUser(User u);

        User GetUserById(int id);

        void DeleteUser(User u);
    }
}
