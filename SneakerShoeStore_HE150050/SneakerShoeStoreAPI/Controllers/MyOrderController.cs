using BusinessObject.Models;
using DataAccess.Repository.OrderDetails;
using DataAccess.Repository.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace SneakerShoeStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyOrderController : ControllerBase
    {
        private IOrderRepository repository = new OrderRepository();
        private IOrderDetailRepository repo = new OrderDetailRepository();
        [HttpGet("get_orders_by_userid/{userId}")]
        public ActionResult<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {

            return repository.GetOrdersByUserId(userId).ToList();
        }

        [HttpGet("get_orderdetail_by_orderid/{orderId}")]
        public ActionResult<IEnumerable<OrderDetail>> GetOrderDetailByOrderId(int orderId)
        {

            return repo.GetOrderDetailByOrderId(orderId).ToList();
        }
    }
}
