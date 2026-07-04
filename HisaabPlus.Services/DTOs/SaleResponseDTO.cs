using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class SaleResponseDTO
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentType { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public List<SaleItemDetailDTO> Items { get; set; } = new List<SaleItemDetailDTO>();
    }
}
