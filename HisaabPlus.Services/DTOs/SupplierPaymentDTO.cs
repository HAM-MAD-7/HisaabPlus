using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class SupplierPaymentDTO
    {
        public int SupplierId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = "";
    }
}
