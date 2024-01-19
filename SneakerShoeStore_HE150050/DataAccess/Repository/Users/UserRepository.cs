using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(User u) => UserDAO.SaveUser(u);
        public void DeleteUser(User u) => UserDAO.DeleteUser(u);

        public User GetUserById(int id) => UserDAO.FindUserById(id);

        public List<User> GetUsers() => UserDAO.GetUsers();

        public void UpdateUser(User u) => UserDAO.UpdateUser(u);
    }
}
