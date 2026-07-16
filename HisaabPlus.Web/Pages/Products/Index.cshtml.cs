using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public List<ProductResponseModel> Products { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Products = await _apiService.GetAsync<List<ProductResponseModel>>("api/product/GetAllProducts",getToken);
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load products. Try again.";
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int productId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                await _apiService.DeleteAsync($"api/product/DeleteProduct/{productId}", getToken);
                return RedirectToPage("/Products/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to delete product. Try again.";
                return Page();
            }
        }
    }
}
