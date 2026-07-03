using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class MonthlyReportDTO
    {
        public string Month { get; set; } = "";
        public int Year { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalCashSales { get; set; }
        public decimal TotalLoanSales { get; set; }
        public decimal EstimatedProfit { get; set; }
        public decimal TotalCustomerPayments { get; set; }
    }
}
