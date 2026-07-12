namespace HisaabPlus.Web.Models
{
    public class StockPurchaseItemInputModel
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
