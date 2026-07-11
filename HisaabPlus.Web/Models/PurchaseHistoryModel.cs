namespace HisaabPlus.Web.Models
{
    public class PurchaseHistoryModel
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentType { get; set; } = "";
    }
}
