using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.StockPurchase
{
    public class RecordPaymentModel : PageModel
    {
        private readonly ApiService _apiService;
        public RecordPaymentModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public SupplierPaymentInputModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet(int supplierId)
        {
            var getToken = HttpContext.Session.GetString("JwtToken");
            if (getToken == null)
            {
                return RedirectToPage("/Auth/Login");
            }
            Input.SupplierId = supplierId;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                if(Input.Amount <= 0)
                {
                    ErrorMessage = "Pls enter a valid amount!";
                    return Page();
                }
                await _apiService.PostAsync<SupplierPaymentInputModel>("api/supplier/StockPayment", Input, getToken);
                return RedirectToPage("/Suppliers/Detail", new { supplierId = Input.SupplierId });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
