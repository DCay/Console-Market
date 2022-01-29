namespace Warmup.App.Data.Entities
{
    public class ProductStorage : BaseEntity
    {
        public string ProductId { get; set; }


        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
