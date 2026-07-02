using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class SupplierDetailDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";   
        public decimal TotalBalance {  get; set; }
        public List<PurchaseHistoryDTO> PurchaseHistory { get; set; } = new List<PurchaseHistoryDTO>();
        public List<SupplierPaymentHistoryDTO> PaymentHistory { get; set; } = new List<SupplierPaymentHistoryDTO>();
    }
}
