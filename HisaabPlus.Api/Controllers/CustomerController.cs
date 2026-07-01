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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _customerService.GetAllAsync(GetShopId());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOneCustomer/{customerId}")]
        public async Task<IActionResult> GetById(int customerId)
        {
            try
            {
                var result = await _customerService.GetByIdAsync(customerId,GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddCustomer")]
        public async Task<IActionResult> Add(CustomerDTO customerDTO)
        {
            try
            {
                var result = await _customerService.AddAsync(customerDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateCustomer/{customerId}")]
        public async Task<IActionResult> Update(int customerId, CustomerDTO customerDTO)
        {
            try
            {
                var result = await _customerService.UpdateAsync(customerId, customerDTO, GetShopId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddLoanRecord")]
        public async Task<IActionResult> LoanRecord(LoanDTO loanDTO)
        {
            try
            {
                var result = await _customerService.LoanRecordAsync(loanDTO, GetShopId(), GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddPayLoanRecord")]
        public async Task<IActionResult> PayLoanRecord(PayLoanDTO payLoanDTO)
        {
            try
            {
                var result = await _customerService.LoanPayRecordAsync(payLoanDTO, GetShopId(), GetUserId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCustomerBalance")]
        public async Task<IActionResult> GetCustomerBalance()
        {
            try
            {
                var result = await _customerService.GetCustomerWithBalanceAsync(GetShopId());
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
