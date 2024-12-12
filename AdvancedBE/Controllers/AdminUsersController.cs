using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdvancedBE.Data;

namespace AdvancedBE.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminUsersController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: AdminUsers/Index
        public async Task<IActionResult> Index(string view = "admins")
        {
            // Get admins (users with ClaimValue "admin")
            var admins = await (from user in _userManager.Users
                                join claim in _context.UserClaims
                                on user.Id equals claim.UserId
                                where claim.ClaimValue == "admin"
                                select user).ToListAsync();

            // Get clients (users with ClaimValue "client")
            var clients = await (from user in _userManager.Users
                                 join claim in _context.UserClaims
                                 on user.Id equals claim.UserId
                                 where claim.ClaimValue == "client"
                                 select user).ToListAsync();

            ViewBag.ViewType = view.ToLower();
            ViewBag.Admins = admins;
            ViewBag.Clients = clients;

            return View();
        }
    }
}
