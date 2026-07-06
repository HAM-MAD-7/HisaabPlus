using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _apiService;
        public LoginModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public Models.LoginInputModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var response = await _apiService.PostAsync<AuthResponseModel>("api/auth/login", Input, "");
                HttpContext.Session.SetString("JwtToken", response.Token);
                HttpContext.Session.SetString("ShopName", response.ShopName);
                HttpContext.Session.SetString("ShopId", response.ShopId.ToString());
                HttpContext.Session.SetString("OwnerName", response.OwnerName);
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
