using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    // Action to display the Settings page
    public IActionResult Settings()
    {
        var roles = _roleManager.Roles; // Fetch all roles
        return View(roles); // Pass roles to the view
    }

    // Action to handle the creation of a new role
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            ModelState.AddModelError("", "Role name cannot be empty.");
            return RedirectToAction("Settings");
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            TempData["Success"] = "Role created successfully.";
            return RedirectToAction("Settings");
        }

        TempData["Error"] = "Failed to create role. Ensure the role name is unique.";
        return RedirectToAction("Settings");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            TempData["Error"] = "Role ID is required.";
            return RedirectToAction("Settings");
        }

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            TempData["Error"] = "Role not found.";
            return RedirectToAction("Settings");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            TempData["Success"] = "Role deleted successfully.";
            return RedirectToAction("Settings");
        }

        TempData["Error"] = "Failed to delete role.";
        return RedirectToAction("Settings");
    }

}
