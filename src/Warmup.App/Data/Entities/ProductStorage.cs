namespace Warmup.App.Data.Entities
{
    public class ProductStorage : BaseEntity
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
