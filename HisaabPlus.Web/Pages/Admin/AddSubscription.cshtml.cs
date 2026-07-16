using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Admin
{
    public class AddSubscriptionModel : PageModel
    {
        private readonly ApiService _apiService;
        public AddSubscriptionModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public int Months { get; set; }
        [BindProperty]
        public int ShopId { get; set; }
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet(int shopId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                ShopId = shopId;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync(int shopId, int months)
        {
            if (Months <= 0)
            {
                ErrorMessage = "Enter a valid month!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                await _apiService.PostAsync<bool>($"api/admin/subscription/{shopId}/{months}", null, getToken);
                return RedirectToPage("/Admin/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to add subscription. Try again!";
                return Page();
            }
        }
    }
}
