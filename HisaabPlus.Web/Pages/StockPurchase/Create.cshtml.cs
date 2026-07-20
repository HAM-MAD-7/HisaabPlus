using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.StockPurchase
{
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;
        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public StockPurchaseInputModel Input { get; set; } = new();
        public List<SupplierResponseModel> Suppliers { get; set; } = new();
        public List<ProductDropdownModel> Products { get; set; } = new();
        [BindProperty]
        public string Action { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Suppliers = await _apiService.GetAsync<List<SupplierResponseModel>>("api/supplier/GetAllSupplier", getToken);
                var allProducts = await _apiService.GetAsync<List<ProductDropdownModel>>("api/product/GetAllProducts", getToken);
                Products = allProducts.Where(p => p.IsActive == true).ToList();
                if (Input.Items.Count == 0)
                {
                    Input.Items.Add(new StockPurchaseItemInputModel());
                }
                return Page();
            }
            catch (Exception )
            {
                ErrorMessage = "Failed to load suppliers and products. Try again.";
                return Page();
            }
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
                Suppliers = await _apiService.GetAsync<List<SupplierResponseModel>>("api/supplier/GetAllSupplier", getToken);
                var allProducts = await _apiService.GetAsync<List<ProductDropdownModel>>("api/product/GetAllProducts", getToken);
                Products = allProducts.Where(p => p.IsActive == true).ToList();
                if (Action == "AddStock")
                {
                    Input.Items.Add(new StockPurchaseItemInputModel());
                    return Page();
                }
                if(Action == "RemoveStock")
                {
                    if (int.TryParse(Request.Form["RemoveIndex"], out int index))
                    {
                        if(index >= 0 && index < Input.Items.Count)
                        {
                            Input.Items.RemoveAt(index);
                        }
                    }
                    return Page();
                }
                Input.Items = Input.Items.Where(p => p.ProductId > 0).ToList();
                if (Input.SupplierId == 0)
                {
                    ErrorMessage = "Pls select a supplier";
                    return Page();
                }
                if(Input.Items.Count == 0)
                {
                    ErrorMessage = "Pls add atleast one product";
                    return Page();
                }
                if (string.IsNullOrEmpty(Input.PaymentType))
                {
                    ErrorMessage = "Please select a payment type!";
                    return Page();
                }
                await _apiService.PostAsync<bool>("api/supplier/StockPurchase", Input ,getToken);
                return RedirectToPage("/StockPurchase/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to create stock purchase. Try again.";
                return Page();
            }
        }
    }
}
