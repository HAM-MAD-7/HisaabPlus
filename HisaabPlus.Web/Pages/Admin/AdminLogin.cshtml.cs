using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Admin
{
    public class AdminLoginModel : PageModel
    {
        private readonly ApiService _apiService;
        public AdminLoginModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty]
        public AdminModel Input { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public IActionResult OnGet()
        {
            var getToken = HttpContext.Session.GetString("JwtAdminToken");
            try
            {
                if (getToken != null)
                {
                    return RedirectToPage("/Admin/Index");
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
                ErrorMessage = "Feilds are invalid or missing!";
                return Page();
            }
            try
            {
                var response = await _apiService.PostAsync<AuthResponseModel>("api/auth/admin-login", Input, "");
                HttpContext.Session.SetString("JwtAdminToken", response.Token);
                HttpContext.Session.SetString("Role", response.Role);
                return RedirectToPage("/Admin/Index");
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
