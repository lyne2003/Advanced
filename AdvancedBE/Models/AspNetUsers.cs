using Microsoft.AspNetCore.Identity;

namespace YourNamespace.Models
{
    // Extending IdentityUser to represent AspNetUsers table
    public class AspNetUsers : IdentityUser
    {
        // You can add custom properties here if needed
        // For example:
        // public string FullName { get; set; }
    }
}
