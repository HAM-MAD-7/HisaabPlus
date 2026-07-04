using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class CreateSaleDTO
    {
        public int? CustomerId { get; set; }
        public string PaymentType { get; set; } = "";
        public decimal PaidAmount { get; set; }
        public List<SaleItemInputDTO> Items { get; set; } = new List<SaleItemInputDTO>();
    }
}
