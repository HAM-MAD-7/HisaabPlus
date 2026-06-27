using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HisaabPlus.Data.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal TotalBalance { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public Shop Shop { get; set; }
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();
    }
}
