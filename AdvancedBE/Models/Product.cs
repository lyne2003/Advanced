//////namespace AdvancedBE.Models
//////{
//////    public class Product
//////    {
//////        public int IdProduct { get; set; }
//////        public string NameProduct { get; set; }
//////        public string DescProduct { get; set; }
//////        public decimal PriceProduct { get; set; }

//////        // Navigation Properties
//////        public ICollection<Image> Images { get; set; }
//////        public ICollection<OrderDetail> OrderDetails { get; set; }
//////    }

//////}
////using System.ComponentModel.DataAnnotations;

////namespace AdvancedBE.Models
////{
////    public class Product
////    {
////        [Key]
////        public int Id { get; set; }

////        [Required]
////        public string Name { get; set; }

////        public string Description { get; set; }

////        [Required]
////        [Range(0.01, double.MaxValue)]
////        public decimal Price { get; set; }

////        public int Stock { get; set; }
////    }
////}
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace AdvancedBE.Models
//{
//    public class Product
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string Name { get; set; }

//        public string Description { get; set; }

//        [Required]
//        [Range(0.01, double.MaxValue)]
//        public decimal Price { get; set; }

//        public int Stock { get; set; }

//        // Navigation Property for multiple images
//        public ICollection<Image> Images { get; set; } = new List<Image>();
//    }
//}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedBE.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
