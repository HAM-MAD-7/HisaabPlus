using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Products
{
    public class AddModel : PageModel
    {
        private readonly ApiService _apiService;
        public AddModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public ProductResponseModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet()
        {
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
                Input = await _apiService.PostAsync<ProductResponseModel>("api/product/AddProduct", Input, getToken);
                return RedirectToPage("/Products/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
