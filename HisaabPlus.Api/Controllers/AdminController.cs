using HisaabPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HisaabPlus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("shops")]
        public async Task<IActionResult> GetAllShopsAsync()
        {
            if(!IsAdmin())
            {
                return Forbid();
            }
            try
            {
                var result = await _adminService.GetAllShopsAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("lock/{shopId}")]
        public async Task<IActionResult> LockShopAsync(int shopId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            try
            {
                var result = await _adminService.LockShopAsync(shopId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("unlock/{shopId}")]
        public async Task<IActionResult> UnlockShopAsync(int shopId)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            try
            {
                var result = await _adminService.UnlockShopAsync(shopId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("subscription/{shopId}/{months}")]
        public async Task<IActionResult> AddSubscriptionAsync(int shopId, int months)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            try
            {
                var result = await _adminService.AddSubscriptionAsync(shopId,months);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private bool IsAdmin()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return role == "Admin";
        }
    }
}
