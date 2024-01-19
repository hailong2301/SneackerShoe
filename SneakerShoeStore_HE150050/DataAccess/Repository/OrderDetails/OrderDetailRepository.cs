using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.OrderDetails
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail o) => OrderDetailDAO.SaveOrderDetail(o);

        public List<OrderDetail> GetOrderDetailByOrderId(int id) => OrderDetailDAO.GetOrderDetailByOrderId(id);
    }
}
