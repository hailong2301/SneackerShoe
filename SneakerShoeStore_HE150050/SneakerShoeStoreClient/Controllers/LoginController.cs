using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using SneakerShoeStoreClient.DTO.Login;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SneakerShoeStoreClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly SneakerShoeStoreContext _context;
        private readonly HttpClient client = null;
        private readonly IConfiguration configuration;
        private string ApiPort = "";
        private string EmailAdmin = null;
        private string PasswordAdmin = null;
        public LoginController(SneakerShoeStoreContext context, IConfiguration configuration)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            this.configuration = configuration;
            ApiPort = configuration.GetSection("ApiHost").Value;
            EmailAdmin = configuration.GetSection("AdminAccount").GetSection("email").Value;
            PasswordAdmin = configuration.GetSection("AdminAccount").GetSection("password").Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            UserModel currentUser;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (request.email.Equals(EmailAdmin) && request.password.Equals(PasswordAdmin))
            {
                currentUser = new UserModel
                {
                    Email = request.email,
                    Password = request.password,
                    Role = "admin"

                };

                var loginUser = JsonSerializer.Serialize(currentUser);
                HttpContext.Session.SetString("loginUser", loginUser);
                ViewData["user"] = currentUser.Email;
                ViewData["role"] = currentUser.Role;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(ApiPort + $"odata/Users?$filter=Email eq '{request.email}' and Password eq '{request.password}'");
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
                        currentUser = new UserModel
                        {
                            Id = user.UserId,
                            Email = user.Email,
                            Password = user.Password,
                            Role = user.Role
                        };
                        var loginUser = JsonSerializer.Serialize(currentUser);
                        HttpContext.Session.SetString("loginUser", loginUser);
                        ViewData["user"] = currentUser.Email;
                        ViewData["role"] = currentUser.Role;
                        ViewData["userId"] = currentUser.Id;
                        return RedirectToAction("Index", "Shop");
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return Ok("Tai khoan hoac mat khau ban nhap sai !!!");
        }
    }
}
