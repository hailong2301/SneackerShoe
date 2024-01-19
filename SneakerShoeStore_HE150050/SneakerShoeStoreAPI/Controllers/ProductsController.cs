using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using DataAccess.Repository.Products;
using AutoMapper;
using SneakerShoeStoreAPI.DTO;
using DataAccess.DAO;

namespace SneakerShoeStoreAPI.Controllers
{

    public class ProductsController : ODataController
    {
        private IProductRepository repository = new ProductSRepository();


        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
        
            return repository.GetProducts().AsQueryable().ToList();
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<Product> Get(int key)
        {
            return repository.GetProductById(key);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductDTO productDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            var product = mapper.Map<ProductDTO, Product>(productDto);
            repository.AddProduct(product);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(int key, [FromBody] ProductDTO productDto)
        {
            var product = repository.GetProductById(key);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            product = mapper.Map<ProductDTO, Product>(productDto);
            product.ProductId = key;
            repository.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int key)
        {
            var product = repository.GetProductById(key);
            if (product == null)
            {
                return NotFound();
            }
            repository.DeleteProduct(product);
            return NoContent();
        }
    }
}
