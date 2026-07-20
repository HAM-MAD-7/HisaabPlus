using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync(int shopId);
        Task<ProductDTO> GetByIdAsync(int productId, int shopId);
        Task<ProductDTO> AddAsync(ProductDTO productDTO, int shopId);
        Task<bool> UpdateAsync(int productId, ProductDTO productDTO, int shopId);
        Task<bool> DeleteAsync(int productId, int shopId);
        Task<bool> ReactivateAsync(int productId, int shopId);
        Task<List<ProductDTO>> GetLowStockAsync(int shopId);
    }
}
