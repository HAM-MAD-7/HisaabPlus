using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public List<SupplierResponseModel> Suppliers { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if(getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Suppliers = await _apiService.GetAsync<List<SupplierResponseModel>>("api/supplier/GetAllSupplier", getToken);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load suppliers. Try again.";
                return Page();
            }
        }
    }
}
