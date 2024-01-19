using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
   public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listProducts = context.Products.ToList();
                    foreach (var item in listProducts)
                    {
                        item.Brand = BrandDAO.FindBrandById(item.BrandId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProducts;
        }
        public static Product FindProductById(int productId)
        {
            Product b = new Product();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    b = context.Products.SingleOrDefault(x => x.ProductId == productId);
                    b.Brand = BrandDAO.FindBrandById(b.BrandId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return b;
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Products.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    var p1 = context.Products.SingleOrDefault(
                        c => c.ProductId == p.ProductId);
                    context.Products.Remove(p1);
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
