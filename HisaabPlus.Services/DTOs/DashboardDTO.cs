using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.DTOs
{
    public class DashboardDTO
    {
        public decimal TodayCashSales { get; set; }
        public decimal TodayLoanSales { get; set; }
        public decimal TodayTotalSales { get; set; }
        public decimal TodayEstimatedProfit { get; set; }
        public decimal TotalCustomerBalance { get; set; }
        public decimal TotalSupplierBalance { get; set; }
        public int LowStockCount { get; set; }
        public DateTime TodayDate { get; set; }
    }
}
