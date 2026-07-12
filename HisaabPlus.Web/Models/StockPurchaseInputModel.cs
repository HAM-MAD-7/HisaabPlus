namespace HisaabPlus.Web.Models
{
    public class StockPurchaseInputModel
    {
        public int SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentType { get; set; } = "";
        public string Notes { get; set; } = "";
        public List<StockPurchaseItemInputModel> Items { get; set; } = new List<StockPurchaseItemInputModel>();
    }
}
