namespace AdvancedBE.Models
{
    public class AdminModel
    {
        public string Title { get; set; } = "Admin Dashboard";
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
    }
}
