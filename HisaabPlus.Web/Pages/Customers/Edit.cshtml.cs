using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;
        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public CustomerResponseModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Input = await _apiService.GetAsync<CustomerResponseModel>($"api/customer/GetOneCustomer/{customerId}", getToken);
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load customer. Try again.";
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                ErrorMessage = "Empty feilds are unable to update!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                await _apiService.PutAsync<bool>($"api/customer/UpdateCustomer/{Input.CustomerId}", Input, getToken);
                return RedirectToPage("/Customers/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to edit customer. Try again.";
                return Page();
            }
        }
    }
}
