namespace HisaabPlus.Web.Models
{
    public class SaleResponseModel
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentType { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public List<SaleItemDetailModel> Items { get; set; } = new List<SaleItemDetailModel>();
    }
}
