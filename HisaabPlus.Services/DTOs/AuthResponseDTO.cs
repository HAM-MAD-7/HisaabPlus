using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = "";
        public int ShopId { get; set; }
        public string ShopName { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
