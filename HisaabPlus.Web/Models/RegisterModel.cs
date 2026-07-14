using System.ComponentModel.DataAnnotations;

namespace HisaabPlus.Web.Models
{
    public class RegisterModel
    {
        [Required]
        public string ShopName { get; set; } = "";
        [Required]
        public string OwnerName { get; set; } = "";
        [Required]
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
