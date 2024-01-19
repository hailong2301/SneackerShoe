using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Brands
{
    public class BrandRepository : IBrandRepository
    {
        public List<Brand> GetBrands() => BrandDAO.GetBrands();
    }
}
