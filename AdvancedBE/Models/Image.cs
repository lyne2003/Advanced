////using System.ComponentModel.DataAnnotations;

////namespace AdvancedBE.Models
////{
////    public class Image
////    {
////        [Key]
////        public int Id { get; set; }
////        //public int IdProduct { get; set; }
////        public string UrlImage { get; set; }

////        // Navigation Properties
////        public Product Product { get; set; }
////    }

////}

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace AdvancedBE.Models
//{
//    public class Image
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string UrlImage { get; set; }

//        // Foreign Key
//        public int ProductId { get; set; }

//        // Navigation Property
//        public Product Product { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;

namespace AdvancedBE.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UrlImage { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        // Navigation Property
        public Product Product { get; set; }
    }
}

