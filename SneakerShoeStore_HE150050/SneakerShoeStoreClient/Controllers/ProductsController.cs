using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Security.Cryptography;
using DataAccess.DAO;
using Microsoft.CodeAnalysis;
using X.PagedList;
using SneakerShoeStoreAPI.DTO;
using AutoMapper;
using SneakerShoeStoreClient.Authorization;

namespace SneakerShoeStoreClient.Controllers
{
    [CustomAuthorizationFilter]
    public class ProductsController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;
        public ProductsController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? page)
        {

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Products?$expand=Brand");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Product>>();
            //listProduct.ToPagedList(page ?? 1, 3);
            return View(listProduct.ToPagedList(page ?? 1, 3));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Products/" + id + "?$expand=Brand");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

 

    
            var product = JsonSerializer.Deserialize<Product>(strData, options);
            var productSizes = _context.ProductSizes.Where(p => p.ProductId == id && p.Quantity > 0).ToList();
            foreach (var item in productSizes)
            {
                item.Size = _context.Sizes.SingleOrDefault(x => x.SizeId == item.SizeId);
            }
            
            ViewData["productsizes"] = productSizes;
            if (product == null)
            {
                return NotFound();
            }
            ViewData["imgurl"] = product.Image;
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,Price,Discount,Image,Description,BrandId,CreatedDate")] ProductDTO product)
        {
            // Convert the payload to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(product);

            // Create the HTTP request
            //var request = new HttpRequestMessage(HttpMethod.Post, ApiPort + "odata/Products");
            //request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //request.AddHeader("content-type", "application/json");
            // Send the request and get the response
            // HttpResponseMessage response = await client.SendAsync(request);

            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiPort + "odata/Products", requestContent);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Products/" + id + "?$expand=Brand");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var product = JsonSerializer.Deserialize<Product>(strData, options);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["proId"] = id;
            var productDTO = _mapper.Map<Product, ProductDTO>(product);
            
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            return View(productDTO);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductName,Price,Discount,Image,Description,BrandId,CreatedDate")] ProductDTO product)
        {
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            string json1 = JsonSerializer.Serialize(product);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiPort + "odata/Products/" + id, requestContent);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            return Redirect($"/Products/Details/{id}");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Products/" + id + "?$expand=Brand");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var product = JsonSerializer.Deserialize<Product>(strData, options);
            if (product == null)
            {
                return NotFound();
            }


            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'SneakerShoeStoreContext.Products'  is null.");
            }
            if (id != 0)
            {
                string requestURL = ApiPort + "odata/Products/" + id;
                HttpResponseMessage response = await client.DeleteAsync(requestURL);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
