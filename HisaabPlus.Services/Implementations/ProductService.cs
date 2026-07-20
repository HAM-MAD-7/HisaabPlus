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
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<ProductDTO>> GetAllAsync(int shopId)
        {
            var products = await _db.Products.Where(p => p.ShopId == shopId).ToListAsync();
            var mapProducts = products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                CurrentStock = p.CurrentStock,
                Unit = p.Unit,
                Category = p.Category,
                LowStockLimit = p.LowStockLimit,
                IsActive = p.IsActive,
            }).ToList();
            return mapProducts;
        }
        public async Task<ProductDTO> GetByIdAsync(int productId, int shopId)
        {
            var getProduct = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId && p.ShopId == shopId);
            if (getProduct == null)
            {
                throw new Exception("Product Not Found!");
            }
            ProductDTO productDTO = new ProductDTO
            {
                ProductId = productId,
                ProductName = getProduct.ProductName,
                Category = getProduct.Category,
                PurchasePrice = getProduct.PurchasePrice,
                SellingPrice = getProduct.SellingPrice,
                CurrentStock = getProduct.CurrentStock,
                Unit = getProduct.Unit,
                LowStockLimit = getProduct.LowStockLimit,
                IsActive = getProduct.IsActive
            };
            return productDTO;
        }
        public async Task<ProductDTO> AddAsync(ProductDTO productDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(productDTO.ProductName))
            {
                throw new Exception("Product name is required!");
            }

            if (productDTO.PurchasePrice <= 0)
            {
                throw new Exception("Purchase price must be greater than 0!");
            }

            if (productDTO.SellingPrice <= 0)
            {
                throw new Exception("Selling price must be greater than 0!");
            }

            if (productDTO.SellingPrice <= productDTO.PurchasePrice)
            {
                throw new Exception("Selling price must be greater than purchase price!");
            }

            if (productDTO.CurrentStock < 0)
            {
                throw new Exception("Current stock cannot be negative!");
            }

            if (productDTO.LowStockLimit <= 0)
            {
                throw new Exception("Low stock limit must be greater than 0!");
            }

            if (productDTO.LowStockLimit >= productDTO.CurrentStock)
            {
                throw new Exception("Current stock must be greater than low stock limit!");
            }

            var existingProduct = await _db.Products.FirstOrDefaultAsync(p =>
                p.ShopId == shopId &&
                p.ProductName.ToLower() == productDTO.ProductName.ToLower() &&
                p.IsActive == true);

            if (existingProduct != null)
            {
                throw new Exception("Product with this name already exists!");
            }
            var addProduct = new Product
            {
                ShopId = shopId,
                ProductName= productDTO.ProductName,
                Category = productDTO.Category,
                PurchasePrice = productDTO.PurchasePrice,
                SellingPrice = productDTO.SellingPrice,
                CurrentStock = productDTO.CurrentStock,
                Unit = productDTO.Unit,
                LowStockLimit = productDTO.LowStockLimit,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _db.Products.AddAsync(addProduct);
            await _db.SaveChangesAsync();
            var mapProduct = new ProductDTO
            {
                ProductId = addProduct.ProductId,
                ProductName = addProduct.ProductName,
                PurchasePrice = addProduct.PurchasePrice,
                SellingPrice = addProduct.SellingPrice,
                Category = addProduct.Category,
                CurrentStock = addProduct.CurrentStock,
                LowStockLimit = addProduct.LowStockLimit,
                Unit = addProduct.Unit,
                IsActive = addProduct.IsActive
            };
            return mapProduct;
        }
        public async Task<bool> UpdateAsync(int productId, ProductDTO productDTO, int shopId)
        {
            if (string.IsNullOrWhiteSpace(productDTO.ProductName))
            {
                throw new Exception("Product name is required!");
            }

            if (productDTO.PurchasePrice <= 0)
            {
                throw new Exception("Purchase price must be greater than 0!");
            }

            if (productDTO.SellingPrice <= 0)
            {
                throw new Exception("Selling price must be greater than 0!");
            }

            if (productDTO.SellingPrice <= productDTO.PurchasePrice)
            {
                throw new Exception("Selling price must be greater than purchase price!");
            }

            if (productDTO.CurrentStock < 0)
            {
                throw new Exception("Current stock cannot be negative!");
            }

            if (productDTO.LowStockLimit <= 0)
            {
                throw new Exception("Low stock limit must be greater than 0!");
            }

            if (productDTO.LowStockLimit >= productDTO.CurrentStock)
            {
                throw new Exception("Current stock must be greater than low stock limit!");
            }

            var existingProduct = await _db.Products.FirstOrDefaultAsync(p =>
                p.ShopId == shopId &&
                p.ProductName.ToLower() == productDTO.ProductName.ToLower() &&
                p.IsActive == true && p.ProductId != productId);

            if (existingProduct != null)
            {
                throw new Exception("Product with this name already exists!");
            }

            var getProduct = await _db.Products.FirstOrDefaultAsync(c => c.ProductId == productId && c.ShopId == shopId);
            if (getProduct == null)
            {
                return false;
            }
            getProduct.ProductName = productDTO.ProductName;
            getProduct.PurchasePrice = productDTO.PurchasePrice;
            getProduct.Category = productDTO.Category;
            getProduct.SellingPrice = productDTO.SellingPrice;
            getProduct.LowStockLimit = productDTO.LowStockLimit;
            getProduct.Unit = productDTO.Unit;
            getProduct.CurrentStock = productDTO.CurrentStock;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int productId, int shopId)
        {
            var getProduct = await _db.Products.FirstOrDefaultAsync(c => c.ProductId == productId && c.ShopId == shopId);
            if (getProduct == null)
            {
                throw new Exception("Product not found!");
            }
            getProduct.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ReactivateAsync(int productId, int shopId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId && p.ShopId == shopId);
            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            product.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<ProductDTO>> GetLowStockAsync(int shopId)
        {
            var getProducts = await _db.Products.Where(p => p.ShopId == shopId && p.CurrentStock <= p.LowStockLimit && p.IsActive == true).ToListAsync();
            var mapProducts = getProducts.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                CurrentStock = p.CurrentStock,
                Unit = p.Unit,
                Category = p.Category,
                LowStockLimit = p.LowStockLimit,
                IsActive = p.IsActive
            }).ToList();
            return mapProducts;
        }
    }
}
