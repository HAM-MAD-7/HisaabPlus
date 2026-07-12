using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Dashboard
{
    public class ReportModel : PageModel
    {
        private readonly ApiService _apiService;
        public ReportModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public MonthlyReportModel Data { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int month, int year)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Data = await _apiService.GetAsync<MonthlyReportModel>($"api/dashboard/GetMonthlyInfo/{month}/{year}", getToken);
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
