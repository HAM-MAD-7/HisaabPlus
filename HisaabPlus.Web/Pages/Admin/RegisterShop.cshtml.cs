using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Admin
{
    public class RegisterShopModel : PageModel
    {
        private readonly ApiService _apiService;
        public RegisterShopModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public RegisterModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
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
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Feilds are invalid or missing!";
                return Page();
            }
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                await _apiService.PostAsync<AuthResponseModel>("api/auth/register", Input, getToken);
                return RedirectToPage("/Admin/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Feilds are invalid!";
                return Page();
            }
        }
    }
}
