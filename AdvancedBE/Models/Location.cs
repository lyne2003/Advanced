//using System.ComponentModel.DataAnnotations;
//using YourNamespace.Models;

//namespace AdvancedBE.Models
//{
//    public class Location
//    {
//        [Key]
//        public int Id { get; set; }
//        //public string IdUser { get; set; } // Foreign Key to ASP.NET Identity User
//        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
//        public string City { get; set; }
//        public string Region { get; set; }
//        public string ZipCode { get; set; }
//        public string Country { get; set; }
//        public double? Latitude { get; set; }
//        public double? Longitude { get; set; }
//        public string Landmark { get; set; }

//        // Navigation Properties
//        public AspNetUsers User { get; set; }
//        public ICollection<Order> Orders { get; set; }
//    }

//}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourNamespace.Models;

namespace AdvancedBE.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        public string Region { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string Landmark { get; set; }

        // Navigation Properties
        public string UserId { get; set; } // Foreign Key
        public AspNetUsers User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
