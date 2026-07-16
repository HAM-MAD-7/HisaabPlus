using HisaabPlus.Web.Models;
using HisaabPlus.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HisaabPlus.Web.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;
        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public List<ShopModel> Shops { get; set; } = new();
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if(getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                Shops = await _apiService.GetAsync<List<ShopModel>>("api/admin/shops", getToken);
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load shops. Try again!";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostLockAsync(int shopId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                await _apiService.PutAsync<bool>($"api/admin/lock/{shopId}", null, getToken);
                return RedirectToPage("/Admin/Index");
            }
            catch(Exception)
            {
                ErrorMessage = "Failed to Lock shop!";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostUnlockAsync(int shopId)
        {
            try
            {
                var getToken = HttpContext.Session.GetString("JwtAdminToken");
                if (getToken == null)
                {
                    return RedirectToPage("/Admin/AdminLogin");
                }
                await _apiService.PutAsync<bool>($"api/admin/unlock/{shopId}", null, getToken);
                return RedirectToPage("/Admin/Index");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to Unlock shop!";
                return Page();
            }
        }
    }
}
