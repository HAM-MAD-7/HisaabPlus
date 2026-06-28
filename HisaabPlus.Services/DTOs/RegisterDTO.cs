using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class RegisterDTO
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
