//using Microsoft.AspNetCore.Mvc;
//using AdvancedBE.Models;
//using AdvancedBE.Data;
//using System.Security.Claims;

//public class CartController : Controller
//{
//    public IActionResult Index()
//    {
//        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
//        return View(cart);
//    }

//    [HttpPost]
//    public IActionResult AddToCart(int productId, string name, decimal price, int quantity = 1)
//    {
//        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
//        cart.AddItem(productId, name, price, quantity);
//        HttpContext.Session.SetObjectAsJson("Cart", cart);

//        return RedirectToAction("Index");
//    }

//    [HttpPost]
//    public IActionResult RemoveFromCart(int productId)
//    {
//        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
//        if (cart != null)
//        {
//            cart.RemoveItem(productId);
//            HttpContext.Session.SetObjectAsJson("Cart", cart);
//        }

//        return RedirectToAction("Index");
//    }

//    public IActionResult ClearCart()
//    {
//        HttpContext.Session.Remove("Cart");
//        return RedirectToAction("Index");
//    }
//    private readonly ApplicationDbContext _context;

//    public CartController(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    [HttpPost]
//    public async Task<IActionResult> Checkout()
//    {
//        // Retrieve the cart from session
//        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");

//        if (cart == null || !cart.Items.Any())
//        {
//            TempData["Error"] = "Your cart is empty!";
//            return RedirectToAction("Index");
//        }

//        // Retrieve the logged-in user's ID
//        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//        if (string.IsNullOrEmpty(userId))
//        {
//            TempData["Error"] = "You must be logged in to checkout.";
//            return RedirectToAction("Index", "Account");
//        }

//        // Calculate the total price
//        decimal totalPrice = cart.Items.Sum(item => item.TotalPrice);
//        var locationId = 1;

//        var order = new Order
//        {
//            UserId = userId,
//            TotalPrice = totalPrice,
//            LocationId = locationId, // Assign a valid LocationId
//            OrderDetails = cart.Items.Select(item => new OrderDetail
//            {
//                ProductId = item.ProductId,
//                Quantity = item.Quantity
//            }).ToList()
//        };

//        // Add the order to the database
//        _context.Add(order);
//        await _context.SaveChangesAsync();

//        // Clear the cart after checkout
//        HttpContext.Session.Remove("Cart");

//        TempData["Success"] = "Order placed successfully!";
//        return RedirectToAction("Index", "Orders");
//    }
//}
using Microsoft.AspNetCore.Mvc;
using AdvancedBE.Models;
using AdvancedBE.Data;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Display the cart and retrieve user's locations
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

        // Get the logged-in user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve all locations for the logged-in user
        var userLocations = _context.Location
            .Where(l => l.UserId == userId)
            .ToList();

        // Pass locations to the ViewBag
        ViewBag.Location = userLocations;

        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, string name, decimal price, int quantity = 1)
    {
        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
        cart.AddItem(productId, name, price, quantity);
        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
        if (cart != null)
        {
            cart.RemoveItem(productId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CompleteCheckout(int LocationId)
    {
        // Retrieve the cart from session
        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");

        if (cart == null || !cart.Items.Any())
        {
            TempData["Error"] = "Your cart is empty!";
            return RedirectToAction("Index");
        }

        // Retrieve the logged-in user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            TempData["Error"] = "You must be logged in to checkout.";
            return RedirectToAction("Index", "Account");
        }

        // Validate the selected location belongs to the user
        var selectedLocation = await _context.Location
            .FirstOrDefaultAsync(l => l.Id == LocationId && l.UserId == userId);

        if (selectedLocation == null)
        {
            TempData["Error"] = "Invalid location selected.";
            return RedirectToAction("Index");
        }

        // Calculate the total price
        decimal totalPrice = cart.Items.Sum(item => item.TotalPrice);

        // Create the order
        var order = new Order
        {
            UserId = userId,
            TotalPrice = totalPrice,
            LocationId = LocationId, // Use the selected location
            OrderDetails = cart.Items.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList()
        };

        // Add the order to the database
        _context.Add(order);
        await _context.SaveChangesAsync();

        // Clear the cart after checkout
        HttpContext.Session.Remove("Cart");

        TempData["Success"] = "Order placed successfully!";
        return RedirectToAction("Index", "Orders");
    }

    [HttpGet]
    public IActionResult AddLocation()
    {
        // Redirect to the Locations/Create page
        return RedirectToAction("Create", "Locations");
    }
}
