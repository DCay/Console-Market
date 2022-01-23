namespace Warmup.App.Data.Entities
{
    public class ProductDisplay : BaseEntity
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
