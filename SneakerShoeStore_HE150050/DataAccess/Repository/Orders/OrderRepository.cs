using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Orders
{
    public class OrderRepository : IOrderRepository
    {
        public void AddOrder(Order o) => OrderDAO.SaveOrder(o);

        public void DeleteOrder(Order o) => OrderDAO.DeleteOrder(o);

        public Order GetOrderById(int id) => OrderDAO.FindOrderById(id);

        public List<Order> GetOrders() => OrderDAO.GetOrders();

        public List<Order> GetOrdersByUserId(int id) => OrderDAO.GetOrdersByUserId(id);


        public void UpdateOrder(Order o) => OrderDAO.UpdateOrder(o);
    }
}
