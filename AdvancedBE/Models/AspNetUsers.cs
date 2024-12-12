using AdvancedBE.Models;
using Microsoft.AspNetCore.Identity;

namespace YourNamespace.Models
{
    // Extending IdentityUser to represent AspNetUsers table
    public class AspNetUsers : IdentityUser
    {
        // You can add custom properties here if needed
        // For example:
        // public string FullName { get; set; }

        // Add navigation property for Locations
        public ICollection<Location> Locations { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
