using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class PayLoanDTO
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = "";
    }
}
