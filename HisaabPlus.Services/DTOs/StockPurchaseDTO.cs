using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class StockPurchaseDTO
    {
        public int SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentType { get; set; } = "";
        public string Notes { get; set; } = "";
        public List<StockPurchaseItemDTO> Items { get; set; } = new List<StockPurchaseItemDTO>();
    }
}
