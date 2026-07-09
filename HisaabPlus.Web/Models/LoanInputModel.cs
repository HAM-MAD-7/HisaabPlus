namespace HisaabPlus.Web.Models
{
    public class LoanInputModel
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = "";
    }
}
