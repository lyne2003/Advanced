//using System.ComponentModel.DataAnnotations;
//using YourNamespace.Models;

//namespace AdvancedBE.Models
//{
//    public class Feedback
//    {
//        [Key]
//        public int Id { get; set; }
//        //public string IdUser { get; set; } // Foreign Key to ASP.NET Identity User
//        //public int IdOrder { get; set; }
//        public int Rate { get; set; }
//        public string Description { get; set; }

//        // Navigation Properties
//        public AspNetUsers User { get; set; }
//        public Order Order { get; set; }
//    }

//}

using System.ComponentModel.DataAnnotations;
using YourNamespace.Models;

namespace AdvancedBE.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }

        public string Description { get; set; }

        // Navigation Properties
        public string UserId { get; set; } // Foreign Key
        public AspNetUsers User { get; set; }
        public int OrderId { get; set; } // Foreign Key
        public Order Order { get; set; }
    }
}

