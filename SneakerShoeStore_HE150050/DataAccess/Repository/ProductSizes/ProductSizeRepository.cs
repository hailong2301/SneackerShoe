using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ProductSizes
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        public List<ProductSize> GetProductSizeByProId(int proId) => ProductSizeDAO.GetProductSizeByProId(proId);
    }
}
