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
            var addSupplier = new Supplier
            {
                ShopId = shopId,
                SupplierName = supplierDTO.SupplierName,
                PhoneNumber = supplierDTO.PhoneNumber,
                CompanyName = supplierDTO.CompanyName,
                TotalBalance = 0,
                CreatedAt = DateTime.UtcNow,
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
            var stockPurchase = new StockPurchase
            {
                ShopId = shopId,
                SupplierId = stockPurchaseDTO.SupplierId,
                PurchaseDate = DateTime.UtcNow,
                TotalAmount = stockPurchaseDTO.TotalAmount,
                PaymentType = stockPurchaseDTO.PaymentType,
                PaidAmount = stockPurchaseDTO.PaidAmount,
                RemainingAmount = stockPurchaseDTO.TotalAmount - stockPurchaseDTO.PaidAmount,
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
                    TotalPrice = item.TotalPrice,
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
            if(supplierPaymentDTO.Amount > getSupplier.TotalBalance)
            {
                throw new Exception("Amount exceeds supplier balance!");
            }
            var supplierPayment = new SupplierPayment
            {
                ShopId = shopId,
                SupplierId = supplierPaymentDTO.SupplierId,
                Amount = supplierPaymentDTO.Amount,
                Notes = supplierPaymentDTO.Notes,
                PaymentDate = DateTime.UtcNow,
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
    }
}
