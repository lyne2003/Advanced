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

    private async Task<List<UserViewModel>> GetNonAdminUsers()
    {
        var allUsers = _userManager.Users.ToList(); // Fetch all users
        var nonAdminUsers = new List<UserViewModel>();

        foreach (var user in allUsers)
        {
            // Fetch user claims
            var claims = await _userManager.GetClaimsAsync(user);
            string status = "Pending"; // Default to pending

            // Determine status based on claims
            if (claims.Any(c => c.Value == "admin"))
            {
                // continue; // Skip admin users
                status = "Admin";
            }

            if (claims.Any(c => c.Value == "client"))
            {
                status = "Client";
            }

            nonAdminUsers.Add(new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Status = status
            });
        }

        return nonAdminUsers;
    }

    //////////////////////////////////////////////////////
    [HttpPost]
    public async Task<IActionResult> UpdateUserClaim(string userId, string claimValue)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(claimValue))
        {
            TempData["Error"] = "Invalid user ID or claim value.";
            return RedirectToAction("Settings");
        }

        // Find the user
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction("Settings");
        }

        // Remove existing role claim
        var claims = await _userManager.GetClaimsAsync(user);
        var existingRoleClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        if (existingRoleClaim != null)
        {
            await _userManager.RemoveClaimAsync(user, existingRoleClaim);
        }

        // Add new role claim
        var newClaim = new System.Security.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", claimValue);
        var result = await _userManager.AddClaimAsync(user, newClaim);

        if (result.Succeeded)
        {
            TempData["Success"] = $"User claim updated to '{claimValue}'.";
        }
        else
        {
            TempData["Error"] = "Failed to update claim.";
        }

        return RedirectToAction("Settings");
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
