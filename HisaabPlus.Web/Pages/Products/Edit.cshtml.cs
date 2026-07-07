using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;
        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public ProductResponseModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int productId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Input = await _apiService.GetAsync<ProductResponseModel>($"api/product/GetOneProduct/{productId}", getToken);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
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
                await _apiService.PutAsync<bool>($"api/product/UpdateProduct/{Input.ProductId}", Input, getToken);
                return RedirectToPage("/Products/Index");
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
