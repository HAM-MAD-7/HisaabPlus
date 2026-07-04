using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HisaabPlus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesServiceController : ControllerBase
    {
        private readonly ISalesService _salesService;
        public SalesServiceController(ISalesService salesService)
        {
            _salesService = salesService;
        }
        [HttpPost("AddSale")]
        public async Task<IActionResult> CreateSale(CreateSaleDTO createSaleDTO)
        {
            try
            {
                var result = await _salesService.CreateSaleAsync(createSaleDTO, GetShopId(), GetUserId());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getSales/today")]
        public async Task<IActionResult> GetTodaySales()
        {
            try
            {
                var result = await _salesService.GetTodaySalesAsync(GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getOneSale/{saleId}")]
        public async Task<IActionResult> GetSaleById(int saleId)
        {
            try
            {
                var result = await _salesService.GetSaleByIdAsync(saleId, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private int GetShopId()
        {
            var shopId = User.Claims.FirstOrDefault(i => i.Type == "ShopId")?.Value;
            return int.Parse(shopId);
        }
        private int GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(i => i.Type == "UserId")?.Value;
            return int.Parse(userId);
        }
    }
}
