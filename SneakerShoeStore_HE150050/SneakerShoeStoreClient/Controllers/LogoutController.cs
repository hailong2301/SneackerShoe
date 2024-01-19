using Microsoft.AspNetCore.Mvc;

namespace SneakerShoeStoreClient.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("loginUser");
            return RedirectToAction("Index", "shop");
        }
    }
}
