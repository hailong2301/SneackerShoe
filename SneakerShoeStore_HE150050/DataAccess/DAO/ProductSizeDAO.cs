using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductSizeDAO
    {
        public static List<ProductSize> GetProductSizeByProId(int proId)
        {
            var listProductSize = new List<ProductSize>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listProductSize = context.ProductSizes.Where(p=>p.ProductId == proId && p.Quantity > 0).Include(p => p.Size).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProductSize;
        }
    }
}
