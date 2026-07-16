using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public List<CustomerResponseModel> Customers { get; set; } = new();
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
                Customers = await _apiService.GetAsync<List<CustomerResponseModel>>("api/customer/GetCustomer", getToken);
            }
            catch(Exception ex)
            {
                ErrorMessage = "Failed to load customers. Try again";
                return Page();
            }
            return Page();
        }
    }
}
