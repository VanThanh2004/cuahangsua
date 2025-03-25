using Microsoft.EntityFrameworkCore;
using cuahangsua.Controllers;
using cuahangsua.Models; // Import model Product

namespace cuahangsua.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products
        { get; set; } // Bảng Products
        public DbSet<User> Users { get; set; }
        public DbSet<donhang> donhangs { get; set; } // Tập hợp các bản ghi đơn hàng
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Fix lỗi giá trị Price bị cắt số
        }
    }
}
