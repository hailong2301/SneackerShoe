using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Products
{
    public interface IProductRepository
    {
        List<Product> GetProducts();

        void AddProduct(Product p);

        void UpdateProduct(Product p);

        Product GetProductById(int id);

        void DeleteProduct(Product p);
    }
}
