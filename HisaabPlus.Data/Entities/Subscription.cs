using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HisaabPlus.Data.Entities
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string PlanType { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public string PaymentMethod { get; set; } = "";
        public string TransactionRef { get; set; } = "";
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
        public int ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public Shop Shop { get; set; }
    }
}
