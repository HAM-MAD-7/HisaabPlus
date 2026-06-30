using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HisaabPlus.Services.Interfaces;
using HisaabPlus.Services.DTOs;

namespace HisaabPlus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _productService.GetAllAsync(GetShopId());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOneProduct/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            try
            {
                var result = await _productService.GetByIdAsync(productId,GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add(ProductDTO productDTO)
        {
            try
            {
                var result = await _productService.AddAsync(productDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> Update(int productId, ProductDTO productDTO)
        {
            try
            {
                var result = await _productService.UpdateAsync(productId,productDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var result = await _productService.DeleteAsync(productId, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("LowStock")]
        public async Task<IActionResult> GetLowStock()
        {
            try
            {
                var result = await _productService.GetLowStockAsync(GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private int GetShopId()
        {
            var id = User.Claims.FirstOrDefault(c => c.Type == "ShopId")?.Value;
            if(string.IsNullOrEmpty(id))
            {
                throw new Exception("ShopId not found in token!");
            }
            return int.Parse(id);
        }
    }
}
