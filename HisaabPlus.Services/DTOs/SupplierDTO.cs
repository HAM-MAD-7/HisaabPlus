using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";   
        public decimal TotalBalance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
