using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Suppliers
{
    public class DetailModel : PageModel
    {
        private readonly ApiService _apiService;
        public DetailModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public SupplierDetailModel SupplierDetail { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int supplierId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                SupplierDetail = await _apiService.GetAsync<SupplierDetailModel>($"api/supplier/GetOneSupplier/{supplierId}", getToken);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load supplier detail. Try again.";
                return Page();
            }
        }
    }
}
