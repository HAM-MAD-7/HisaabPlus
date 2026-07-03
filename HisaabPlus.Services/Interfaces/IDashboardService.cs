using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardAsync(int shopId);
        Task<MonthlyReportDTO> GetMonthlyReportAsync(int shopId, int month, int year);
    }
}
