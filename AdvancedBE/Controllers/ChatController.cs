using Microsoft.AspNetCore.Mvc;

namespace AdvancedBE.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
