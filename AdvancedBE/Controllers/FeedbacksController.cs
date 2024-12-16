using Microsoft.AspNetCore.Mvc;
using AdvancedBE.Data;
using AdvancedBE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

public class FeedbacksController : Controller
{
    private readonly ApplicationDbContext _context;

    public FeedbacksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Feedbacks/Create
    [Authorize(Roles = "client")]
    public IActionResult Create(int orderId)
    {
        // Check if the order exists
        var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        // Pass the orderId to the view for creating feedback
        ViewBag.OrderId = orderId;
        return View();
    }

    // POST: Feedbacks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "client")]
    public IActionResult Create(int orderId, Feedback feedback)
    {
            // Set the logged-in user's ID
            feedback.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            feedback.OrderId = orderId;

            // Save feedback to the database
            _context.Feedback.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("IndexClient", "Orders"); // Redirect to Orders page

    }
}
