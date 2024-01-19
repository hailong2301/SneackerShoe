using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CartDAO
    {
        public static List<Cart> GetCarts()
        {
            var listCarts = new List<Cart>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listCarts = context.Carts.Include(c => c.Product).Include(c=>c.Size).ToList();
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
            return listCarts;
        }
        public static void SaveCart(Cart c)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Carts.Add(c);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        public static void UpdateCart(Cart p)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Entry<Cart>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Cart FindCartById(int recordId)
        {
            Cart b = new Cart();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    b = context.Carts.SingleOrDefault(x => x.RecordId == recordId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return b;
        }
        public static void DeleteCart(Cart p)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    var p1 = context.Carts.SingleOrDefault(
                        c => c.RecordId == p.RecordId);
                    context.Carts.Remove(p1);
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
