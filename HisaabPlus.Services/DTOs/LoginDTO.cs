using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string PhoneNumber { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
