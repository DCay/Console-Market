namespace Warmup.App.Data.Entities
{
    public class CashDeck : BaseEntity
    {
        public int Index { get; set; }

        public User Cashier { get; set; }

        public decimal TotalMoney { get; set; }
    }
}
