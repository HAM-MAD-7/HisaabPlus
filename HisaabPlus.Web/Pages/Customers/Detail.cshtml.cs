using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class DetailModel : PageModel
    {
        private readonly ApiService _apiService;
        public DetailModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public CustomerDetailResponseModel Customer { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync([FromQuery] int customerId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Auth/Login");
                }
                Customer = await _apiService.GetAsync<CustomerDetailResponseModel>($"api/customer/GetOneCustomer/{customerId}", getToken);
                return Page();
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to load detail of customer. Try again.";
                return Page();
            }
        }
    }
}
