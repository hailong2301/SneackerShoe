using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SneakerShoeStoreAPI.DTO;
using SneakerShoeStoreClient.DTO.Login;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SneakerShoeStoreClient.Controllers
{
    public class RegisterController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private IMapper _mapper;

        public RegisterController(SneakerShoeStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string pass, string name, int gender, string phone, string address)
        {
            HttpResponseMessage response1 = await client.GetAsync(ApiPort + "odata/Users");
            string strData = await response1.Content.ReadAsStringAsync();
            JObject json1 = JObject.Parse(strData);
            JArray valueArray = (JArray)json1["value"];
            var listUser = valueArray.ToObject<List<User>>();
            foreach (var item in listUser)
            {
                if (item.Email.ToLower().Equals(email.ToLower()))
                {
                    ViewBag.Er = "Email already exist!";
                    return View();
                }
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
            if (gender == 2)
            {
                gender1 = false;
            }
            UserDTO userdto = new UserDTO
            {
                Email = email,
                Name = name,
                Password = pass,
                Address = address,
                Phone = phone,
                UserStatus = true,
                Gender = gender1,
                Avatar = null,
                Role = "Customer"
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(userdto);
            //string json1 = JsonSerializer.Serialize(product);
            var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiPort + "odata/Users", requestContent);

            return Redirect($"/Login/Index");
        }
    }
}
