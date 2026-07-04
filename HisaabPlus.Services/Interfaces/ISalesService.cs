using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface ISalesService
    {
        Task<SaleResponseDTO> CreateSaleAsync(CreateSaleDTO createSaleDTO, int shopId, int userId);
        Task<List<SaleResponseDTO>> GetTodaySalesAsync(int shopId);
        Task<SaleResponseDTO> GetSaleByIdAsync(int saleId, int shopId);
    }
}
