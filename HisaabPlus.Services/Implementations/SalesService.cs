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
    public class SalesService : ISalesService
    {
        private readonly AppDbContext _db;
        public SalesService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<SaleResponseDTO> CreateSaleAsync(CreateSaleDTO createSaleDTO, int shopId, int userId)
        {
            if (string.IsNullOrWhiteSpace(createSaleDTO.PaymentType))
            {
                throw new Exception("Payment type is required!");
            }

            if (createSaleDTO.Items == null || createSaleDTO.Items.Count == 0)
            {
                throw new Exception("At least one product is required!");
            }

            if (createSaleDTO.PaymentType.ToLower() == "loan" && !createSaleDTO.CustomerId.HasValue)
            {
                throw new Exception("Loan sales require a registered customer!");
            }

            if (createSaleDTO.PaidAmount < 0)
            {
                throw new Exception("Paid amount cannot be negative!");
            }

            decimal totalAmount = 0;
            var saleProducts = new List<(Product product, SaleItemInputDTO item)>();
            foreach(var items in createSaleDTO.Items)
            {
                if (items.Quantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0!");
                }
                var getProduct = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == items.ProductId && p.ShopId == shopId && p.IsActive == true);
                if(getProduct == null)
                {
                    throw new Exception("Product not found!");
                }
                if(getProduct.CurrentStock < items.Quantity)
                {
                    throw new Exception($"Insufficient stock for {getProduct.ProductName}");
                }
                var itemTotal = items.Quantity * getProduct.SellingPrice;
                totalAmount += itemTotal;
                saleProducts.Add((getProduct, items));
            }
            if (createSaleDTO.PaidAmount > totalAmount)
            {
                throw new Exception("Paid amount cannot exceed total amount!");
            }
            var newSale = new Sale
            {
                ShopId = shopId,
                CustomerId = createSaleDTO.CustomerId,
                SaleDate = GetPakistanTime(),
                TotalAmount = totalAmount,
                PaymentType = createSaleDTO.PaymentType,
                PaidAmount = createSaleDTO.PaidAmount,
                RemainingAmount = totalAmount - createSaleDTO.PaidAmount,
                CreatedBy = userId
            };
            await _db.Sales.AddAsync(newSale);
            await _db.SaveChangesAsync();
            foreach(var (product, item) in saleProducts)
            {
                var newSaleItem = new SaleItem
                {
                    SaleId = newSale.SaleId,
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.SellingPrice,
                    TotalPrice = item.Quantity * product.SellingPrice
                };
                await _db.SaleItems.AddAsync(newSaleItem);
                product.CurrentStock -= item.Quantity;
            }
            await _db.SaveChangesAsync();
            if(createSaleDTO.PaymentType.ToLower() == "loan" && createSaleDTO.CustomerId.HasValue)
            {
                var getCustomer = await _db.Customers.FirstOrDefaultAsync(p => p.CustomerId == createSaleDTO.CustomerId.Value && p.ShopId == shopId);
                if(getCustomer == null)
                {
                    throw new Exception("Customer not found!");
                }
                else
                {
                    getCustomer.TotalBalance += newSale.RemainingAmount;
                }
                await _db.SaveChangesAsync();
            }
            var customerName = "Walk-in customer";
            if(createSaleDTO.CustomerId.HasValue)
            {
                var getCustomer = await _db.Customers.FirstOrDefaultAsync(p => p.CustomerId == createSaleDTO.CustomerId.Value && p.ShopId == shopId);
                if (getCustomer == null)
                {
                    throw new Exception("Customer not found!");
                }
                customerName = getCustomer.FullName;
            }
            var saleItemDetails = new List<SaleItemDetailDTO>();
            foreach(var (product, item) in saleProducts)
            {
                var newSaleItem = new SaleItemDetailDTO
                {
                    ProductName = product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = product.SellingPrice,
                    TotalPrice = item.Quantity * product.SellingPrice
                };
                saleItemDetails.Add(newSaleItem);
            }
            return new SaleResponseDTO
            {
                SaleId =newSale.SaleId,
                SaleDate = newSale.SaleDate,
                TotalAmount = newSale.TotalAmount,
                PaidAmount = newSale.PaidAmount,
                RemainingAmount = newSale.RemainingAmount,
                PaymentType = newSale.PaymentType,
                CustomerName = customerName,
                Items = saleItemDetails
            };
        }
        public async Task<List<SaleResponseDTO>> GetTodaySalesAsync(int shopId)
        {
            var todayStart = GetPakistanTime().Date;
            var todayEnd = todayStart.AddDays(1);
            var customerName = "";
            var result = new List<SaleResponseDTO>();
            var getSales = await _db.Sales.Where(s => s.ShopId == shopId && s.SaleDate >= todayStart && s.SaleDate < todayEnd).Include(p => p.SaleItems).ThenInclude(si => si.Product).ToListAsync();
            foreach(var item in getSales)
            {
                if (item.CustomerId.HasValue)
                {
                    var getCustomer = await _db.Customers.FirstOrDefaultAsync(p => p.CustomerId == item.CustomerId.Value && p.ShopId == shopId);
                    if (getCustomer == null)
                    {
                        throw new Exception("Customer not found!");
                    }
                    customerName = getCustomer.FullName;
                }
                else
                {
                    customerName = "Walk-in customer";
                }
                var mapSaleItems = item.SaleItems.Select(s => new SaleItemDetailDTO
                {
                    ProductName = s.Product.ProductName,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    TotalPrice = s.TotalPrice
                }).ToList();
                result.Add(new SaleResponseDTO
                {
                    SaleId = item.SaleId,
                    SaleDate = item.SaleDate,
                    TotalAmount = item.TotalAmount,
                    PaidAmount = item.PaidAmount,
                    RemainingAmount = item.RemainingAmount,
                    PaymentType = item.PaymentType,
                    CustomerName = customerName,
                    Items = mapSaleItems
                });
            }
            return result;
        }
        public async Task<SaleResponseDTO> GetSaleByIdAsync(int saleId, int shopId)
        {
            var customerName = "";
            var getSale = await _db.Sales.Include(si => si.SaleItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(s => s.SaleId == saleId && s.ShopId == shopId);
            if (getSale == null)
            {
                throw new Exception("Sale not found!");
            }
            if (getSale.CustomerId.HasValue)
            {
                var getCustomer = await _db.Customers.FirstOrDefaultAsync(p => p.CustomerId == getSale.CustomerId.Value && p.ShopId == shopId);
                if (getCustomer == null)
                {
                    throw new Exception("Customer not found!");
                }
                customerName = getCustomer.FullName;
            }
            else
            {
                customerName = "Walk-in customer";
            }
            var mapSaleItems = getSale.SaleItems.Select(p => new SaleItemDetailDTO
            {
                ProductName = p.Product.ProductName,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                TotalPrice = p.TotalPrice
            }).ToList();
            return new SaleResponseDTO
            {
                SaleId = getSale.SaleId,
                SaleDate = getSale.SaleDate,
                TotalAmount = getSale.TotalAmount,
                PaidAmount = getSale.PaidAmount,
                RemainingAmount = getSale.RemainingAmount,
                PaymentType = getSale.PaymentType,
                CustomerName = customerName,
                Items = mapSaleItems
            };
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
