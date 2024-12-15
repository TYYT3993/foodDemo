using FoodOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Data
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }

        // DbSet 屬性建議放在上方
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定主鍵
            modelBuilder.Entity<MenuItem>().HasKey(m => m.ItemID);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderID);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemID);

            // Order 關聯設定
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()  // 如果 Customer 類別要追蹤訂單，應改為 .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);  // 防止刪除顧客時連帶刪除訂單

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);  // 刪除訂單時連帶刪除訂單項目

            // OrderItem 關聯設定
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemID)
                .OnDelete(DeleteBehavior.Restrict);  // 防止刪除菜單項目時連帶刪除訂單項目

            // 設定必填欄位
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Name)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .IsRequired();

            // 設定金額欄位精度
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.SubTotal)
                .HasPrecision(10, 2);

        }
    }
}
