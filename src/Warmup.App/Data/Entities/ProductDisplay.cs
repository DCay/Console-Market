namespace Warmup.App.Data.Entities
{
    public class ProductDisplay : BaseEntity
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
