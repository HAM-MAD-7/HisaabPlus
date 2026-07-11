namespace HisaabPlus.Web.Models
{
    public class SupplierDetailModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public decimal TotalBalance { get; set; }
        public List<PurchaseHistoryModel> PurchaseHistory { get; set; } = new List<PurchaseHistoryModel>();
        public List<SupplierPaymentHistoryModel> PaymentHistory { get; set; } = new List<SupplierPaymentHistoryModel>();
    }
}
