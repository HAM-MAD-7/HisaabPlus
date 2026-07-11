namespace HisaabPlus.Web.Models
{
    public class CreateSaleInputModel
    {
        public int? CustomerId { get; set; }
        public string PaymentType { get; set; } = "";
        public decimal PaidAmount { get; set; }
        public List<SaleItemInputModel> Items { get; set; } = new List<SaleItemInputModel>();
    }
}
