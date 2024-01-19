using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailDAO
    {
        public static void SaveOrderDetail(OrderDetail od)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.OrderDetails.Add(od);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }
        public static List<OrderDetail> GetOrderDetailByOrderId(int id)
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listOrderDetails = context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).Include(o => o.Size).ToList();
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
            return listOrderDetails;
        }
    }
}
