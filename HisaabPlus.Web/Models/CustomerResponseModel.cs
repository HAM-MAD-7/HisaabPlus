namespace HisaabPlus.Web.Models
{
    public class CustomerResponseModel
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal TotalBalance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
