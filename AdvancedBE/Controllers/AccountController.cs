using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvancedBE.Controllers
{
    [Authorize] // Ensures only logged-in users can access this
    public class AccountController : Controller
    {
        public IActionResult UserId()
        {
            // Retrieve the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Pass it to the view
            ViewBag.UserId = userId;

            return View();
        }
    }
}
