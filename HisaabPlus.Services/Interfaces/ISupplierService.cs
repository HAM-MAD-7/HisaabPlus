using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDTO>> GetAllAsync(int shopId);
        Task<SupplierDetailDTO> GetByIdAsync(int supplierId, int shopId);
        Task<SupplierDTO> AddAsync(SupplierDTO supplierDTO, int shopId);
        Task<bool> UpdateAsync(int supplierId, SupplierDTO supplierDTO, int shopId);
        Task<bool> RecordStockPurchaseAsync(StockPurchaseDTO stockPurchaseDTO, int shopId, int userId);
        Task<bool> RecordSupplierPaymentAsync(SupplierPaymentDTO supplierPaymentDTO, int shopId, int userId);
        Task<List<SupplierDTO>> GetSuppliersWithBalanceAsync(int shopId);
        Task<List<StockPurchaseResponseDTO>> GetMonthlyPurchasesAsync(int shopId);
        Task<StockPurchaseResponseDTO> GetPurchaseByIdAsync(int purchaseId, int shopId);
    }
}
