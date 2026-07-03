using HisaabPlus.Data.Context;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HisaabPlus.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;
        public DashboardService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<DashboardDTO> GetDashboardAsync(int shopId)
        {
            var todayStart = DateTime.UtcNow.Date;
            var todayEnd = todayStart.AddDays(1);
            var cashSales = await _db.Sales.Where(s => s.ShopId == shopId && s.PaymentType == "Cash" && s.SaleDate >= todayStart && s.SaleDate < todayEnd).SumAsync(p => p.TotalAmount);
            var loanSales = await _db.Sales.Where(s => s.ShopId == shopId && s.PaymentType == "Loan" && s.SaleDate >= todayStart && s.SaleDate < todayEnd).SumAsync(p => p.TotalAmount);
            var totalSales = cashSales + loanSales;
            var getSalesItems = await _db.SaleItems.Where(s => s.Sale.ShopId == shopId && s.Sale.SaleDate >= todayStart && s.Sale.SaleDate < todayEnd).SumAsync(p => (p.Product.SellingPrice - p.Product.PurchasePrice) * p.Quantity);
            var totalCustomerBalance = await _db.Customers.Where(s => s.ShopId == shopId).SumAsync(p => p.TotalBalance);
            var totalSupplierBalance = await _db.Suppliers.Where(s => s.ShopId == shopId).SumAsync(p => p.TotalBalance);
            var countProducts = await _db.Products.Where(p => p.ShopId == shopId && p.CurrentStock <= p.LowStockLimit && p.IsActive == true).CountAsync();
            var mapDashboard = new DashboardDTO
            {
                TodayCashSales = cashSales,
                TodayLoanSales = loanSales,
                TodayTotalSales = totalSales,
                TodayEstimatedProfit = getSalesItems,
                TotalCustomerBalance = totalCustomerBalance,
                TotalSupplierBalance = totalSupplierBalance,
                LowStockCount = countProducts,
                TodayDate = todayStart
            };
            return mapDashboard;
        }
        public async Task<MonthlyReportDTO> GetMonthlyReportAsync(int shopId, int month, int year)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1);
            var totalSales = await _db.Sales.Where(s => s.ShopId == shopId && s.SaleDate >= monthStart && s.SaleDate < monthEnd).SumAsync(p => p.TotalAmount);
            var cashSales = await _db.Sales.Where(s => s.ShopId == shopId && s.PaymentType == "Cash" && s.SaleDate >= monthStart && s.SaleDate < monthEnd).SumAsync(p => p.TotalAmount);
            var loanSales = await _db.Sales.Where(s => s.ShopId == shopId && s.PaymentType == "Loan" && s.SaleDate >= monthStart && s.SaleDate < monthEnd).SumAsync(p => p.TotalAmount);
            var monthlyEstimatedProfit = await _db.SaleItems.Where(s => s.Sale.ShopId == shopId && s.Sale.SaleDate >= monthStart && s.Sale.SaleDate < monthEnd).SumAsync(p => (p.Product.SellingPrice - p.Product.PurchasePrice) * p.Quantity);
            var totalCustomerPayments = await _db.CustomerPayments.Where(s => s.ShopId == shopId && s.PaymentDate >= monthStart && s.PaymentDate < monthEnd).SumAsync(p => p.Amount);
            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var mapMonthlyReport = new MonthlyReportDTO
            {
                Month = monthName,
                Year = year,
                TotalSales = totalSales,
                TotalCashSales = cashSales,
                TotalLoanSales = loanSales,
                EstimatedProfit = monthlyEstimatedProfit,
                TotalCustomerPayments = totalCustomerPayments
            };
            return mapMonthlyReport;
        }
    }
}
