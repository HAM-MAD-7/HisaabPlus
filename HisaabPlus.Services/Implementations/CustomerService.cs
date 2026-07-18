using HisaabPlus.Data.Context;
using HisaabPlus.Data.Entities;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _db;
        public CustomerService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<CustomerDTO>> GetAllAsync(int shopId)
        {
            var getCustomers = await _db.Customers.Where(x => x.ShopId == shopId).ToListAsync();
            var mapCustomers = getCustomers.Select(x => new CustomerDTO
            {
                CustomerId = x.CustomerId,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                TotalBalance = x.TotalBalance,
                CreatedAt = x.CreatedAt
            }).ToList();
            return mapCustomers;
        }
        public async Task<CustomerDetailDTO> GetByIdAsync(int customerId, int shopId)
        {
            var getCustomer = await _db.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.ShopId == shopId);
            if(getCustomer == null)
            {
                throw new Exception("Customer Not Found!");
            }
            var getSales = await _db.Sales.Where(s => s.PaymentType == "Loan" && s.CustomerId == getCustomer.CustomerId).Select(p => new LoanHistoryDTO
            {
                SaleId = p.SaleId,
                SaleDate = p.SaleDate,
                TotalAmount = p.TotalAmount,
                RemainingAmount = p.RemainingAmount,
                PaymentType = p.PaymentType
            }).ToListAsync();
            var getCustomerPayments = await _db.CustomerPayments.Where(c => c.CustomerId == getCustomer.CustomerId).Select(p => new PaymentHistoryDTO
            {
                PaymentId = p.PaymentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Notes = p.Notes

            }).ToListAsync();
            var mapCustomerDetail = new CustomerDetailDTO
            {
                CustomerId = customerId,
                FullName = getCustomer.FullName,
                PhoneNumber = getCustomer.PhoneNumber,
                Address = getCustomer.Address,
                TotalBalance = getCustomer.TotalBalance,
                LoanHistory = getSales,
                PaymentHistory = getCustomerPayments
            };
            return mapCustomerDetail;
        }
        public async Task<CustomerDTO> AddAsync(CustomerDTO customerDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(customerDTO.FullName))
            {
                throw new Exception("Customer name is required!");
            }

            if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(customerDTO.Address))
            {
                throw new Exception("Address is required!");
            }

            if (!customerDTO.PhoneNumber.All(char.IsDigit) || customerDTO.PhoneNumber.Length != 11)
            {
                throw new Exception("Phone number must be 11 digits!");
            }

            var phoneExistsInShops = await _db.Shops.AnyAsync(s => s.PhoneNumber == customerDTO.PhoneNumber);
            var phoneExistsInSuppliers = await _db.Suppliers.AnyAsync(s => s.PhoneNumber == customerDTO.PhoneNumber && s.ShopId == shopId);

            if (phoneExistsInShops || phoneExistsInSuppliers)
            {
                throw new Exception("This phone number is already registered!");
            }

            var existingCustomer = await _db.Customers.FirstOrDefaultAsync(c =>
                c.ShopId == shopId &&
                c.PhoneNumber == customerDTO.PhoneNumber);

            if (existingCustomer != null)
            {
                throw new Exception("Customer with this phone number already exists!");
            }

            var addCustomer = new Customer
            {
                ShopId = shopId,
                TotalBalance = 0,
                CreatedAt = GetPakistanTime(),
                FullName = customerDTO.FullName,
                Address = customerDTO.Address,
                PhoneNumber = customerDTO.PhoneNumber
            };
            await _db.Customers.AddAsync(addCustomer);
            await _db.SaveChangesAsync();
            var mapCustomer = new CustomerDTO
            {
                CustomerId = addCustomer.CustomerId,
                FullName = addCustomer.FullName,
                Address = addCustomer.Address,
                PhoneNumber = addCustomer.PhoneNumber,
                TotalBalance= addCustomer.TotalBalance,
                CreatedAt = GetPakistanTime()
            };
            return mapCustomer;
        }
        public async Task<bool> UpdateAsync(int customerId, CustomerDTO customerDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(customerDTO.FullName))
            {
                throw new Exception("Customer name is required!");
            }

            if (string.IsNullOrWhiteSpace(customerDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(customerDTO.Address))
            {
                throw new Exception("Address is required!");
            }

            if (!customerDTO.PhoneNumber.All(char.IsDigit) || customerDTO.PhoneNumber.Length != 11)
            {
                throw new Exception("Phone number must be 11 digits!");
            }

            var phoneExistsInShops = await _db.Shops.AnyAsync(s => s.PhoneNumber == customerDTO.PhoneNumber);
            var phoneExistsInSuppliers = await _db.Suppliers.AnyAsync(s => s.PhoneNumber == customerDTO.PhoneNumber && s.ShopId == shopId);

            if (phoneExistsInShops || phoneExistsInSuppliers)
            {
                throw new Exception("This phone number is already registered!");
            }

            var existingCustomer = await _db.Customers.FirstOrDefaultAsync(c =>
                c.ShopId == shopId &&
                c.PhoneNumber == customerDTO.PhoneNumber && c.CustomerId != customerId);

            if (existingCustomer != null)
            {
                throw new Exception("Customer with this phone number already exists!");
            }

            var getCustomer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.ShopId == shopId);
            if(getCustomer == null)
            {
                return false;
            }
            getCustomer.FullName = customerDTO.FullName;
            getCustomer.PhoneNumber = customerDTO.PhoneNumber;
            getCustomer.Address = customerDTO.Address;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> LoanRecordAsync(LoanDTO loanDTO, int shopId, int userId)
        {
            var getCustomer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == loanDTO.CustomerId && c.ShopId == shopId);
            if (getCustomer == null)
            {
                throw new Exception("Customer Not Found!");
            }
            var newSale = new Sale
            { 
                ShopId = shopId,
                CustomerId = loanDTO.CustomerId,
                SaleDate = GetPakistanTime(),
                TotalAmount = loanDTO.Amount,
                PaymentType = "Loan",
                PaidAmount = 0,
                RemainingAmount = loanDTO.Amount,
                CreatedBy = userId
            };
            await _db.Sales.AddAsync(newSale);
            getCustomer.TotalBalance += loanDTO.Amount;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> LoanPayRecordAsync(PayLoanDTO payLoanDTO, int shopId, int userId)
        {
            var getCustomer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == payLoanDTO.CustomerId && c.ShopId == shopId);
            if (getCustomer == null)
            {
                throw new Exception("Customer Not Found!");
            }
            if(payLoanDTO.Amount > getCustomer.TotalBalance)
            {
                throw new Exception("Amount exceeds customer balance!");
            }
            var newPayment = new CustomerPayment
            {
                ShopId = shopId,
                CustomerId = payLoanDTO.CustomerId,
                Amount = payLoanDTO.Amount,
                PaymentDate = GetPakistanTime(),
                Notes = payLoanDTO.Notes,
                ReceivedBy = userId
            };
            await _db.CustomerPayments.AddAsync(newPayment);
            getCustomer.TotalBalance -= payLoanDTO.Amount;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<CustomerDTO>> GetCustomerWithBalanceAsync(int shopId)
        {
            var getCustomer = await _db.Customers.Where(c => c.ShopId == shopId && c.TotalBalance > 0).ToListAsync();
            var mapCustomers = getCustomer.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                TotalBalance = c.TotalBalance,
                CreatedAt = GetPakistanTime(),
            }).ToList();
            return mapCustomers;
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
