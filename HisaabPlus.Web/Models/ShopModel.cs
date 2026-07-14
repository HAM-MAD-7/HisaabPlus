namespace HisaabPlus.Web.Models
{
    public class ShopModel
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public int DaysRemaining { get; set; }
    }
}
