using HisaabPlus.Data.Context;
using HisaabPlus.Data.Entities;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HisaabPlus.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _db;
        public SupplierService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<SupplierDTO>> GetAllAsync(int shopId)
        {
            var getSuppliers = await _db.Suppliers.Where(s => s.ShopId == shopId).ToListAsync();
            var mapSuppliers = getSuppliers.Select(s => new SupplierDTO
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                PhoneNumber = s.PhoneNumber,
                CompanyName = s.CompanyName,
                TotalBalance = s.TotalBalance,
                CreatedAt = s.CreatedAt
            }).ToList();
            return mapSuppliers;
        }
        public async Task<SupplierDetailDTO> GetByIdAsync(int supplierId, int shopId)
        {
            var getSupplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId && s.ShopId == shopId);
            if(getSupplier == null)
            {
                throw new Exception("Supplier Not Found!");
            }
            var stockPurchases = await _db.StockPurchases.Where(s => s.SupplierId == supplierId).Select(p => new PurchaseHistoryDTO
            {
                PurchaseId = p.PurchaseId,
                PurchaseDate = p.PurchaseDate,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                RemainingAmount = p.RemainingAmount,
                PaymentType = p.PaymentType
            }).ToListAsync();
            var stockPayments = await _db.SupplierPayments.Where(s => s.SupplierId == supplierId).Select(p => new SupplierPaymentHistoryDTO
            {
                PaymentId = p.PaymentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Notes = p.Notes
            }).ToListAsync();
            var mapSuppliers = new SupplierDetailDTO
            {
                SupplierId = getSupplier.SupplierId,
                SupplierName = getSupplier.SupplierName,
                PhoneNumber = getSupplier.PhoneNumber,
                CompanyName = getSupplier.CompanyName,
                TotalBalance = getSupplier.TotalBalance,
                PurchaseHistory = stockPurchases,
                PaymentHistory = stockPayments
            };
            return mapSuppliers;    
        }
        public async Task<SupplierDTO> AddAsync(SupplierDTO supplierDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(supplierDTO.SupplierName))
            {
                throw new Exception("Supplier name is required!");
            }

            if (string.IsNullOrWhiteSpace(supplierDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(supplierDTO.CompanyName))
            {
                throw new Exception("Company name is required!");
            }

            if (!supplierDTO.PhoneNumber.All(char.IsDigit) || supplierDTO.PhoneNumber.Length != 11)
            {
                throw new Exception("Phone number must be 11 digits!");
            }

            var phoneExistsInShops = await _db.Shops.AnyAsync(s => s.PhoneNumber == supplierDTO.PhoneNumber);
            var phoneExistsInCustomers = await _db.Customers.AnyAsync(c => c.PhoneNumber == supplierDTO.PhoneNumber && c.ShopId == shopId);

            if (phoneExistsInShops || phoneExistsInCustomers)
            {
                throw new Exception("This phone number is already registered!");
            }

            var existingSupplier = await _db.Suppliers.FirstOrDefaultAsync(s =>
                s.ShopId == shopId &&
                s.PhoneNumber == supplierDTO.PhoneNumber);

            if (existingSupplier != null)
            {
                throw new Exception("Supplier with this phone number already exists!");
            }

            var existingSupplierName = await _db.Suppliers.FirstOrDefaultAsync(s =>
                s.ShopId == shopId &&
                s.SupplierName.ToLower() == supplierDTO.SupplierName.ToLower());

            if (existingSupplierName != null)
            {
                throw new Exception("Supplier with this name already exists!");
            }
            var addSupplier = new Supplier
            {
                ShopId = shopId,
                SupplierName = supplierDTO.SupplierName,
                PhoneNumber = supplierDTO.PhoneNumber,
                CompanyName = supplierDTO.CompanyName,
                TotalBalance = 0,
                CreatedAt = GetPakistanTime()
            };
            await _db.Suppliers.AddAsync(addSupplier);
            await _db.SaveChangesAsync();
            var mapSupplier = new SupplierDTO
            {
                SupplierId = addSupplier.SupplierId,
                SupplierName = addSupplier.SupplierName,
                PhoneNumber = addSupplier.PhoneNumber,
                CompanyName = addSupplier.CompanyName,
                TotalBalance = addSupplier.TotalBalance,
                CreatedAt = addSupplier.CreatedAt
            };
            return mapSupplier;
        }
        public async Task<bool> UpdateAsync(int supplierId, SupplierDTO supplierDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(supplierDTO.SupplierName))
            {
                throw new Exception("Supplier name is required!");
            }

            if (string.IsNullOrWhiteSpace(supplierDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(supplierDTO.CompanyName))
            {
                throw new Exception("Company name is required!");
            }

            if (!supplierDTO.PhoneNumber.All(char.IsDigit) || supplierDTO.PhoneNumber.Length != 11)
            {
                throw new Exception("Phone number must be 11 digits!");
            }

            var phoneExistsInShops = await _db.Shops.AnyAsync(s => s.PhoneNumber == supplierDTO.PhoneNumber);
            var phoneExistsInCustomers = await _db.Customers.AnyAsync(c => c.PhoneNumber == supplierDTO.PhoneNumber && c.ShopId == shopId);

            if (phoneExistsInShops || phoneExistsInCustomers)
            {
                throw new Exception("This phone number is already registered!");
            }

            var existingSupplier = await _db.Suppliers.FirstOrDefaultAsync(s =>
                s.ShopId == shopId &&
                s.PhoneNumber == supplierDTO.PhoneNumber && s.SupplierId != supplierId);

            if (existingSupplier != null)
            {
                throw new Exception("Supplier with this phone number already exists!");
            }

            var existingSupplierName = await _db.Suppliers.FirstOrDefaultAsync(s =>
                s.ShopId == shopId &&
                s.SupplierName.ToLower() == supplierDTO.SupplierName.ToLower());

            if (existingSupplierName != null)
            {
                throw new Exception("Supplier with this name already exists!");
            }

            var getSupplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId && s.ShopId == shopId);
            if (getSupplier == null)
            {
                throw new Exception("Supplier Not Found!");
            }
            getSupplier.SupplierName = supplierDTO.SupplierName;
            getSupplier.PhoneNumber = supplierDTO.PhoneNumber;
            getSupplier.CompanyName = supplierDTO.CompanyName;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RecordStockPurchaseAsync(StockPurchaseDTO stockPurchaseDTO, int shopId, int userId)
        {
            var getSupplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == stockPurchaseDTO.SupplierId && s.ShopId == shopId);
            if (getSupplier == null)
            {
                throw new Exception("Supplier Not Found!");
            }
            if (string.IsNullOrWhiteSpace(stockPurchaseDTO.PaymentType))
            {
                throw new Exception("Payment type is required!");
            }

            if (stockPurchaseDTO.Items == null || stockPurchaseDTO.Items.Count == 0)
            {
                throw new Exception("At least one product is required!");
            }

            foreach (var item in stockPurchaseDTO.Items)
            {
                if (item.Quantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0!");
                }
                if (item.UnitPrice <= 0)
                {
                    throw new Exception("Unit price must be greater than 0!");
                }
            }

            if (stockPurchaseDTO.PaidAmount < 0)
            {
                throw new Exception("Paid amount cannot be negative!");
            }

            decimal totalAmount = stockPurchaseDTO.Items.Sum(p => p.Quantity * p.UnitPrice);

            if (stockPurchaseDTO.PaidAmount > totalAmount)
            {
                throw new Exception("Paid amount cannot exceed total amount!");
            }
            var stockPurchase = new StockPurchase
            {
                ShopId = shopId,
                SupplierId = stockPurchaseDTO.SupplierId,
                PurchaseDate = GetPakistanTime(),
                TotalAmount = totalAmount,
                PaymentType = stockPurchaseDTO.PaymentType,
                PaidAmount = stockPurchaseDTO.PaidAmount,
                RemainingAmount = totalAmount - stockPurchaseDTO.PaidAmount,
                CreatedBy = userId
            };
            await _db.StockPurchases.AddAsync(stockPurchase);
            await _db.SaveChangesAsync();
            foreach(var item in stockPurchaseDTO.Items)
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                if(product == null)
                {
                    throw new Exception("Product not found!");
                }
                var stockPurchaseItem = new StockPurchaseItem
                {
                    PurchaseId = stockPurchase.PurchaseId,
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                };
                product.CurrentStock += item.Quantity;
                await _db.StockPurchaseItems.AddAsync(stockPurchaseItem);
            }
            if(stockPurchaseDTO.PaymentType.ToLower() == "credit")
            {
                getSupplier.TotalBalance += stockPurchase.RemainingAmount;
            }
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RecordSupplierPaymentAsync(SupplierPaymentDTO supplierPaymentDTO, int shopId, int userId)
        {
            var getSupplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierPaymentDTO.SupplierId && s.ShopId == shopId);
            if (getSupplier == null)
            {
                throw new Exception("Supplier Not Found!");
            }
            if (supplierPaymentDTO.Amount <= 0)
            {
                throw new Exception("Amount must be greater than 0!");
            }
            if (supplierPaymentDTO.Amount > getSupplier.TotalBalance)
            {
                throw new Exception("Amount exceeds supplier balance!");
            }
            var supplierPayment = new SupplierPayment
            {
                ShopId = shopId,
                SupplierId = supplierPaymentDTO.SupplierId,
                Amount = supplierPaymentDTO.Amount,
                Notes = supplierPaymentDTO.Notes,
                PaymentDate = GetPakistanTime(),
                PaidBy = userId
            };
            getSupplier.TotalBalance -= supplierPaymentDTO.Amount;
            await _db.SupplierPayments.AddAsync(supplierPayment);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<SupplierDTO>> GetSuppliersWithBalanceAsync(int shopId)
        {
            var getSupplier = await _db.Suppliers.Where(s => s.ShopId == shopId && s.TotalBalance > 0).ToListAsync();
            var mapSupplier = getSupplier.Select(s => new SupplierDTO
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                PhoneNumber = s.PhoneNumber,
                CompanyName = s.CompanyName,
                TotalBalance = s.TotalBalance,
                CreatedAt = s.CreatedAt
            }).ToList();
            return mapSupplier;
        }
        public async Task<List<StockPurchaseResponseDTO>> GetMonthlyPurchasesAsync(int shopId)
        {
            var today = GetPakistanTime();
            var startDate = new DateTime(today.Year, today.Month, 1);
            var endDate = startDate.AddMonths(1);
            var itemsPurchases = await _db.StockPurchases.Include(i => i.StockPurchaseItems).Include(l => l.Supplier).Where(p => p.ShopId == shopId && p.PurchaseDate >= startDate && p.PurchaseDate < endDate).ToListAsync();
            var mapItemsPurchases = itemsPurchases.Select(p => new StockPurchaseResponseDTO
            {
                PurchaseId = p.PurchaseId,
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier.SupplierName,
                PurchaseDate = p.PurchaseDate,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                RemainingAmount = p.RemainingAmount,
                PaymentType = p.PaymentType,
                Items = p.StockPurchaseItems.Select(p => new StockPurchaseItemDTO
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    TotalPrice = p.TotalPrice
                }).ToList()
            }).ToList();
            return mapItemsPurchases;
        }
        public async Task<StockPurchaseResponseDTO> GetPurchaseByIdAsync(int purchaseId, int shopId)
        {
            var getPurchase = await _db.StockPurchases.Include(i => i.StockPurchaseItems).ThenInclude(v => v.Product).Include(k => k.Supplier).FirstOrDefaultAsync(p => p.PurchaseId == purchaseId && p.ShopId == shopId);
            if(getPurchase == null)
            {
                throw new Exception("Purchase not found");
            }
            var mapPurchase = new StockPurchaseResponseDTO
            {
                PurchaseId = getPurchase.PurchaseId,
                SupplierId = getPurchase.SupplierId,
                SupplierName = getPurchase.Supplier.SupplierName,
                PurchaseDate = getPurchase.PurchaseDate,
                TotalAmount = getPurchase.TotalAmount,
                PaidAmount = getPurchase.PaidAmount,
                RemainingAmount = getPurchase.RemainingAmount,
                PaymentType = getPurchase.PaymentType,
                Items = getPurchase.StockPurchaseItems.Select(p => new StockPurchaseItemDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    TotalPrice = p.TotalPrice
                }).ToList()
            };
            return mapPurchase;
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
