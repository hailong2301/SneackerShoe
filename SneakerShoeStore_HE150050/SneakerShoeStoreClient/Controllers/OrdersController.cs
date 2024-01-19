using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using AutoMapper;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using SneakerShoeStoreAPI.DTO;
using SneakerShoeStoreClient.DTO;
using SneakerShoeStoreClient.DTO.Login;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace SneakerShoeStoreClient.Controllers
{
    public class OrdersController : Controller
    {
        
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;

        public OrdersController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? page)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Orders?$expand=User");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Order>>();
            //listProduct.ToPagedList(page ?? 1, 3);
            return View(listProduct.ToPagedList(page ?? 1, 5));
        }

        public async Task<IActionResult> MyOrder(int? page, int userId)
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
            HttpResponseMessage response = await client.GetAsync(ApiPort + "api/MyOrder/get_orders_by_userid/" + userId);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrders = JsonSerializer.Deserialize<List<Order>>(strData, options);
            //listProduct.ToPagedList(page ?? 1, 3);
            return View(listOrders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Orders/" + id + "?$expand=User");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };




            var order = JsonSerializer.Deserialize<Order>(strData, options); ;
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public  async Task<IActionResult> Create()
        {
            var session = HttpContext.Session.GetString("loginUser");
            int id1 = 0;
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
                id1 = currentUser.Id;

            }
            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Carts?$expand=Product, Size");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listCart = valueArray.ToObject<List<Cart>>();
            int quantity = 0;
            double? totalPrice = 0;
            listCart.Where(c => c.CartId == id1).ToList();
            foreach (var item in listCart)
            {
                quantity += item.Quantity;
                if(item.Product.Discount > 0)
                {
                    totalPrice += item.Product.Price * item.Product.Discount * item.Quantity;
                }
                else
                {
                    totalPrice += item.Product.Price * item.Quantity;
                }
            }

            var orderDTO = new OrderDTO
            {
                UserId = id1,
                Quantity= quantity,
                TotalPrice= totalPrice,
                OrderDate= DateTime.Now,
            };

            string json1 = Newtonsoft.Json.JsonConvert.SerializeObject(orderDTO);
            var requestContent = new StringContent(json1, System.Text.Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync(ApiPort + "odata/Orders", requestContent);

            HttpResponseMessage response2 = await client.GetAsync(ApiPort + "odata/Orders");
            string strData2 = await response2.Content.ReadAsStringAsync();
            JObject json2 = JObject.Parse(strData2);
            JArray valueArray2 = (JArray)json2["value"];
            var listOrder = valueArray2.ToObject<List<Order>>();
            int id = 0;
            var order = listOrder.LastOrDefault();
            id = order.OrderId;

            foreach (var item in listCart)
            {
                double price = 0;
                double amount = 0;
                if (item.Product.Discount > 0)
                {
                    price = (double)(item.Product.Price * item.Product.Discount);
                    amount = (double)(item.Product.Price * item.Product.Discount) * item.Quantity;
                }
                else
                {
                    price = (double)item.Product.Price;
                    amount = (double)(item.Product.Price * item.Quantity);

                }
                var orderDetailDTO = new OrderDetailDTO
                {
                    OrderId = id,
                    ProductId = item.ProductId,
                    SizeId = item.SizeId,
                    Quantity = item.Quantity,
                    Price = price,
                    CreateDate = DateTime.Now,
                    Amount = amount
                };
                //string json3 = Newtonsoft.Json.JsonConvert.SerializeObject(orderDetailDTO);
                //var requestContent2 = new StringContent(json3, System.Text.Encoding.UTF8, "application/json");
                //var response3 = await client.PostAsync(ApiPort + "odata/OrderDetails", requestContent2);
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                _context.OrderDetails.Add(orderDetail);
            }

            foreach (var item in listCart)
            {
                string requestURL = ApiPort + "odata/Carts/" + item.RecordId;
                HttpResponseMessage response5 = await client.DeleteAsync(requestURL);
                var productsize = _context.ProductSizes.Where(p => p.ProductId == item.ProductId && p.SizeId == item.SizeId).FirstOrDefault();
                productsize.Quantity -= item.Quantity;
                _context.ProductSizes.Update(productsize);
                _context.SaveChanges();
            }
                
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,Quantity,TotalPrice,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Orders/" + id + "?$expand=User");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var order = JsonSerializer.Deserialize<Order>(strData, options);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,Quantity,TotalPrice,OrderDate")] Order order)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiPort + "odata/Orders/" + id, requestContent);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", order.UserId);
            return Redirect($"/Orders/Details/{id}");
        }

        //// GET: Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Orders == null)
        //    {
        //        return NotFound();
        //    }


        //    HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Orders/" + id + "?$expand=User");
        //    string strData = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true,
        //    };

        //    var order = JsonSerializer.Deserialize<Order>(strData, options);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Orders == null)
        //    {
        //        return Problem("Entity set 'SneakerShoeStoreContext.Orders'  is null.");
        //    }
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order != null)
        //    {
        //        string requestURL = ApiPort + "odata/Orders/" + id;
        //        HttpResponseMessage response = await client.DeleteAsync(requestURL);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
