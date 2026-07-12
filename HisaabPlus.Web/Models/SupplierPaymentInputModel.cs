namespace HisaabPlus.Web.Models
{
    public class SupplierPaymentInputModel
    {
        public int SupplierId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = "";
    }
}
