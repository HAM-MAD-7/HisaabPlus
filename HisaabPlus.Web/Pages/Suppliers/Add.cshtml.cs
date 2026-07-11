using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Suppliers
{
    public class AddModel : PageModel
    {
        private readonly ApiService _apiService;
        public AddModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public SupplierResponseModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet()
        {
            var getToken = HttpContext.Session.GetString("JwtToken");
            if (getToken == null)
            {
                return RedirectToPage("/Auth/Login");
            }
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
                await _apiService.PostAsync<SupplierResponseModel>("api/supplier/AddSupplier", Input, getToken);
                return RedirectToPage("/Suppliers/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
