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
        public IActionResult OnGet()
        {
            var getToken = HttpContext.Session.GetString("JwtToken");
            try
            {
                if (getToken != null)
                {
                    return RedirectToPage("/Dashboard/Index");
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                ErrorMessage = "Phone and Password are required!";
                return Page();
            }
            try
            {
                var response = await _apiService.PostAsync<AuthResponseModel>("api/auth/login", Input, "");
                HttpContext.Session.SetString("JwtToken", response.Token);
                HttpContext.Session.SetString("ShopName", response.ShopName);
                HttpContext.Session.SetString("ShopId", response.ShopId.ToString());
                HttpContext.Session.SetString("Role", response.Role);
                HttpContext.Session.SetString("OwnerName", response.OwnerName);
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Invalid phone or password. Try again.";
                return Page();
            }
        }
    }
}
