//using System.ComponentModel.DataAnnotations;
//using YourNamespace.Models;

//namespace AdvancedBE.Models
//{
//    public class Category
//    {
//        [Key]
//        public int Id { get; set; }
//        public string NameCategory { get; set; }
//        public string DescCategory { get; set; }

//        // Navigation Properties
//        public ICollection<Product> Products { get; set; }
//    }

//}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedBE.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NameCategory { get; set; }

        public string DescCategory { get; set; }

        // Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
