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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using X.PagedList;
using System.Text.Json;

namespace SneakerShoeStoreClient.Controllers
{
    public class UsersController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;
        public UsersController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }

        // GET: Users
        public async Task<IActionResult> Index(int? page)
        {
            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Users");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listProduct = valueArray.ToObject<List<User>>();
            //listProduct.ToPagedList(page ?? 1, 3);
            return View(listProduct.ToPagedList(page ?? 1, 3));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }


            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Users/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };




            var user = JsonSerializer.Deserialize<User>(strData, options);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,Password,Avatar,Name,Gender,Phone,Address,Role,UserStatus")] User user)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiPort + "odata/Users", requestContent);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Users/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var user = JsonSerializer.Deserialize<User>(strData, options);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,Avatar,Name,Gender,Phone,Address,Role,UserStatus")] User user)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiPort + "odata/Users/" + id, requestContent);
           
            return Redirect($"/Users/Details/{id}");
        }

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Users/" + id);
        //    string strData = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true,
        //    };
        //    var user = JsonSerializer.Deserialize<User>(strData, options);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return Problem("Entity set 'SneakerShoeStoreContext.Users'  is null.");
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
