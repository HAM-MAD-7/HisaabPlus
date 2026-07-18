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
            var getToken = HttpContext.Session.GetString("JwtToken");
            if (getToken == null)
            {
                return RedirectToPage("/Auth/Login");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Pls fill all required feilds!";
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
            if(Input.CurrentStock < 0)
            {
                ErrorMessage = "Current stock amount is not valid!";
                return Page();
            }
            if(Input.SellingPrice <= Input.PurchasePrice)
            {
                ErrorMessage = "Selling price should be greater than Purchasing price!";
                return Page();
            }
            if(Input.LowStockLimit >= Input.CurrentStock)
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
                if(allProducts.Any(p => p.ProductName.ToLower() == Input.ProductName.ToLower()))
                {
                    ErrorMessage = "Product with this name already exists!";
                    return Page();
                }
                Input = await _apiService.PostAsync<ProductResponseModel>("api/product/AddProduct", Input, getToken);
                return RedirectToPage("/Products/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to add product. Try again.";
                return Page();
            }
        }
    }
}
