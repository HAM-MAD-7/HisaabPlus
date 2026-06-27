using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HisaabPlus.Data.Entities
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }
        [Required]
        public string ShopName { get; set; } = "";
        [Required]
        public string OwnerName { get; set; } = "";
        [Required]
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        [Required]
        public string PasswordHash { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
