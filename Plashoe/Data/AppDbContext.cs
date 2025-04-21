using Microsoft.EntityFrameworkCore;
using Plashoe.Model;

namespace Plashoe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Address> Address { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<User>()
                .Property(x => x.IsBlocked)
                .HasDefaultValue(false);
            modelBuilder.Entity<User>()
              .HasOne(x => x.Cart)
              .WithOne(y => y.User)
              .HasForeignKey<Cart>(x => x.UserId);


            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Men" },
                new Category { CategoryId = 2, Name = "Women" }
                );
            modelBuilder.Entity<Category>()
                .HasMany(x => x.products)
                .WithOne(r => r.Category)
                .HasForeignKey(x => x.CategoryId);


            modelBuilder.Entity<Product>()
                .Property(pr => pr.Price)
                .HasColumnType("decimal(18,2)");
        
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.products)
                .HasForeignKey(p => p.CategoryId);


            modelBuilder.Entity<Cart>()
                .HasMany(q=>q.Cartitems)
                .WithOne(r => r.Cart)
                .HasForeignKey(p => p.CartId);


            modelBuilder.Entity<CartItems>()
                .HasOne(a => a.Product)
                .WithMany(o => o.CartItems)
                .HasForeignKey(i => i.ProductId);


            modelBuilder.Entity<Wishlist>()
                .HasOne(x => x.Users)
                .WithMany(w => w.Wishlist)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Wishlist>()
                .HasOne(x => x.Products)
                .WithMany()
                .HasForeignKey(e => e.ProductId);


         


            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Order>()
                .Property(x => x.Status)
                .HasDefaultValue("placed");
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(u => u.AddressId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(d => d.OrderId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItems>()
    .HasKey(ci => ci.Id);


        }
    }
}
