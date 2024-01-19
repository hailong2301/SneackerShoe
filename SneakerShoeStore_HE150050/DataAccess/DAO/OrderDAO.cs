using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        public static List<Order> GetOrders()
        {
            var listOrders = new List<Order>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listOrders = context.Orders.Include(o => o.User).ToList();
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
            return listOrders;
        }
        public static Order FindOrderById(int orderId)
        {
            Order o = new Order();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    o = context.Orders.Include(o => o.User).SingleOrDefault(x => x.OrderId == orderId);
                    //b.Brand = BrandDAO.FindPublisherById(b.pub_id);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return o;
        }

        public static void SaveOrder(Order o)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Orders.Add(o);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        public static void UpdateOrder(Order o)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Entry<Order>(o).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteOrder(Order o)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    var o1 = context.Orders.SingleOrDefault(
                        c => c.OrderId == o.OrderId);
                    context.Orders.Remove(o1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Order> GetOrdersByUserId(int id)
        {
            var listOrders = new List<Order>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listOrders = context.Orders.Where(o => o.UserId == id).ToList();
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
            return listOrders;
        }
    }
}
