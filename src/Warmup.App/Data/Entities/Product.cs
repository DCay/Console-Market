namespace Warmup.App.Data.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
