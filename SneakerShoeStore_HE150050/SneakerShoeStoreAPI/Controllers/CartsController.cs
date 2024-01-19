using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DataAccess.Repository.Carts;
using AutoMapper;
using SneakerShoeStoreAPI.DTO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.OData.Query;

namespace SneakerShoeStoreAPI.Controllers
{
    public class CartsController : ODataController
    {
        ICartRepository repository = new CartRepository();
        private IMapper _mapper;
        public CartsController(IMapper mapper)
        {
   
            _mapper = mapper;
          
        }
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<Cart>> Get()
        {

            return repository.GetCarts().AsQueryable().ToList();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CartDTO cartDTO)
        {
            var cart = _mapper.Map<CartDTO, Cart>(cartDTO);

            repository.AddCart(cart);
            return NoContent();
        }


        [HttpPut]
        public IActionResult Put(int key, [FromBody] CartDTO cartDto)
        {
            var product = repository.GetCartByRecordId(key);
         
          
            product = _mapper.Map<CartDTO, Cart>(cartDto);
            product.RecordId = key;
            repository.UpdateCart(product);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int key)
        {
            var product = repository.GetCartByRecordId(key);
            if (product == null)
            {
                return NotFound();
            }
            repository.DeleteCart(product);
            return NoContent();
        }
    }
}
