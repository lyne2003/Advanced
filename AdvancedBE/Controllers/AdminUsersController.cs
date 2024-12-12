using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedBE.Controllers
{
    [Authorize(Roles = "admin")] // Ensure only admins can access this controller
    public class AdminUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all users
            var users = await _userManager.Users.ToListAsync();

            // Initialize lists for admins and clients
            var admins = new List<IdentityUser>();
            var clients = new List<IdentityUser>();

            // Separate users based on claims
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("admin"))
                {
                    admins.Add(user);
                }
                else
                {
                    clients.Add(user);
                }
            }

            // Use strongly-typed models instead of ViewBag for better maintainability
            var model = new AdminUsersViewModel
            {
                Admins = admins,
                Clients = clients
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ClientDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User ID is required.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create a detailed view model to fetch user-specific information
            var clientDetails = new ClientDetailsViewModel
            {
                User = user,
                Roles = await _userManager.GetRolesAsync(user),
                Claims = await _userManager.GetClaimsAsync(user)
            };

            return View(clientDetails);
        }
    }

    // Strongly-typed view models for better maintainability and performance
    public class AdminUsersViewModel
    {
        public List<IdentityUser> Admins { get; set; } = new List<IdentityUser>();
        public List<IdentityUser> Clients { get; set; } = new List<IdentityUser>();
    }

    public class ClientDetailsViewModel
    {
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }
}
