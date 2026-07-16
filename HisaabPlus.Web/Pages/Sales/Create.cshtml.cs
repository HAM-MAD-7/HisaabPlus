using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Sales
{
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;
        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public CreateSaleInputModel Input { get; set; } = new();
        [BindProperty]
        public string Action { get; set; } = "CreateSale";
        public List<ProductDropdownModel> Products { get; set; } = new();
        public List<CustomerResponseModel> Customers { get; set; } = new();
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
                Products = await _apiService.GetAsync<List<ProductDropdownModel>>("api/product/GetAllProducts", getToken);
                Customers =  await _apiService.GetAsync<List<CustomerResponseModel>>("api/customer/GetCustomer", getToken);
                if(Input.Items.Count == 0)
                {
                    Input.Items.Add(new SaleItemInputModel());
                }
                return Page();
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to load products or customers. Try again.";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                ErrorMessage = "Pls fill all required feilds!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Products = await _apiService.GetAsync<List<ProductDropdownModel>>("api/product/GetAllProducts", getToken);
                Customers = await _apiService.GetAsync<List<CustomerResponseModel>>("api/customer/GetCustomer", getToken);
                if(Action == "AddItem")
                {
                    Input.Items.Add(new SaleItemInputModel());
                    return Page();
                }
                if(Action == "RemoveItem")
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
                if (Input.Items.Count == 0)
                {
                    ErrorMessage = "Pls add atleast one product";
                    return Page();
                }
                if (string.IsNullOrEmpty(Input.PaymentType))
                {
                    ErrorMessage = "Please select a payment type!";
                    return Page();
                }
                if (Input.PaymentType.ToLower() == "loan" && !Input.CustomerId.HasValue)
                {
                    ErrorMessage = "Loan sales require a registered customer. Please select a registered customer.";
                    return Page();
                }
                Input.Items = Input.Items.Where(i => i.ProductId > 0).ToList();
                var response = await _apiService.PostAsync<SaleResponseModel>("api/SalesService/AddSale", Input, getToken);
                return RedirectToPage("/Sales/Receipt", new { saleId = response.SaleId });
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to create sale. Try again.";
                return Page();
            }
        }
    }
}
