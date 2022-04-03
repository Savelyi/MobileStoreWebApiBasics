using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MobileStore.Models
{
    public class MobileStoreDbContext : DbContext
    {
        public MobileStoreDbContext()
        {

        }
        public MobileStoreDbContext(DbContextOptions<MobileStoreDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            if (!Roles.Any())
            {
                Roles.AddRangeAsync(new Role() { Id = 0, Name = "Admin" },
                    new Role() { Id = 1, Name = "User" });
            }
            if (Products.Any())
            {
                Products.AddRangeAsync(new Product() { Id = 0, Name = "Iphone" },
                    new Product() { Id = 1, Name = "Samsung" },
                    new Product() { Id = 2, Name = "Pixel" });
            }
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                .IsRequired();

                entity.Property(e => e.ProductId)
                .IsRequired();

                entity.HasOne(e => e.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.PriceUSD).IsUnique();

                entity.Property(e => e.PriceUSD)
                .IsRequired();
                entity.Property(e => e.Name)
                .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Password).IsUnique();

                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.RoleId).IsRequired();


            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
