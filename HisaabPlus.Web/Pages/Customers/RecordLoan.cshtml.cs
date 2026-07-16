using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Customers
{
    public class RecordLoanModel : PageModel
    {
        private readonly ApiService _apiService;
        public RecordLoanModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public LoanInputModel Input { get; set; } = new();
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
                await _apiService.PostAsync<LoanInputModel>("api/customer/AddLoanRecord", Input, getToken);
                return RedirectToPage("/Customers/Detail", new { customerId = Input.CustomerId});
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to record loan. Try Again.";
                return Page();
            }
        }
    }
}
