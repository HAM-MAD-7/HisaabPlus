using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class RecordPaymentModel : PageModel
    {
        private readonly ApiService _apiService;
        public RecordPaymentModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public PayLoanInputModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet(int customerId)
        {
            var getToken = HttpContext.Session.GetString("JwtToken");
            if (getToken == null)
            {
                return RedirectToPage("/Auth/Login");
            }
            Input.CustomerId = customerId;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
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
                await _apiService.PostAsync<bool>("api/customer/AddPayLoanRecord", Input, getToken);
                return RedirectToPage("/Customers/Detail", new { customerId = Input.CustomerId });
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to record payment. Try Again.";
                return Page();
            }
        }
    }
}
