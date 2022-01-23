using Warmup.App.Data.Entities;

namespace Warmup.App.Data
{
    public class WarmupDbContext
    {
        public List<Product> Products { get; set; }
        
        public List<ProductDisplay> ProductDisplays { get; set; }

        public List<ProductStorage> ProductStorages { get; set; }

        public List<User> Users { get; set; }

        public List<UserRole> Roles { get; set; }

        public WarmupDbContext()
        {
            this.Products = new List<Product>();
            this.ProductDisplays = new List<ProductDisplay>();  
            this.ProductStorages = new List<ProductStorage>();
            this.Users = new List<User>();
            this.Roles = new List<UserRole>();
        }
    }
}
