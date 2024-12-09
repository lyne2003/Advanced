//using System.ComponentModel.DataAnnotations;

//namespace AdvancedBE.Models
//{
//    public class OrderDetail
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public int OrderId { get; set; } // Foreign Key to Order

//        [Required]
//        public int ProductId { get; set; } // Foreign Key to Product

//        [Required]
//        public int Quantity { get; set; }

//        [Required]
//        public decimal Price { get; set; }

//        // Navigation Properties
//        public Order Order { get; set; }
//        public Product Product { get; set; }
//    }


//}
using System.ComponentModel.DataAnnotations;

namespace AdvancedBE.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } // Foreign Key

        [Required]
        public int ProductId { get; set; } // Foreign Key

        [Required]
        public int Quantity { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
