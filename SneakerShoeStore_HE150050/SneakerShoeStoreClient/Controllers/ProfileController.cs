using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SneakerShoeStoreAPI.DTO;
using SneakerShoeStoreClient.Authorization;
using SneakerShoeStoreClient.DTO.Login;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SneakerShoeStoreClient.Controllers
{
    [CustomerAuthorization]
    public class ProfileController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;

        public ProfileController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
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
            HttpResponseMessage response = await client.GetAsync(ApiPort + "odata/Users/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var product = JsonSerializer.Deserialize<User>(strData, options);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string email,string avatar, string password ,string name, int gender, string phone, string address, int status, string role)
        {
            bool userStatus = false;
            if (status == 1)
            {
                userStatus = true;
            }
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
            bool gender1 = true;
            if(gender == 2)
            {
                gender1 = false;
            }
            UserDTO userdto = new UserDTO
            {
                Email = email,
                Name = name,
                Password = password,
                Address = address,
                Phone = phone,
                UserStatus = userStatus,
                Gender = gender1,
                Avatar = avatar,
                Role = role
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(userdto);
            //string json1 = JsonSerializer.Serialize(product);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiPort + "odata/Users/" + id, requestContent);
  
            return Redirect($"/Profile/Index/{id}");
        }

        public async Task<IActionResult> ChangePass()
        {
            var session = HttpContext.Session.GetString("loginUser");
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
  
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePass(string oldpass, string newpass, string repass)
        {
            var session = HttpContext.Session.GetString("loginUser");
            string email = null;
            if (session != null)
            {
                UserModel currentUser = JsonSerializer.Deserialize<UserModel>(session);
                ViewData["user"] = currentUser.Email;
                email = currentUser.Email;
                ViewData["role"] = currentUser.Role;
                ViewData["userId"] = currentUser.Id;
            }
            HttpResponseMessage response = await client.GetAsync(ApiPort + $"odata/Users?$filter=Email eq '{email}'");
            string strData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(strData);
            JArray valueArray = (JArray)json["value"];
            var listUsers = valueArray.ToObject<List<User>>();
            User user = null;
            if (listUsers.Count() != 0)
            {
                user = listUsers[0];
            }
            if (user != null)
            {
                if (user.Password.Equals(oldpass) == false)
                {
                    ViewBag.Er = "oldpass is not correct";

                    return View();
                }
                else
                {
                    if (repass.Equals(newpass) == false)
                    {
                        ViewBag.Er = "Repassword is not correct";
                        return View();
                    }
                    else
                    {
                        user.Password = newpass;
                        _context.Users.Update(user);
                        _context.SaveChanges();
                        ViewBag.Mess = "Changepass successfully";
                        return View();
                    }
                }
            }


            return View();
        }
    }
}
