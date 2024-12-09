//using System.ComponentModel.DataAnnotations;
//using YourNamespace.Models;

//namespace AdvancedBE.Models
//{
//    public class Order
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string UserId { get; set; } // Foreign Key to ASP.NET Identity User

//        [Required]
//        public decimal TotalPrice { get; set; }

//        public int? LocationId { get; set; } // Optional location for delivery

//        // Navigation Properties
//        public AspNetUsers User { get; set; }
//        public Location Location { get; set; }
//        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
//        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
//    }


//}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourNamespace.Models;

namespace AdvancedBE.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign Key to AspNetUsers

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        public int? LocationId { get; set; } // Optional Foreign Key to Location

        // Navigation Properties
        public AspNetUsers User { get; set; }
        public Location Location { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
