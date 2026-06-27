using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HisaabPlus.Data.Entities
{
    public class StockPurchaseItem
    {
        [Key]
        public int PurchaseItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int PurchaseId { get; set; }
        [ForeignKey(nameof(PurchaseId))]
        public StockPurchase StockPurchase { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
