namespace Warmup.App.Data.Entities
{
    public class CashDeck : BaseEntity
    {
        public int CashDeckIndex { get; set; }

        public string? CashierId { get; set; }

        public User? Cashier { get; set; }

        public decimal TotalMoney { get; set; }
    }
}
