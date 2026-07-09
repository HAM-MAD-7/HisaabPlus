namespace HisaabPlus.Web.Models
{
    public class LoanHistoryModel
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentType { get; set; } = "";
    }
}
