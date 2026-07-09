namespace HisaabPlus.Web.Models
{
    public class CustomerDetailResponseModel
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal TotalBalance { get; set; }
        public List<LoanHistoryModel> LoanHistory { get; set; } = new List<LoanHistoryModel>();
        public List<PaymentHistoryModel> PaymentHistory { get; set; } = new List<PaymentHistoryModel>();
    }
}
