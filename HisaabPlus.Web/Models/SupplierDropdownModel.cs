namespace HisaabPlus.Web.Models
{
    public class SupplierDropdownModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public decimal TotalBalance { get; set; } 
    }
}
