using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public string Unit { get; set; } = "";
        public decimal LowStockLimit { get; set; }
        public bool IsActive { get; set; }
    }
}
