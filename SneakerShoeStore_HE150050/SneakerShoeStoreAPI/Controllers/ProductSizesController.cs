using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.Repository.Products;
using DataAccess.Repository.ProductSizes;

namespace SneakerShoeStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private IProductSizeRepository repository = new ProductSizeRepository();

        [HttpGet("proId")]
        public ActionResult<IEnumerable<ProductSize>> GetProducts(int proId) => repository.GetProductSizeByProId(proId);
    }
}
