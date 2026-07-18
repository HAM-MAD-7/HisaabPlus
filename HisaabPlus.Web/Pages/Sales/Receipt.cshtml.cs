using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Sales
{
    public class ReceiptModel : PageModel
    {
        private readonly ApiService _apiService;
        public ReceiptModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public SaleResponseModel Sale { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int saleId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Sale = await _apiService.GetAsync<SaleResponseModel>($"api/salesservice/getOneSale/{saleId}", getToken);
                return Page();
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to load sale record. Try again.";
                return Page();
            }
        }
    }
}
