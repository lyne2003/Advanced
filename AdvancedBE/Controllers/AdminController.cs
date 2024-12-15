using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // Index Page
    public IActionResult Index()
    {
        return View();
    }

    // Dashboard Page
    public IActionResult Dashboard()
    {
        return View();
    }

    // Settings Page
    public async Task<IActionResult> Settings()
    {
        var roles = _roleManager.Roles; // Fetch all roles
        var users = await GetNonAdminUsers(); // Fetch users without "admin" role

        var viewModel = new SettingsViewModel
        {
            Roles = roles,
            Users = users
        };

        return View(viewModel);
    }

    // Create a new role
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            TempData["Error"] = "Role name cannot be empty.";
            return RedirectToAction("Settings");
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            TempData["Success"] = "Role created successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to create role. Ensure the role name is unique.";
        }

        return RedirectToAction("Settings");
    }

    // Helper method to fetch users without the "admin" role
    private async Task<List<UserViewModel>> GetNonAdminUsers()
    {
        var allUsers = _userManager.Users.ToList();
        var nonAdminUsers = new List<UserViewModel>();

        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("admin"))
            {
                string status = roles.Any() ? "Client" : "Pending";
                nonAdminUsers.Add(new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Status = status
                });
            }
        }

        return nonAdminUsers;
    }
}

// ViewModels
public class SettingsViewModel
{
    public IEnumerable<IdentityRole> Roles { get; set; }
    public List<UserViewModel> Users { get; set; }
}

public class UserViewModel
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
}
