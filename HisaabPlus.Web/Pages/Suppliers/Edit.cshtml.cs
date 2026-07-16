using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;
        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public SupplierResponseModel Input { get; set; } = new();
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
                Input = await _apiService.GetAsync<SupplierResponseModel>($"api/supplier/GetOneSupplier/{supplierId}", getToken);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load supplier. Try again.";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                ErrorMessage = "Failed to edit empty feild. Try again.";
                return Page();
            }    
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                await _apiService.PutAsync<bool>($"api/supplier/UpdateSupplier/{Input.SupplierId}",Input, getToken);
                return RedirectToPage("/Suppliers/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to edit supplier. Try again.";
                return Page();
            }
        }
    }
}
