namespace HisaabPlus.Web.Models
{
    public class SupplierPaymentHistoryModel
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Notes { get; set; } = "";
    }
}
