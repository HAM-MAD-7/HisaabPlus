namespace HisaabPlus.Web.Models
{
    public class PaymentHistoryModel
    {
        public int PaymentId { get; set; }
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Notes { get; set; } = "";
    }
}
