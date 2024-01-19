using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            var listUsers = new List<User>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listUsers = context.Users.ToList();
                    //foreach (var item in listUsers)
                    //{
                    //    item.Brand = BrandDAO.FindBrandById(item.BrandId);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listUsers;
        }
        public static User FindUserById(int userId)
        {
            User b = new User();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    b = context.Users.SingleOrDefault(x => x.UserId == userId);
                    //b.Brand = BrandDAO.FindPublisherById(b.pub_id);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return b;
        }

        public static void SaveUser(User u)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Users.Add(u);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        public static void UpdateUser(User u)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Entry<User>(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteUser(User u)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    var u1 = context.Users.SingleOrDefault(
                        c => c.UserId == u.UserId);
                    context.Users.Remove(u1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
