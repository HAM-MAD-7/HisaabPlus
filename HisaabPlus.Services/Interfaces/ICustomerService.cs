using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllAsync(int shopId);
        Task<CustomerDetailDTO> GetByIdAsync(int customerId, int shopId);
        Task<CustomerDTO> AddAsync(CustomerDTO customerDTO, int shopId);
        Task<bool> UpdateAsync(int customerId, CustomerDTO customerDTO, int shopId);
        Task<bool> LoanRecordAsync(LoanDTO loanDTO, int shopId, int userId);
        Task<bool> LoanPayRecordAsync(PayLoanDTO payLoanDTO, int shopId, int userId);
        Task<List<CustomerDTO>> GetCustomerWithBalanceAsync(int shopId);
    }
}
