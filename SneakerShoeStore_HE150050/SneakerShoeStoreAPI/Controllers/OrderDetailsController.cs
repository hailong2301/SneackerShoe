using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AutoMapper;
using NuGet.Protocol.Core.Types;
using SneakerShoeStoreAPI.DTO;
using DataAccess.Repository.Carts;
using DataAccess.Repository.OrderDetails;

namespace SneakerShoeStoreAPI.Controllers
{
    public class OrderDetailsController : ODataController
    {
        IOrderDetailRepository repository = new OrderDetailRepository();
        private IMapper _mapper;
        public OrderDetailsController(IMapper mapper)
        {

            _mapper = mapper;

        }
        [HttpPost]
        public IActionResult Post([FromBody] OrderDetailDTO productDto)
        {
          
        
            var product = _mapper.Map<OrderDetailDTO, OrderDetail>(productDto);
            repository.AddOrderDetail(product);
            return NoContent();
        }
    }
}
