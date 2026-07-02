using HisaabPlus.Data.Entities;
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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet("GetAllSupplier")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _supplierService.GetAllAsync(GetShopId());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOneSupplier/{supplierId}")]
        public async Task<IActionResult> GetById(int supplierId)
        {
            try
            {
                var result = await _supplierService.GetByIdAsync(supplierId,GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddSupplier")]
        public async Task<IActionResult> Add(SupplierDTO supplierDTO)
        {
            try
            {
                var result = await _supplierService.AddAsync(supplierDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateSupplier/{supplierId}")]
        public async Task<IActionResult> Update(int supplierId,SupplierDTO supplierDTO)
        {
            try
            {
                var result = await _supplierService.UpdateAsync(supplierId, supplierDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("StockPurchase")]
        public async Task<IActionResult> RecordStockPurchase(StockPurchaseDTO stockPurchaseDTO)
        {
            try
            {
                var result = await _supplierService.RecordStockPurchaseAsync(stockPurchaseDTO,GetShopId(), GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("StockPayment")]
        public async Task<IActionResult> RecordSupplierPayment(SupplierPaymentDTO supplierPaymentDTO)
        {
            try
            {
                var result = await _supplierService.RecordSupplierPaymentAsync(supplierPaymentDTO, GetShopId(), GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SupplierWithBalance")]
        public async Task<IActionResult> GetSuppliersWithBalance()
        {
            try
            {
                var result = await _supplierService.GetSuppliersWithBalanceAsync(GetShopId());
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
