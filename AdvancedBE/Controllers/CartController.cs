using Microsoft.AspNetCore.Mvc;
using AdvancedBE.Models;
using AdvancedBE.Data;
using System.Security.Claims;

public class CartController : Controller
{
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
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
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
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

        // Calculate the total price
        decimal totalPrice = cart.Items.Sum(item => item.TotalPrice);
        var locationId = 1;

        var order = new Order
        {
            UserId = userId,
            TotalPrice = totalPrice,
            LocationId = locationId, // Assign a valid LocationId
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
}
