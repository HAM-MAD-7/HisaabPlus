using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class CustomerDetailDTO
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal TotalBalance { get; set; }
        public List<LoanHistoryDTO> LoanHistory { get; set; } = new List<LoanHistoryDTO>();
        public List<PaymentHistoryDTO> PaymentHistory { get; set; } = new List<PaymentHistoryDTO>();
    }
}
