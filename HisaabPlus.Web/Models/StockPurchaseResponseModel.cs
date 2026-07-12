namespace HisaabPlus.Web.Models
{
    public class StockPurchaseResponseModel
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentType { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public List<StockPurchaseItemDetailModel> Items { get; set; } = new List<StockPurchaseItemDetailModel>();
    }
}
