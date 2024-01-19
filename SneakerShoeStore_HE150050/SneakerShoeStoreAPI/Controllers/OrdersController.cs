using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using AutoMapper;
using DataAccess.Repository.Products;
using Microsoft.AspNetCore.OData.Query;
using SneakerShoeStoreAPI.DTO;
using DataAccess.Repository.Orders;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace SneakerShoeStoreAPI.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly SneakerShoeStoreContext _context;
        private IOrderRepository repository = new OrderRepository();

        public OrdersController(SneakerShoeStoreContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {

            return repository.GetOrders().AsQueryable().ToList();
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<Order> Get(int key)
        {
            return repository.GetOrderById(key);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderDTO orderDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            var order = mapper.Map<OrderDTO, Order>(orderDto);
            repository.AddOrder(order);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(int key, [FromBody] OrderDTO orderDto)
        {
            var order = repository.GetOrderById(key);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            order = mapper.Map<OrderDTO, Order>(orderDto);
            order.OrderId = key;
            repository.UpdateOrder(order);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int key)
        {
            var order = repository.GetOrderById(key);
            if (order == null)
            {
                return NotFound();
            }
            repository.DeleteOrder(order);
            return NoContent();
        }

        [HttpGet("get_orders_by_userid/{userId}")]
        public ActionResult<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            return repository.GetOrdersByUserId(userId).ToList();
        }

    }
}
