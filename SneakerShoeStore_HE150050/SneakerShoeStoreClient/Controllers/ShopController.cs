using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json.Linq;
using SneakerShoeStoreClient.DTO.Login;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace SneakerShoeStoreClient.Controllers
{
    public class ShopController : Controller
    {
        private readonly HttpClient client = null;
        private string Api = "";
        private readonly IConfiguration configuration;
        private readonly SneakerShoeStoreContext _context;
        public ShopController(SneakerShoeStoreContext context, IConfiguration configuration)
        {
         
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            Api = configuration.GetSection("ApiHost").Value;
        }
        public async Task<IActionResult> Index(int? currentpage, string? search, int? brandId, double? fromPrice, double? toPrice, int? sort)
        {
            HttpResponseMessage response = await client.GetAsync(Api + "odata/Products?$expand=Brand");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Product>>();

            HttpResponseMessage response1 = await client.GetAsync(Api + "odata/Brands");
            string strData1 = await response1.Content.ReadAsStringAsync();
            JObject json1 = JObject.Parse(strData1);
            JArray valueArray1 = (JArray)json1["value"];
            var listBrand = valueArray1.ToObject<List<Brand>>();

            int page = 1;
            if (currentpage != null)
            {
                page = (int)currentpage;
            }
            List<Product> products = new List<Product>();
            foreach (var item in listProduct)
            {
                products.Add(item);
            }
            if(search!= null && search != "")
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(search.ToLower())).ToList();
            }
            if(brandId != null)
            {
                products = products.Where(p => p.BrandId == brandId).ToList();

            }
          

            if (fromPrice != null && toPrice == null)
            {
                products = products.Where(p => p.Price >= fromPrice).ToList();
            }
            if (fromPrice == null && toPrice != null)
            {
                products = products.Where(p => p.Price <= toPrice).ToList();
            }
            if (fromPrice != null && toPrice != null)
            {
                products = products.Where(p =>p.Price>= fromPrice && p.Price <= toPrice).ToList();
            }
            if(sort != null)
            {
                if(sort == 1)
                {
                    products = products.OrderByDescending(p=>p.ProductName).ToList();
                }else if(sort == 2)
                {
                    products = products.OrderByDescending(p => p.Price).ToList();
                }
                else if (sort == 3)
                {
                    products = products.OrderBy(p => p.ProductName).ToList();
                }
                else
                {
                    products = products.OrderBy(p => p.Price).ToList();
                }

            }

            int size = 6;
            int pages = (int)Math.Ceiling((double)products.Count / size);
            
             products = products.Skip((page - 1) * size).Take(size).ToList();
            ViewData["totalpage"] = pages;
            ViewData["page"] = page;
            ViewData["search"] = search;
            ViewData["brands"] = listBrand;
            ViewData["brandid"] = brandId;
            ViewData["fromPrice"] = fromPrice;
            ViewData["toPrice"] = toPrice;
            ViewData["sort"] = sort;
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;

            }
            return View(products);
        }
        public async Task<IActionResult> Detail(int id, int? type, int? size)
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id; 

            }

            HttpResponseMessage response = await client.GetAsync(Api + "odata/Products/" + id + "?$expand=Brand");
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


            HttpResponseMessage response1 = await client.GetAsync(Api + "odata/Products?$expand=Brand");
            string strData1 = await response1.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData1);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<Product>>();

            listProduct = listProduct.Where(p=>p.BrandId == product.BrandId).ToList();
            ViewData["productsizes"] = productSizes;
            ViewData["products"] = listProduct;
            if (product == null)
            {
                return NotFound();
            }
            //if (i == null)
            //{
            //    i = 1;
            //}
            //if (type == 1)
            //{
            //    if (i > 1)
            //    {
            //        i--;
            //    }
            //}
            //else if(type == 2)  
            //{
            //    i++;
            //}
            return View(product);
        }
    }
}
