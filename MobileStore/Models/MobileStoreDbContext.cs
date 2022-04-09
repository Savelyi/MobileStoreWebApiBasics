using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MobileStore.Models
{
    public class MobileStoreDbContext : IdentityDbContext<User>
    {
        public MobileStoreDbContext()
        {

        }
        public MobileStoreDbContext(DbContextOptions<MobileStoreDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.Property(e=>e.UserName).IsRequired();
                e.Property(e=>e.PasswordHash).IsRequired();
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

                entity.Property(e => e.PriceUSD)
                .IsRequired();
                entity.Property(e => e.Name)
                .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
