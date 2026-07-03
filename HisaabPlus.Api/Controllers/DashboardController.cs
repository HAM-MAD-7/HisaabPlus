using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HisaabPlus.Services.Interfaces;

namespace HisaabPlus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("GetRegularInfo")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var result = await _dashboardService.GetDashboardAsync(GetShopId());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetMonthlyInfo/{month}/{year}")]
        public async Task<IActionResult> GetMonthlyReport(int month, int year)
        {
            try
            {
                var result = await _dashboardService.GetMonthlyReportAsync(GetShopId(), month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private int GetShopId()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "ShopId")?.Value;
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("ShopId not found in token!");
            }
            return int.Parse(id);
        }
    }
}
