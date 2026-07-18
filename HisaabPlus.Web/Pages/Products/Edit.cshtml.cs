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
            catch (Exception)
            {
                ErrorMessage = "Failed to load product. Try again.";
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Empty feilds are unable to update!";
                return Page();
            }
            if (Input.PurchasePrice <= 0)
            {
                ErrorMessage = "Purchase price is not valid!";
                return Page();
            }
            if (Input.SellingPrice <= 0)
            {
                ErrorMessage = "Selling price is not valid!";
                return Page();
            }
            if (Input.LowStockLimit <= 0)
            {
                ErrorMessage = "Low stock limit is not valid!";
                return Page();
            }
            if (Input.CurrentStock < 0)
            {
                ErrorMessage = "Current stock amount is not valid!";
                return Page();
            }
            if (Input.SellingPrice <= Input.PurchasePrice)
            {
                ErrorMessage = "Selling price should be greater than Purchasing price!";
                return Page();
            }
            if (Input.LowStockLimit >= Input.CurrentStock)
            {
                ErrorMessage = "Current stock should be greater than Low stock limit!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                var allProducts = await _apiService.GetAsync<List<ProductResponseModel>>("api/product/GetAllProducts", getToken);
                if (allProducts.Any(p => p.ProductName.ToLower() == Input.ProductName.ToLower() && p.ProductId != Input.ProductId))
                {
                    ErrorMessage = "Product with this name already exists!";
                    return Page();
                }
                await _apiService.PutAsync<bool>($"api/product/UpdateProduct/{Input.ProductId}", Input, getToken);
                return RedirectToPage("/Products/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to update product. Try again.";
                return Page();
            }
        }
    }
}
