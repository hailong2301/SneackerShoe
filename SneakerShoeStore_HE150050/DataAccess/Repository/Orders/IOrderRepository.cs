using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Orders
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        List<Order> GetOrdersByUserId(int id);

        void AddOrder(Order o);

        void UpdateOrder(Order  o);

        Order GetOrderById(int id);

        void DeleteOrder(Order o);
    }
}
