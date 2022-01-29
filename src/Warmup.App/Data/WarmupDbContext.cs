using Microsoft.EntityFrameworkCore;
using Warmup.App.Data.Entities;

namespace Warmup.App.Data
{
    public class WarmupDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        
        public virtual DbSet<ProductDisplay> ProductDisplays { get; set; }

        public virtual DbSet<ProductStorage> ProductStorages { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRole> Roles { get; set; }

        public virtual DbSet<CashDeck> CashDecks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-39PBTTP\SQLEXPRESS;Database=warmup;Trusted_Connection=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
