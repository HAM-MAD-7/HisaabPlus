using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.StockPurchase
{
    public class ReceiptModel : PageModel
    {
        private readonly ApiService _apiService;
        public ReceiptModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public StockPurchaseResponseModel StockPurchase { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int purchaseId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                StockPurchase = await _apiService.GetAsync<StockPurchaseResponseModel>($"api/supplier/GetOnePurchase/{purchaseId}", getToken);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load stock purchase receipt. Try again.";
                return Page();
            }
        }
    }
}
