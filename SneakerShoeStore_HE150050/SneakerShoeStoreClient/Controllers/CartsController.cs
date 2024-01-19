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
using SneakerShoeStoreAPI.DTO;
using Newtonsoft.Json.Linq;
using SneakerShoeStoreClient.DTO.Login;
using System.Text.Json;
using SneakerShoeStoreClient.Authorization;

namespace SneakerShoeStoreClient.Controllers
{
    [CustomerAuthorization]
    public class CartsController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string Api = "";
        private IMapper _mapper;

        public CartsController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            Api = configuration.GetSection("ApiHost").Value;
        }

        // GET: Carts
        public async Task<IActionResult> Index(int id)
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
            HttpResponseMessage response = await client.GetAsync(Api + "odata/Carts?$expand=Product, Size");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Cart>>();

            listProduct.Where(c => c.CartId== id).ToList(); 
            return View(listProduct);

        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.Size)
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

   

        // GET: Carts/Create
        //public IActionResult Create()
        //{
        //    ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
        //    ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeId");
        //    return View();
        //}

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int size, int proId)
        {
            var session = HttpContext.Session.GetString("loginUser");
            int id = 0;
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
                id = currentUser.Id;

            }
            else
            {
                return Redirect($"/Login/Index");
            }
            
            CartDTO cart = new CartDTO
            {
                CartId = id,
                ProductId = proId,
                Quantity = 1,
                SizeId = size

            };

            HttpResponseMessage response1 = await client.GetAsync(Api + "odata/Carts?$expand=Product, Size");
            string strData = await response1.Content.ReadAsStringAsync();
            JObject json1 = JObject.Parse(strData);
            JArray valueArray = (JArray)json1["value"];
            var listCart = valueArray.ToObject<List<Cart>>();
            listCart.Where(p => p.CartId == 1).ToList();

            var cart1 = listCart.Where(p => p.ProductId == proId && p.SizeId == size).FirstOrDefault();
            if(cart1 == null)
            {
                // Convert the payload to JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
                var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Api + "odata/Carts", requestContent);
            }
            else
            {
                var productsize = _context.ProductSizes.FirstOrDefault(p => p.ProductId == cart1.ProductId && p.SizeId == cart1.SizeId);
                if (cart1.Quantity < productsize.Quantity)
                {
                    cart1.Quantity += 1;
                }
                var cart2 = _mapper.Map<Cart, CartDTO>(cart1);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(cart2);
                //string json1 = JsonSerializer.Serialize(product);
                var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync(Api + "odata/Carts/" + cart1.RecordId, requestContent);
            }

         
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? recordId, int type)

        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
            HttpResponseMessage response = await client.GetAsync(Api + "odata/Carts?$expand=Product, Size");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Cart>>();

            
            if (recordId != null)
            {
                var cart = listProduct.FirstOrDefault(c => c.RecordId == recordId);
                if (type == 1)
                {
                    if (cart.Quantity > 0)
                    {
                        cart.Quantity -= 1;
                    }
                    if (cart.Quantity == 0)
                    {
                        return Redirect($"/Carts/Delete/{recordId}");
                    }
                }
                else if (type == 2)
                {
                    var productsize = _context.ProductSizes.FirstOrDefault(p => p.ProductId == cart.ProductId && p.SizeId == cart.SizeId);
                    if(cart.Quantity < productsize.Quantity)
                    {
                        cart.Quantity += 1;
                    }
                  

                }

                var cart2 = _mapper.Map<Cart, CartDTO>(cart);
                string json1 = Newtonsoft.Json.JsonConvert.SerializeObject(cart2);
                //string json1 = JsonSerializer.Serialize(product);
                var requestContent = new StringContent(json1, System.Text.Encoding.UTF8, "application/json");
                var response1 = await client.PutAsync(Api + "odata/Carts/" + cart.RecordId, requestContent);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,CartId,ProductId,SizeId,Quantity")] Cart cart)
        {
            if (id != cart.RecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.RecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cart.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeId", cart.SizeId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }


            if (id != 0)
            {
                string requestURL = Api + "odata/Carts/" + id;
               HttpResponseMessage response = await client.DeleteAsync(requestURL);
            }
            return RedirectToAction(nameof(Index));
}

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'SneakerShoeStoreContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Carts?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
