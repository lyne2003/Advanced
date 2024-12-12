//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using AdvancedBE.Models;
//using System.Reflection.Emit;

//namespace AdvancedBE.Data
//{
//    public class ApplicationDbContext : IdentityDbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options)
//        {
//        }
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            // Configure one-to-many relationship
//            builder.Entity<Image>()
//                .HasOne(i => i.Product)
//                .WithMany(p => p.Images)
//                .HasForeignKey(i => i.ProductId);

//            // Correct column type for IdentityRole
//            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
//            {
//                entity.Property(e => e.Name).HasMaxLength(256);
//                entity.Property(e => e.NormalizedName).HasMaxLength(256);
//            });

//            // Correct column type for IdentityUser
//            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>(entity =>
//            {
//                entity.Property(e => e.UserName).HasMaxLength(256);
//                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
//                entity.Property(e => e.Email).HasMaxLength(256);
//                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

//                // Change ConcurrencyStamp to TEXT
//                entity.Property(e => e.ConcurrencyStamp).HasColumnType("TEXT");
//            });
//        }
//        public DbSet<AdvancedBE.Models.Category> Category { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.Feedback> Feedback { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.Image> Image { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.Location> Location { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.Order> Order { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.OrderDetail> OrderDetail { get; set; } = default!;
//        public DbSet<AdvancedBE.Models.Product> Product { get; set; } = default!;

//    }
//}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdvancedBE.Models;
using System.Reflection.Emit;

namespace AdvancedBE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure one-to-many relationship between AspNetUsers and Locations
            builder.Entity<Location>()
                .HasOne(l => l.User) // Each Location has one User
                .WithMany(u => u.Locations) // Each User can have many Locations
                .HasForeignKey(l => l.UserId) // Foreign key is UserId
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure Image and Product relationship
            builder.Entity<Image>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId);

            // Correct column type for IdentityRole
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            // Correct column type for IdentityUser
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                // Change ConcurrencyStamp to TEXT
                entity.Property(e => e.ConcurrencyStamp).HasColumnType("TEXT");
            });
        }

        public DbSet<AdvancedBE.Models.Category> Category { get; set; } = default!;
        public DbSet<AdvancedBE.Models.Feedback> Feedback { get; set; } = default!;
        public DbSet<AdvancedBE.Models.Image> Image { get; set; } = default!;
        public DbSet<AdvancedBE.Models.Location> Location { get; set; } = default!;
        public DbSet<AdvancedBE.Models.Order> Order { get; set; } = default!;
        public DbSet<AdvancedBE.Models.OrderDetail> OrderDetail { get; set; } = default!;
        public DbSet<AdvancedBE.Models.Product> Product { get; set; } = default!;
    }
}
