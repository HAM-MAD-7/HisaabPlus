using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class AddModel : PageModel
    {
        private readonly ApiService _apiService;
        public AddModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public CustomerResponseModel Input { get; set; } = new();
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
                ErrorMessage = "Pls fill all feilds!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Input = await _apiService.PostAsync<CustomerResponseModel>("api/customer/AddCustomer", Input, getToken);
                return RedirectToPage("/Customers/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to add customer. Try again.";
                return Page();
            }
        }
    }
}
